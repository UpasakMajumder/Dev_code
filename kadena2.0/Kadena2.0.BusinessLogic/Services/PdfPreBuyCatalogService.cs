using Kadena.BusinessLogic.Contracts;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Linq;
using System.Text;

namespace Kadena.BusinessLogic.Services
{
    public class PdfPreBuyCatalogService : IPreBuyCatalogService
    {
        private readonly IByteConverter byteConverter;
        private readonly IKenticoResourceService resourceService;
        private readonly IKenticoCampaignsProvider campaignsProvider;
        private readonly IKenticoProgramsProvider programsProvider;
        private readonly IKenticoBrandsProvider brandsProvider;
        private readonly IKenticoProductsProvider productsProvider;
        private readonly IKenticoAddressBookProvider addressBookProvider;
        private readonly IImageService imageService;
        private readonly IKenticoSiteProvider siteProvider;

        public PdfPreBuyCatalogService(IByteConverter byteConverter, IKenticoResourceService resourceService, IKenticoCampaignsProvider campaignsProvider,
            IKenticoProgramsProvider programsProvider, IKenticoBrandsProvider brandsProvider, IKenticoProductsProvider productsProvider,
            IKenticoAddressBookProvider addressBookProvider, IImageService imageService, IKenticoSiteProvider siteProvider)
        {
            this.byteConverter = byteConverter ?? throw new ArgumentNullException(nameof(byteConverter));
            this.resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            this.campaignsProvider = campaignsProvider ?? throw new ArgumentNullException(nameof(campaignsProvider));
            this.programsProvider = programsProvider ?? throw new ArgumentNullException(nameof(programsProvider));
            this.brandsProvider = brandsProvider ?? throw new ArgumentNullException(nameof(brandsProvider));
            this.productsProvider = productsProvider ?? throw new ArgumentNullException(nameof(productsProvider));
            this.addressBookProvider = addressBookProvider ?? throw new ArgumentNullException(nameof(addressBookProvider));
            this.imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            this.siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
        }

        public byte[] Generate(int campaignId)
        {
            var campaign = campaignsProvider.GetCampaign(campaignId);
            if (campaign == null)
            {
                return null;
            }

            var programs = programsProvider.GetProgramsForCampaign(campaign.CampaignID);
            var brands = brandsProvider.GetBrands(programs.Select(p => p.BrandID).ToList());
            var coverData = brands
                .Join(programs, b => b.ItemID, p => p.BrandID, (b, p) =>
                new
                {
                    b.ItemID,
                    b.BrandName,
                    p.ProgramName,
                    p.DeliveryDateToDistributors
                })
                .OrderBy(b => b.BrandName);
            //// cover page
            var programContentTemplate = resourceService.GetSiteSettingsKey(Settings.ProgramsContent);
            var coverContent = new StringBuilder();
            foreach (var cd in coverData)
            {
                coverContent
                    .Append(programContentTemplate)
                    .Replace("^ProgramName^", cd.ProgramName)
                    .Replace("^ProgramBrandName^", cd.BrandName)
                    .Replace("ProgramDate", cd.DeliveryDateToDistributors == default(DateTime)
                        ? string.Empty
                        : cd.DeliveryDateToDistributors.ToString("MMM dd, yyyy"));
            }

            var programFooterTextTemplate = resourceService.GetSiteSettingsKey(Settings.KDA_ProgramFooterText);
            var programFooterText = programFooterTextTemplate?.Replace("PROGRAMFOOTERTEXT", resourceService.GetResourceString("Kadena.Catalog.ProgramFooterText"));
            coverContent.Append(programFooterText);

            // content
            var productData = productsProvider.GetCampaignsProductSKUIDs(programs.Select(p => p.ProgramID).ToList());
            var stateGroups = addressBookProvider.GetStateGroups();
            var pdfProductsContentWithBrands = new StringBuilder();
            var closingDiv = resourceService.GetSiteSettingsKey(Settings.ClosingDIV);

            var programBrands = coverData
                .GroupBy(p => p.ItemID)
                .Select(pg => pg.First())
                .OrderBy(p => p.BrandName)
                .ToList();
            var brandHeaderTemplate = resourceService.GetSiteSettingsKey(Settings.PDFBrand);
            var brandContentTemplate = resourceService.GetSiteSettingsKey(Settings.PDFInnerHTML);
            foreach (var pb in programBrands)
            {
                var catalogList = productData
                     .Where(x => x.BrandId == pb.ItemID)
                     .ToList();
                if (catalogList.Count > 0)
                {
                    var brandContent = new StringBuilder();
                    foreach (var p in catalogList)
                    {
                        var stateInfo = stateGroups.Where(s => s.Id == p.StateId).FirstOrDefault();
                        brandContent
                            .Append(brandContentTemplate)
                            .Replace("IMAGEGUID", siteProvider.GetAbsoluteUrl(imageService.GetThumbnailLink(p.ImageUrl)))
                            .Replace("PRODUCTPARTNUMBER", p.POSNumber)
                            .Replace("PRODUCTBRANDNAME", pb.BrandName)
                            .Replace("PRODUCTSHORTDESCRIPTION", p.ProductName)
                            .Replace("PRODUCTDESCRIPTION", p.Description)
                            .Replace("PRODUCTVALIDSTATES", stateInfo?.States)
                            .Replace("PRODUCTCOSTBUNDLE", siteProvider.GetFormattedPrice(p.EstimatedPrice))
                            .Replace("PRODUCTBUNDLEQUANTITY", p.NumberOfItemsInPackage.ToString())
                            .Replace("PRODUCTEXPIRYDATE", p.ValidTo != default(DateTime) ? p.ValidTo.ToString("MMM dd, yyyy") : string.Empty);

                    }

                    pdfProductsContentWithBrands
                        .Append($"{brandHeaderTemplate}{brandContent}{closingDiv}")
                        .Replace("^PROGRAMNAME^", pb.ProgramName)
                        .Replace("^BrandName^", pb.BrandName);
                }
            }

            var coverHeader = resourceService.GetSiteSettingsKey(Settings.ProductsPDFHeader)?
                .Replace("CAMPAIGNNAME", campaign.Name)
                .Replace("OrderStartDate", campaign.StartDate == default(DateTime) ? string.Empty : campaign.StartDate.ToString("MMM dd, yyyy"))
                .Replace("OrderEndDate", campaign.EndDate == default(DateTime) ? string.Empty : campaign.EndDate.ToString("MMM dd, yyyy"));
            var pdfClosingDivs = resourceService.GetSiteSettingsKey(Settings.PdfEndingTags);
            var html = pdfProductsContentWithBrands + pdfClosingDivs;
            var cover = coverHeader + coverContent + closingDiv;

            return byteConverter.GetBytes(html, cover);
        }
    }
}
