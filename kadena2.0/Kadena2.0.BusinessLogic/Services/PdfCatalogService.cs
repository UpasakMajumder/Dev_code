using Kadena.BusinessLogic.Contracts;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using NReco.PdfGenerator;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class PdfCatalogService : ICatalogService
    {
        private readonly IKenticoResourceService resourceService;
        private readonly HtmlToPdfConverter pdfConverter;

        public PdfCatalogService(IKenticoResourceService resourceService, HtmlToPdfConverter pdfConverter)
        {
            this.resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            this.pdfConverter = pdfConverter ?? throw new ArgumentNullException(nameof(pdfConverter));
        }

        public byte[] GetPdfBytes(string contentHtml, string coverHtml)
        {
            pdfConverter.License.SetLicenseKey(resourceService.GetSiteSettingsKey(Settings.KDA_NRecoOwner), resourceService.GetSiteSettingsKey(Settings.KDA_NRecoKey));
            pdfConverter.LowQuality = resourceService.GetSiteSettingsKey<bool>(Settings.KDA_NRecoLowQuality);
            return pdfConverter.GeneratePdf(contentHtml, coverHtml);
        }
    }
}
