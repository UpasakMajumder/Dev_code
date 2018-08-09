using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Common;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.EmailNotifications;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kadena.Models.SiteSettings;
using Kadena.Models.TableSorting;

public partial class CMSWebParts_Kadena_Product_InboundTracking : CMSAbstractWebPart
{
    private OrderBy OrderBy => TableSortingHelper.ExtractOrderByFromUrl(CMSHttpContext.Current.Request.RawUrl);

    private class InboundTrackingGridItem
    {
        public int SKUID { get; set; }
        public string SKUNumber { get; set; }
        public string SKUName { get; set; }
        public string ProductCustomerReferenceNumber { get; set; }
        public int QtyOrdered { get; set; }
        public int DemandGoal { get; set; }
        public int QtyReceived { get; set; }
        public int QtyProduced { get; set; }
        public int Overage { get; set; }
        public string Vendor { get; set; }
        public string ExpArrivalToCenveo { get; set; }
        public string DeliveryToDistBy { get; set; }
        public string ShippedToDist { get; set; }
        public string CenveoComments { get; set; }
        public string TweComments { get; set; }
        public double ActualPrice { get; set; }
        public bool Status { get; set; }
        public bool IsClosed { get; set; }
        public string ItemSpec { get; set; }
        public string CustomItemSpecs { get; set; }
        public double EstimatedPrice { get; set; }
    }

    #region Properties

    /// <summary>
    /// Refresh bustton resource string
    /// </summary>
    public string RefreshButtonText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.RefreshButtonText"), string.Empty);
        }
        set
        {
            SetValue("RefreshButtonText", value);
        }
    }

    /// <summary>
    /// Export Button Resource string
    /// </summary>
    public string ExportButtonText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.ExportButtonText"), string.Empty);
        }
        set
        {
            SetValue("ExportButtonText", value);
        }
    }

    /// <summary>
    /// SkuNumber Resource string
    /// </summary>
    public string SKUNumberHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.SKUNumberHeaderText"), string.Empty);
        }
        set
        {
            SetValue("SKUNumberHeaderText", value);
        }
    }

    /// <summary>
    /// Skuname resource string
    /// </summary>
    public string SKUNameheaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.SKUNameheaderText"), string.Empty);
        }
        set
        {
            SetValue("SKUNameheaderText", value);
        }
    }

    /// <summary>
    /// Skuname resource string
    /// </summary>
    public string CustomerReferenceNumberHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.CustomerReferenceNumberHeaderText"), string.Empty);
        }
        set
        {
            SetValue("CustomerReferenceNumberHeaderText", value);
        }
    }

    /// <summary>
    /// Qty ordered resource string
    /// </summary>
    public string QtyOrderedHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.QtyOrderedHeaderText"), string.Empty);
        }
        set
        {
            SetValue("QtyOrderedHeaderText", value);
        }
    }

    /// <summary>
    /// Demand resource string
    /// </summary>
    public string DemandHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.DemandHeaderText"), string.Empty);
        }
        set
        {
            SetValue("DemandHeaderText", value);
        }
    }

    /// <summary>
    /// Qty Recieved resource string
    /// </summary>
    public string QtyReceivedHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.QtyReceivedHeaderText"), string.Empty);
        }
        set
        {
            SetValue("QtyReceivedHeaderText", value);
        }
    }

    /// <summary>
    /// Qty Produced resource string
    /// </summary>
    public string QtyProdusedHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.QtyProdusedHeaderText"), string.Empty);
        }
        set
        {
            SetValue("QtyProdusedHeaderText", value);
        }
    }

    /// <summary>
    /// Overage resource string
    /// </summary>
    public string OverageHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.OverageHeaderText"), string.Empty);
        }
        set
        {
            SetValue("OverageHeaderText", value);
        }
    }

    /// <summary>
    /// Get Vendor resource string
    /// </summary>
    public string VendorHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.VendorHeaderText"), string.Empty);
        }
        set
        {
            SetValue("VendorHeaderText", value);
        }
    }

    /// <summary>
    /// ExpArraival to cenveo resource string
    /// </summary>
    public string ExpArraivalToCenveoHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.ExpArraivalToCenveoHeaderText"), string.Empty);
        }
        set
        {
            SetValue("ExpArraivalToCenveoHeaderText", value);
        }
    }

    /// <summary>
    /// Deliver to dist by resource string
    /// </summary>
    public string DeliveryToDistByHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.DeliveryToDistByHeaderText"), string.Empty);
        }
        set
        {
            SetValue("DeliveryToDistByHeaderText", value);
        }
    }

    /// <summary>
    /// Shipped to dist resource string
    /// </summary>
    public string ShippedToDistHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.ShippedToDistHeaderText"), string.Empty);
        }
        set
        {
            SetValue("ShippedToDistHeaderText", value);
        }
    }

    /// <summary>
    /// Cenveo Comments resource string
    /// </summary>
    public string CenveoCommentsHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.CenveoCommentsHeaderText"), string.Empty);
        }
        set
        {
            SetValue("CenveoCommentsHeaderText", value);
        }
    }

    /// <summary>
    /// Twe Comments resource string
    /// </summary>
    public string TWECommentsHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.TWECommentsHeaderText"), string.Empty);
        }
        set
        {
            SetValue("TWECommentsHeaderText", value);
        }
    }

    /// <summary>
    /// Actual price resource string
    /// </summary>
    public string ActualPriceHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.ActualPriceHeaderText"), string.Empty);
        }
        set
        {
            SetValue("ActualPriceHeaderText", value);
        }
    }

    /// <summary>
    /// Status Resource string
    /// </summary>
    public string StatusHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.StatusHeaderText"), string.Empty);
        }
        set
        {
            SetValue("StatusHeaderText", value);
        }
    }

    /// <summary>
    /// Edit link resource string
    /// </summary>
    public string EditLinkText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.EditLinkText"), string.Empty);
        }
        set
        {
            SetValue("EditLinkText", value);
        }
    }

    /// <summary>
    /// Cancel link resource string
    /// </summary>
    public string CancelText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.CancelText"), string.Empty);
        }
        set
        {
            SetValue("CancelText", value);
        }
    }

    /// <summary>
    /// Update link resource string
    /// </summary>
    public string UpdateText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.UpdateText"), string.Empty);
        }
        set
        {
            SetValue("UpdateText", value);
        }
    }

    /// <summary>
    /// Get Active text resource string
    /// </summary>
    public string ActiveText
    {
        get
        {
            return ResHelper.GetString("KDA.Common.Status.Active");
        }
        set
        {
            SetValue("ActiveText", value);
        }
    }

    /// <summary>
    /// Get InActive Resource string
    /// </summary>
    public string InActiveText
    {
        get
        {
            return ResHelper.GetString("KDA.Common.Status.Inactive");
        }
        set
        {
            SetValue("InActiveText", value);
        }
    }

    /// <summary>
    /// No Data resource string
    /// </summary>
    public string NoDataText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.NoDataText"), string.Empty);
        }
        set
        {
            SetValue("NoDataText", value);
        }
    }

    /// <summary>
    /// Get the No Access Page
    /// </summary>
    public string NoAccessPage
    {
        get
        {
            string noAccessPath = string.Empty;
            try
            {
                Guid nodeGUID = ValidationHelper.GetGuid(SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_NoAccessPage"), Guid.Empty);
                {
                    if (!nodeGUID.Equals(Guid.Empty))
                    {
                        var document = new TreeProvider().SelectSingleNode(nodeGUID, CurrentDocument.DocumentCulture, CurrentSite.SiteName);
                        if (document != null)
                        {
                            noAccessPath = document.DocumentUrlPath;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Get no Access Page", "NoAccessPage", ex, CurrentSite.SiteID, ex.Message);
            }
            return noAccessPath;
        }
        set
        {
            SetValue("NoAccessPage", value);
        }
    }

    /// <summary>
    /// Get Action resource string
    /// </summary>
    public string ActionsText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.ActionsText"), string.Empty);
        }
        set
        {
            SetValue("ActionsText", value);
        }
    }

    /// <summary>
    /// Items specs resource string
    /// </summary>
    public string ItemSpecHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.ItemSpecHeaderText"), string.Empty);
        }
        set
        {
            SetValue("ItemSpecHeaderText", value);
        }
    }

    /// <summary>
    /// Items specs resource string
    /// </summary>
    public string EstimatedPriceHeaderText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Inbound.EstimatedPriceHeaderText"), string.Empty);
        }
        set
        {
            SetValue("EstimatedPriceHeaderText", value);
        }
    }

    #endregion Properties

    #region "Methods"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }

    /// <summary>
    /// Bind Resource strings
    /// </summary>
    public void BindLabels()
    {
        gdvInboundProducts.Columns[0].HeaderText = SKUNumberHeaderText;
        gdvInboundProducts.Columns[1].HeaderText = SKUNameheaderText;
        gdvInboundProducts.Columns[2].HeaderText = CustomerReferenceNumberHeaderText;
        gdvInboundProducts.Columns[3].HeaderText = ItemSpecHeaderText;
        gdvInboundProducts.Columns[4].HeaderText = QtyOrderedHeaderText;
        gdvInboundProducts.Columns[5].HeaderText = DemandHeaderText;
        gdvInboundProducts.Columns[6].HeaderText = QtyReceivedHeaderText;
        gdvInboundProducts.Columns[7].HeaderText = QtyProdusedHeaderText;
        gdvInboundProducts.Columns[8].HeaderText = OverageHeaderText;
        gdvInboundProducts.Columns[9].HeaderText = VendorHeaderText;
        gdvInboundProducts.Columns[10].HeaderText = ExpArraivalToCenveoHeaderText;
        gdvInboundProducts.Columns[11].HeaderText = DeliveryToDistByHeaderText;
        gdvInboundProducts.Columns[12].HeaderText = ShippedToDistHeaderText;
        gdvInboundProducts.Columns[13].HeaderText = CenveoCommentsHeaderText;
        gdvInboundProducts.Columns[14].HeaderText = TWECommentsHeaderText;
        gdvInboundProducts.Columns[15].HeaderText = ActualPriceHeaderText;
        gdvInboundProducts.Columns[16].HeaderText = EstimatedPriceHeaderText;
        gdvInboundProducts.Columns[17].HeaderText = StatusHeaderText;
        gdvInboundProducts.Columns[18].HeaderText = ActionsText;
        btnExport.Text = ExportButtonText;
        btnRefresh.Text = RefreshButtonText;
        btnClose.Text = ResHelper.GetString("Kadena.Inbound.CloseButtonText");
    }

    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (!this.StopProcessing)
        {
            string gAdminRoleName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_GlobalAminRoleName");
            if (!string.IsNullOrEmpty(gAdminRoleName) && !string.IsNullOrWhiteSpace(gAdminRoleName))
            {
                if (CurrentUser.IsInRole(gAdminRoleName, CurrentSiteName))
                {
                    if (!IsPostBack)
                    {
                        divSelectCampaign.Visible = true;
                        BindCampaigns();
                        string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectProgramText"), string.Empty);
                        ddlProgram.Items.Insert(0, new ListItem(selectText, "0"));
                        BindLabels();
                    }
                }
                else
                {
                    Response.Redirect(NoAccessPage);
                }
            }
            else
            {
                Response.Redirect(NoAccessPage);
            }
        }
    }

    /// <summary>
    /// Reloads the control data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }

    /// <summary>
    /// Get the Campaign Closed or not
    /// </summary>
    /// <param name="programID"></param>
    /// <returns></returns>
    public bool IsCampaignClosed(int programID)
    {
        bool isClosed = false;
        try
        {
            Program program = ProgramProvider.GetPrograms()
                 .WhereEquals("ProgramID", programID)
                 .Columns("CampaignID")
                 .FirstOrDefault();
            if (program != null)
            {
                Campaign campaign = CampaignProvider.GetCampaigns()
                    .WhereEquals("CampaignID", program.CampaignID).Columns("CloseCampaign")
                    .FirstOrDefault();
                if (campaign != null)
                {
                    isClosed = campaign.CloseCampaign;
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Checking campaign Closed", "IsCampaignClosed()", ex, CurrentSite.SiteID, ex.Message);
        }
        return isClosed;
    }

    /// <summary>
    /// Get Product details
    /// </summary>
    /// <returns></returns>
    public List<CampaignsProduct> GetProductDetails()
    {
        List<CampaignsProduct> productsDetails = new List<CampaignsProduct>();
        try
        {
            List<int> programIds = new List<int>();
            if (ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)) != default(int))
            {
                if (IsCampaignClosed(ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int))))
                {
                    programIds.Add(ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)));
                }
            }
            else
            {
                programIds = GetProgramIDs(ValidationHelper.GetInteger(ddlCampaign.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)));
            }
            if (!DataHelper.DataSourceIsEmpty(programIds))
            {
                productsDetails = CampaignsProductProvider.GetCampaignsProducts()
                                  .WhereIn("ProgramID", programIds)
                                  .ToList();
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Get Product Details", "GetProductDetails()", ex, CurrentSite.SiteID, ex.Message);
        }
        return productsDetails;
    }

    /// <summary>
    /// Get Sku Details
    /// </summary>
    /// <param name="productsDetails"></param>
    /// <returns></returns>
    public List<SKUInfo> GetSkuDetails(List<CampaignsProduct> productsDetails)
    {
        List<SKUInfo> skuDetails = new List<SKUInfo>();
        try
        {
            List<int> skuIds = new List<int>();
            if (!DataHelper.DataSourceIsEmpty(productsDetails))
            {
                foreach (var product in productsDetails)
                {
                    skuIds.Add(product.NodeSKUID);
                }
            }
            if (!DataHelper.DataSourceIsEmpty(skuIds))
            {
                skuDetails = SKUInfoProvider.GetSKUs()
                               .WhereIn("SKUID", skuIds)
                               .Columns("SKUNumber,SKUName,SKUPrice,SKUEnabled,SKUID")
                               .ToList();
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Get Sku details", "GetSkuDetails()", ex, CurrentSite.SiteID, ex.Message);
        }
        return skuDetails;
    }

    /// <summary>
    /// Get Products
    /// </summary>
    public void GetProducts()
    {
        try
        {
            List<CampaignsProduct> productsDetails = GetProductDetails();
            List<SKUInfo> skuDetails = GetSkuDetails(productsDetails);
            if (!DataHelper.DataSourceIsEmpty(skuDetails) && !DataHelper.DataSourceIsEmpty(productsDetails))
            {
                var productAndSKUDetails = productsDetails
                                  .Join(skuDetails, x => x.NodeSKUID, y => y.SKUID, 
                                  (x, y) => new
                                  {
                                      x.ProgramID, x.CategoryID, x.CustomItemSpecs, x.ItemSpecs,
                                      y.SKUNumber, y.SKUName, y.SKUPrice, y.SKUEnabled, y.SKUID,
                                      x.EstimatedPrice, x.Product.ProductCustomerReferenceNumber
                                  })
                                  .ToList();
                var inboundDetails = CustomTableItemProvider.GetItems<InboundTrackingItem>()
                    .WhereIn("SKUID", skuDetails.Select(x => x.SKUID).ToList()).ToList();
                var data = from product in productAndSKUDetails
                                 join inbound in inboundDetails
                                 on product.SKUID equals inbound.SKUID into alldata
                                 from newData in alldata.DefaultIfEmpty()
                                 select new InboundTrackingGridItem
                                 {
                                     SKUID = product.SKUID,
                                     SKUNumber = product.SKUNumber,
                                     SKUName = product.SKUName,
                                     ProductCustomerReferenceNumber = product.ProductCustomerReferenceNumber,
                                     QtyOrdered = newData?.QtyOrdered ?? default(int),
                                     DemandGoal = newData?.DemandGoal ?? default(int),
                                     QtyReceived = newData?.QtyReceived ?? default(int),
                                     QtyProduced = newData?.QtyProduced ?? default(int),
                                     Overage = newData?.Overage ?? default(int),
                                     Vendor = newData?.Vendor ?? string.Empty,
                                     ExpArrivalToCenveo = newData?.ExpArrivalToCenveo ?? string.Empty,
                                     DeliveryToDistBy = newData?.DeliveryToDistBy ?? string.Empty,
                                     ShippedToDist = newData?.ShippedToDist ?? string.Empty,
                                     CenveoComments = newData?.CenveoComments ?? string.Empty,
                                     TweComments = newData?.TweComments ?? string.Empty,
                                     ActualPrice = newData?.ActualPrice ?? default(double),
                                     Status = product.SKUEnabled,
                                     IsClosed = IsIBTFClosed(product?.ProgramID ?? 0),
                                     ItemSpec = (product?.ItemSpecs ?? string.Empty) == "0" ? product?.CustomItemSpecs ?? string.Empty : (product?.ItemSpecs ?? string.Empty) == "0" ? string.Empty : GetItemSpecs(product?.ItemSpecs ?? string.Empty),
                                     CustomItemSpecs = product.CustomItemSpecs ?? string.Empty,
                                     EstimatedPrice = product?.EstimatedPrice ?? default(double)
                                 };

                data = ApplyOrderBy(data);

                var allDetails = data.ToList();

                var closeButtonStatus = allDetails.Select(x => x.IsClosed).FirstOrDefault();
                if (!DataHelper.DataSourceIsEmpty(allDetails))
                {
                    divNodatafound.Visible = false;
                    gdvInboundProducts.DataSource = allDetails;
                    gdvInboundProducts.DataBind();
                    gdvInboundProducts.Visible = true;
                    btnClose.Enabled = !closeButtonStatus;
                    btnRefresh.Enabled = true;
                    btnExport.Enabled = true;
                }
                else
                {
                    btnClose.Enabled = false;
                    btnRefresh.Enabled = false;
                    btnExport.Enabled = false;
                    divNodatafound.Visible = true;
                    gdvInboundProducts.Visible = false;
                    BindLabels();
                    gdvInboundProducts.DataBind();
                }
            }
            else
            {
                btnClose.Enabled = false;
                btnRefresh.Enabled = false;
                btnExport.Enabled = false;
                divNodatafound.Visible = true;
                BindLabels();
                gdvInboundProducts.DataBind();
                gdvInboundProducts.Visible = false;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Get Products", "GetProducts()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    private IEnumerable<InboundTrackingGridItem> ApplyOrderBy(IEnumerable<InboundTrackingGridItem> allDetails)
    {
        var orderBy = OrderBy;

        if (orderBy.IsEmpty)
        {
            return allDetails;
        }

        var property = typeof(InboundTrackingGridItem)
            .GetProperties()
            .FirstOrDefault(p => string.Compare(p.Name, orderBy.Column, ignoreCase: true) == 0);
        if (property == null)
        {
            return allDetails;
        }

        Func<InboundTrackingGridItem, object> keySelector = (itg) => property.GetValue(itg, null);
        
        return orderBy.Direction == TableSortingHelper.DirectionDescending
            ? allDetails.OrderByDescending(keySelector)
            : allDetails.OrderBy(keySelector);
    }

    /// <summary>
    /// Get the Program Ids in Open Campaign
    /// </summary>
    /// <returns></returns>
    public List<int> GetProgramIDs(int campaignID = default(int), int programID = default(int))
    {
        List<int> programIds = new List<int>();
        try
        {
            List<Campaign> campaigns = new List<Campaign>();
            if (campaignID != default(int))
            {
                campaigns = CampaignProvider.GetCampaigns()
                    .WhereEquals("CloseCampaign", true)
                    .Columns("CampaignID")
                    .WhereEquals("CampaignID", campaignID)
                    .ToList();
            }
            else
            {
                campaigns = CampaignProvider.GetCampaigns()
                             .Columns("CampaignID")
                             .WhereEquals("CloseCampaign", true)
                             .ToList();
            }
            if (!DataHelper.DataSourceIsEmpty(campaigns))
            {
                foreach (var campaign in campaigns)
                {
                    var programs = ProgramProvider.GetPrograms()
                      .WhereEquals("CampaignID", campaign.CampaignID)
                      .Columns("ProgramID")
                      .ToList();
                    if (!DataHelper.DataSourceIsEmpty(programs))
                    {
                        foreach (var program in programs)
                        {
                            programIds.Add(program.ProgramID);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Get ProgramsIDs from CVampaign", "GetProgramIDs()", ex, CurrentSite.SiteID, ex.Message);
        }
        return programIds;
    }

    /// <summary>
    /// Bind the Campaigns to dropdown
    /// </summary>
    public void BindCampaigns()
    {
        try
        {
            var camapaigns = CampaignProvider.GetCampaigns().Columns("CampaignID,Name")
                                .Where(new WhereCondition().WhereEquals("CloseCampaign", true))
                                .WhereEquals("NodeSiteID", CurrentSite.SiteID).OrderBy(x => x.Name).ToList();
            if (!DataHelper.DataSourceIsEmpty(camapaigns))
            {
                ddlCampaign.DataSource = camapaigns;
                ddlCampaign.DataTextField = "Name";
                ddlCampaign.DataValueField = "CampaignID";
                ddlCampaign.DataBind();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectCampaignText"), string.Empty);
                ddlCampaign.Items.Insert(0, new ListItem(selectText, "0"));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Bind Campaigns to dropdown", "BindCampaigns()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Bind Programs to dropdown
    /// </summary>
    /// <param name="campaignID"></param>
    public void BindPrograms(int campaignID)
    {
        try
        {
            if (campaignID != default(int))
            {
                List<Program> programs = ProgramProvider.GetPrograms()
                                   .WhereEquals("CampaignID", campaignID)
                                   .Columns("ProgramID,ProgramName")
                                   .ToList();
                if (!DataHelper.DataSourceIsEmpty(programs))
                {
                    ddlProgram.DataSource = programs;
                    ddlProgram.DataTextField = "ProgramName";
                    ddlProgram.DataValueField = "ProgramID";
                    ddlProgram.DataBind();
                    string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectProgramText"), string.Empty);
                    ddlProgram.Items.Insert(0, new ListItem(selectText, "0"));
                }
                else
                {
                    ddlProgram.Items.Clear();
                    string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectProgramText"), string.Empty);
                    ddlProgram.Items.Insert(0, new ListItem(selectText, "0"));
                }
            }
            else
            {
                ddlProgram.Items.Clear();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectProgramText"), string.Empty);
                ddlProgram.Items.Insert(0, new ListItem(selectText, "0"));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Bind Programs to dropdown", "BindPrograms()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Editing the Products
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void inboundProducts_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gdvInboundProducts.EditIndex = e.NewEditIndex;
        GetProducts();
    }

    /// <summary>
    /// Updating the products
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void inboundProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string skuid = ((HiddenField)gdvInboundProducts.Rows[e.RowIndex].FindControl("hfSKUID")).Value;
            bool isExist = true;
            InboundTrackingItem inboundData = CustomTableItemProvider.GetItems<InboundTrackingItem>().WhereEquals("SKUID", ValidationHelper.GetInteger(skuid, default(int))).FirstOrDefault();
            if (inboundData == null)
            {
                inboundData = new InboundTrackingItem();
                isExist = false;
            }
            inboundData.SKUID = ValidationHelper.GetInteger(skuid, default(int));
            inboundData.QtyOrdered = ValidationHelper.GetInteger(((Label)gdvInboundProducts.Rows[e.RowIndex].FindControl("lblQtyOrdered")).Text, default(int));
            inboundData.DemandGoal = ValidationHelper.GetInteger(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtDemandGoal")).Text, default(int));
            inboundData.QtyReceived = ValidationHelper.GetInteger(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtQtyReceived")).Text, default(int));
            inboundData.QtyProduced = ValidationHelper.GetInteger(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtQtyProduced")).Text, default(int));
            inboundData.Overage = inboundData.QtyReceived - inboundData.QtyOrdered;
            inboundData.Vendor = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtVendor")).Text, string.Empty);
            inboundData.ExpArrivalToCenveo = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtExpArrivalToCenveo")).Text, string.Empty);
            inboundData.DeliveryToDistBy = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtDeliveryToDistBy")).Text, string.Empty);
            inboundData.ShippedToDist = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtShippedToDist")).Text, string.Empty);
            inboundData.CenveoComments = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtCenveoComments")).Text, string.Empty);
            inboundData.TweComments = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtTweComments")).Text, string.Empty);
            inboundData.ActualPrice = ValidationHelper.GetDouble(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtActualPrice")).Text, default(double));
            if (!isExist)
            {
                inboundData.Insert();
            }
            else
            {
                inboundData.Update();
            }
            UpdateSkuTable(ValidationHelper.GetInteger(skuid, default(int)), ValidationHelper.GetDouble(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtActualPrice")).Text, default(double)), ValidationHelper.GetInteger(((DropDownList)gdvInboundProducts.Rows[e.RowIndex].FindControl("ddlStatus")).SelectedValue, default(int)));
            gdvInboundProducts.EditIndex = -1;
            GetProducts();
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Updating Products", "inboundProducts_RowUpdating()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Update the SKU table
    /// </summary>
    /// <param name="skuID"></param>
    /// <param name="actualPrice"></param>
    /// <param name="statusID"></param>
    public void UpdateSkuTable(int skuID, double actualPrice, int statusID)
    {
        try
        {
            SKUInfo updateProduct = SKUInfoProvider.GetSKUs().WhereEquals("SKUID", ValidationHelper.GetInteger(skuID, default(int))).FirstObject;
            if (updateProduct != null)
            {
                updateProduct.SKUPrice = actualPrice;
                updateProduct.SKUEnabled = statusID == 1 ? true : false;
                SKUInfoProvider.SetSKUInfo(updateProduct);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Updating Skutable", "UpdateSkuTable()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Canceling edit product
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gdvInboundProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gdvInboundProducts.EditIndex = -1;
        GetProducts();
    }

    /// <summary>
    /// Bind the Products by CampaignID
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCampaign_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCampaign.SelectedIndex != 0)
            {
                inBoundGrid.Visible = true;
                ddlProgram.Enabled = true;
                divSelectCampaign.Visible = false;
                BindPrograms(ValidationHelper.GetInteger(ddlCampaign.SelectedValue, default(int)));
                GetProducts();
            }
            else
            {
                ddlProgram.Enabled = false;
                divNodatafound.Visible = false;
                divSelectCampaign.Visible = true;
                inBoundGrid.Visible = false;
                btnClose.Enabled = false;
                btnRefresh.Enabled = false;
                btnExport.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Campaign selection change event", "ddlCampaign_SelectedIndexChanged()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Bind Products by ProgramID
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetProducts();
    }

    /// <summary>
    /// Refresh the page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }

    /// <summary>
    /// Export Products data to Excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            gdvInboundProducts.Columns[gdvInboundProducts.Columns.Count - 1].Visible = false;
            gdvInboundProducts.AllowPaging = false;
            gdvInboundProducts.EditIndex = -1;
            GetProducts();
            for (int i = 0; i < gdvInboundProducts.Rows.Count; i++)
            {
                var control = (Label)gdvInboundProducts.Rows[i].FindControl("lblItemSpecs");
                control.Text = control.ToolTip;
            }
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=InboudTracking.xls");
            Response.ContentType = ContentTypes.Xlsx;
            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();
            Controls.Add(form);
            form.Controls.Add(gdvInboundProducts);
            form.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
            gdvInboundProducts.Columns[gdvInboundProducts.Columns.Count - 1].Visible = true;
            gdvInboundProducts.AllowPaging = true;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Export data to excel", "btnExport_Click()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Adding the pagination for products
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gdvInboundProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvInboundProducts.PageIndex = e.NewPageIndex;
        GetProducts();
    }

    /// <summary>
    /// Binding the data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gdvInboundProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
                ddlStatus.Items.Insert(0, new ListItem(ResHelper.GetString("KDA.Common.Status.Active"), "1"));
                ddlStatus.Items.Insert(1, new ListItem(ResHelper.GetString("KDA.Common.Status.Inactive"), "0"));
                string inActiveText = ResHelper.GetString("KDA.Common.Status.Inactive");
                string activeText = ResHelper.GetString("KDA.Common.Status.Active");
                string lblStatus = (e.Row.FindControl("lbleditStatus") as Label).Text;
                if (lblStatus == inActiveText)
                {
                    ddlStatus.SelectedValue = "0";
                }
                if (lblStatus == activeText)
                {
                    ddlStatus.SelectedValue = "1";
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Binding Status", "gdvInboundProducts_RowDataBound()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Pop up Yes click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void popUpYes_ServerClick(object sender, EventArgs e)
    {
        try
        {
            int campaignID = ValidationHelper.GetInteger(ddlCampaign.SelectedValue, default(int));
            var client = DIContainer.Resolve<IKenticoCampaignsProvider>();
            bool result = client.CloseCampaignIBTF(campaignID);
            var emailNotificationTemplate = DIContainer.Resolve<IKenticoResourceService>().GetSiteSettingsKey(Settings.KDA_IBTFFinalizeEmailTemplate);
            if (result)
            {
                DIContainer.Resolve<IIBTFService>().UpdateRemainingBudget(campaignID);
                ProductEmailNotifications.GetCampaignOrders(campaignID, emailNotificationTemplate);
                btnClose.Enabled = false;
                GetProducts();
                Response.Cookies["status"].Value = QueryStringStatus.Updated;
                Response.Cookies["status"].HttpOnly = false;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CloseButtonYesClick", "popUpYes_ServerClick()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// checking if Inbound form is finalized
    /// </summary>
    /// <param name="programID"></param>
    /// <returns></returns>
    public bool IsIBTFClosed(int? programID)
    {
        try
        {
            var program = ProgramProvider.GetPrograms().WhereEquals("ProgramID", programID).Column("CampaignID").FirstOrDefault();
            var campaign = CampaignProvider.GetCampaigns().WhereEquals("CampaignID", program?.CampaignID ?? 0).FirstOrDefault();
            if (campaign != null)
            {
                return campaign?.IBTFFinalized ?? false;
            }
            return false;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("IsIBTFClosed", "checking if IBTF of particular campaign is closed", ex, CurrentSite.SiteID, ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Gets the item specs from data base
    /// </summary>
    /// <returns>returns value based on itemid</returns>
    public string GetItemSpecs(string itemID)
    {
        try
        {
            var itemSpecs = CustomTableItemProvider.GetItems(ProductItemSpecsItem.CLASS_NAME).WhereEquals("ItemID", ValidationHelper.GetInteger(itemID, 0))
                .Columns("ItemSpec").FirstOrDefault();
            if (itemSpecs != null)
            {
                return itemSpecs.GetValue("ItemSpec", string.Empty);
            }
            return string.Empty;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "GetItemSpecs()", ex, CurrentSite.SiteID, ex.Message);
            return string.Empty;
        }
    }
}

#endregion "Methods"

