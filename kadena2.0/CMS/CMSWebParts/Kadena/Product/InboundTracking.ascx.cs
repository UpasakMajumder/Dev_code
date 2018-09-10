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

    private int SelectedCampaignId => ValidationHelper.GetInteger(Request.QueryString["campaignId"], 0);
    private int SelectedProgramId => ValidationHelper.GetInteger(Request.QueryString["programId"], 0);

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

    protected string GetTableSortingColumnURL(string column) =>
        TableSortingHelper.GetColumnURL(column, CMSHttpContext.Current.Request.RawUrl);

    protected string GetTableSortingColumnDirection(string column) =>
        TableSortingHelper.GetColumnDirection(column, CMSHttpContext.Current.Request.RawUrl);

    /// <summary>
    /// Bind Resource strings
    /// </summary>
    public void BindLabels()
    {
        btnExport.Text = ResHelper.GetString("Kadena.Inbound.ExportButtonText");
        btnRefresh.Text = ResHelper.GetString("Kadena.Inbound.RefreshButtonText");
        btnClose.Text = ResHelper.GetString("Kadena.Inbound.CloseButtonText");
    }

    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            return;
        }

        var gAdminRoleName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_GlobalAminRoleName");
        if (string.IsNullOrWhiteSpace(gAdminRoleName) || !CurrentUser.IsInRole(gAdminRoleName, CurrentSiteName))
        {
            Response.Redirect(NoAccessPage);
        }

        BindCampaigns();
        BindPrograms(SelectedCampaignId);
        BindLabels();

        var campaignSelected = SelectedCampaignId > 0;
        divSelectCampaign.Visible = !campaignSelected;
        ddlProgram.Enabled = campaignSelected;
        inBoundGrid.Visible = campaignSelected;
        btnClose.Enabled = campaignSelected;
        btnRefresh.Enabled = campaignSelected;
        btnExport.Enabled = campaignSelected;

        if (!IsPostBack && campaignSelected)
        {
            BindProducts();
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
                programIds = GetProgramIDs(ValidationHelper.GetInteger(ddlCampaign.SelectedValue, default(int)));
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
    public void BindProducts()
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
    public List<int> GetProgramIDs(int campaignID = default(int))
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
            var campaigns = CampaignProvider.GetCampaigns()
                .Columns("CampaignID,Name")
                .Where(new WhereCondition().WhereEquals("CloseCampaign", true))
                .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                .OrderBy(x => x.Name)
                .ToList();
            if (!DataHelper.DataSourceIsEmpty(campaigns))
            {
                ddlCampaign.DataSource = campaigns;
                ddlCampaign.DataTextField = "Name";
                ddlCampaign.DataValueField = "CampaignID";
                ddlCampaign.DataBind();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectCampaignText"), string.Empty);
                ddlCampaign.Items.Insert(0, new ListItem(selectText, "0"));

                if (campaigns.Any(c => c.CampaignID == SelectedCampaignId))
                {
                    ddlCampaign.SelectedValue = SelectedCampaignId.ToString();
                }
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
            ddlProgram.Items.Clear();

            if (campaignID > 0)
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

                    if (programs.Any(c => c.ProgramID == SelectedProgramId))
                    {
                        ddlProgram.SelectedValue = SelectedProgramId.ToString();
                    }
                }
            }

            string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectProgramText"), string.Empty);
            ddlProgram.Items.Insert(0, new ListItem(selectText, "0"));
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
        BindProducts();
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
            BindProducts();
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
        BindProducts();
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
            BindProducts();
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
        BindProducts();
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
                BindProducts();
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

