using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public partial class CMSWebParts_Kadena_Product_InboundTracking : CMSAbstractWebPart
{
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
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (!this.StopProcessing)
        {
            if (!IsPostBack)
            {
                BindCampaigns();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectProgramText"), string.Empty);
                ddlProgram.Items.Insert(0, new ListItem(selectText, "0"));
                GetProducts();
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
    /// Get Products
    /// </summary>
    public void GetProducts()
    {
        try
        {
            var allDetails = GetAllData();
            if (allDetails != null)
            {
                gdvInboundProducts.DataSource = allDetails;
                gdvInboundProducts.DataBind();
            }
            else
            {
                gdvInboundProducts.DataBind();
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Get Products", "GetProducts()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    public object GetAllData()
    {
        List<CampaignsProduct> productsDetails = new List<CampaignsProduct>();
        List<int> programIds = new List<int>();
        if (ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)) != default(int))
        {
            programIds.Add(ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)));
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
            var skuDetails = SKUInfoProvider.GetSKUs()
                            .WhereIn("SKUID", skuIds)
                            .Columns("SKUNumber,SKUName,SKUPrice,SKUEnabled,SKUID")
                            .ToList();

            if (!DataHelper.DataSourceIsEmpty(skuIds) && !DataHelper.DataSourceIsEmpty(productsDetails))
            {
                var productAndSKUDetails = productsDetails
                                  .Join(skuDetails, x => x.NodeSKUID, y => y.SKUID, (x, y) => new { x.ProgramID, x.CategoryID, y.SKUNumber, y.SKUName, y.SKUPrice, y.SKUEnabled, y.SKUID }).ToList();
                var inboundDetails = CustomTableItemProvider.GetItems<InboundTrackingItem>().ToList();
                var allDetails = from product in productAndSKUDetails
                                 join inbound in inboundDetails
                                 on product.SKUID equals inbound.SKUID into alldata
                                 from newData in alldata.DefaultIfEmpty()
                                 select new
                                 {
                                     SKUID = product.SKUID,
                                     SKUNumber = product.SKUNumber,
                                     SKUName = product.SKUName,
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
                                     Status = product.SKUEnabled
                                 };
                return allDetails;
            }
            return null;
        }
        return null;
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
                    .Columns("CampaignID")
                    .WhereEquals("CampaignID", campaignID)
                    .ToList();
            }
            else
            {
                campaigns = CampaignProvider.GetCampaigns()
                             .Columns("CampaignID")
                             .Where(x => x.OpenCampaign == true && x.CloseCampaign == false)
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
            var camapaigns = CampaignProvider.GetCampaigns().Columns("CampaignID,Name").ToList();
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
            var inboundData = CustomTableItemProvider.GetItems<InboundTrackingItem>().WhereEquals("SKUID", ValidationHelper.GetInteger(skuid, default(int))).FirstOrDefault();
            if (inboundData != null)
            {
                inboundData.SKUID = ValidationHelper.GetInteger(skuid, default(int));
                inboundData.QtyOrdered = ValidationHelper.GetInteger(((Label)gdvInboundProducts.Rows[e.RowIndex].FindControl("lblQtyOrdered")).Text, default(int));
                inboundData.DemandGoal = ValidationHelper.GetInteger(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtDemandGoal")).Text, default(int));
                inboundData.QtyReceived = ValidationHelper.GetInteger(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtQtyReceived")).Text, default(int));
                inboundData.QtyProduced = ValidationHelper.GetInteger(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtQtyProduced")).Text, default(int));
                inboundData.Overage = inboundData.QtyOrdered - inboundData.QtyProduced;
                inboundData.Vendor = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtVendor")).Text, string.Empty);
                inboundData.ExpArrivalToCenveo = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtExpArrivalToCenveo")).Text, string.Empty);
                inboundData.DeliveryToDistBy = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtDeliveryToDistBy")).Text, string.Empty);
                inboundData.ShippedToDist = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtShippedToDist")).Text, string.Empty);
                inboundData.CenveoComments = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtCenveoComments")).Text, string.Empty);
                inboundData.TweComments = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtTweComments")).Text, string.Empty);
                inboundData.ActualPrice = ValidationHelper.GetDouble(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtActualPrice")).Text, default(double));
                inboundData.Update();
                UpdateSkuTable(ValidationHelper.GetInteger(skuid, default(int)), ValidationHelper.GetDouble(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtActualPrice")).Text, default(double)), ValidationHelper.GetInteger(((DropDownList)gdvInboundProducts.Rows[e.RowIndex].FindControl("ddlStatus")).SelectedValue, default(int)));
                gdvInboundProducts.EditIndex = -1;
                GetProducts();
            }
            else
            {
                InboundTrackingItem item = new InboundTrackingItem()
                {
                    SKUID = ValidationHelper.GetInteger(skuid, default(int)),
                    QtyOrdered = ValidationHelper.GetInteger(((Label)gdvInboundProducts.Rows[e.RowIndex].FindControl("lblQtyOrdered")).Text, default(int)),
                    DemandGoal = ValidationHelper.GetInteger(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtDemandGoal")).Text, default(int)),
                    QtyReceived = ValidationHelper.GetInteger(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtQtyReceived")).Text, default(int)),
                    QtyProduced = ValidationHelper.GetInteger(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtQtyProduced")).Text, default(int)),
                    Overage = ValidationHelper.GetInteger(((Label)gdvInboundProducts.Rows[e.RowIndex].FindControl("lblQtyOrdered")).Text, default(int)) - ValidationHelper.GetInteger(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtQtyProduced")).Text, default(int)),
                    Vendor = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtVendor")).Text, string.Empty),
                    ExpArrivalToCenveo = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtExpArrivalToCenveo")).Text, string.Empty),
                    DeliveryToDistBy = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtDeliveryToDistBy")).Text, string.Empty),
                    ShippedToDist = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtShippedToDist")).Text, string.Empty),
                    CenveoComments = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtCenveoComments")).Text, string.Empty),
                    TweComments = ValidationHelper.GetString(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtTweComments")).Text, string.Empty),
                    ActualPrice = ValidationHelper.GetDouble(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtActualPrice")).Text, default(double))
                };
                item.Insert();
                UpdateSkuTable(ValidationHelper.GetInteger(skuid, default(int)), ValidationHelper.GetDouble(((TextBox)gdvInboundProducts.Rows[e.RowIndex].FindControl("txtActualPrice")).Text, default(double)), ValidationHelper.GetInteger(((DropDownList)gdvInboundProducts.Rows[e.RowIndex].FindControl("ddlStatus")).SelectedValue, default(int)));
                gdvInboundProducts.EditIndex = -1;
                GetProducts();
            }
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
        BindPrograms(ValidationHelper.GetInteger(ddlCampaign.SelectedValue, default(int)));
        GetProducts();
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
}

#endregion "Methods"