using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.IO;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DocumentEngine.Types.KDA;
using System.Collections.Generic;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.EventLog;
using CMS.MediaLibrary;
using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using Kadena.Old_App_Code.Kadena.ImageUpload;

public partial class CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts : CMSAbstractWebPart
{
    #region "Properties"



    #endregion


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
        if (this.StopProcessing)
        {
            // Do not process
        }
        else
        {
            try
            {
                BindPrograms();
                BindCategories();
                int productID = ValidationHelper.GetInteger(Request.QueryString["id"], 0);
                if (productID != 0)
                {
                    CampaignProduct product = CampaignProductProvider.GetCampaignProducts().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignProductID", productID).TopN(1).FirstOrDefault();
                    if (product != null)
                    {
                        ddlProgram.SelectedValue = product.ProgramID.ToString();
                        ddlPos.SelectedValue = product.POSNumber.ToString();
                        txtProductName.Text = product.ProductName;
                        txtState.Text = product.State;
                        txtLongDescription.Text = product.LongDescription;
                        txtExpireDate.Text = product.ExpirationDate.ToString();
                        txtBrand.Text = GetBrandName(product.BrandID);
                        ddlProductcategory.SelectedValue = product.CategoryID.ToString();
                        txtQty.Text = product.QtyPerPack;
                        txtItemSpecs.Text = product.ItemSpecs;
                        txtActualPrice.Text = product.ActualPrice;
                        txtEstimatedprice.Text = product.EstimatedPrice;
                        if (product.Image != default(Guid) && !product.Image.Equals(Guid.Empty))
                        {
                            MediaFileInfo image = MediaFileInfoProvider.GetMediaFileInfo(product.Image, SiteContext.CurrentSiteName);
                            if (image != null)
                            {
                                imgProduct.ImageUrl = MediaFileURLProvider.GetMediaFileAbsoluteUrl(product.Image, image.FileName);
                                imgProduct.Visible = true;
                            }
                        }

                        ViewState["ProgramID"] = product.ProgramID;
                        ViewState["ProductNodeID"] = product.NodeID;
                        btnSave.Visible = false;
                        btnUpdate.Visible = true;
                    }
                    else
                    {
                        btnSave.Visible = true;
                        btnUpdate.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "SetupControl", ex.Message);
            }
        }
    }
    /// <summary>
    /// Bind programs to dropdown
    /// </summary>
    public void BindPrograms()
    {
        try
        {
            int capaignNodeID = ValidationHelper.GetInteger(Request.QueryString["camp"], default(int));
            if (capaignNodeID != default(int))
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                var campaign = DocumentHelper.GetDocument(capaignNodeID, CurrentSite.DefaultVisitorCulture, tree);
                if (campaign != null)
                {
                    int campaignID = campaign.GetIntegerValue("CampaignID", default(int));
                    if (campaignID != default(int))
                    {
                        var programs = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignID", campaignID).Columns("ProgramName,ProgramID").Select(x => new Program { ProgramID = x.ProgramID, ProgramName = x.ProgramName }).ToList();
                        if (programs != null)
                        {
                            ddlProgram.DataSource = programs;
                            ddlProgram.DataTextField = "ProgramName";
                            ddlProgram.DataValueField = "ProgramID";
                            ddlProgram.DataBind();
                            ddlProgram.Items.Insert(0, "--Select Program");

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "BindPrograms", ex.Message);
        }
    }
    /// <summary>
    /// Bind categories to dropdown
    /// </summary>
    public void BindCategories()
    {
        try
        {
            var categories = ProductCategoryProvider.GetProductCategories().WhereEquals("NodeSiteID", CurrentSite.SiteID).Columns("ProductCategoryID,ProductCategoryTitle").Select(x => new ProductCategory { ProductCategoryID = x.ProductCategoryID, ProductCategoryTitle = x.ProductCategoryTitle }).ToList();
            if (categories != null)
            {
                ddlProductcategory.DataSource = categories;
                ddlProductcategory.DataTextField = "ProductCategoryTitle";
                ddlProductcategory.DataValueField = "ProductCategoryID";
                ddlProductcategory.DataBind();
                ddlProductcategory.Items.Insert(0, "--Select Category");

            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "BindCategories", ex.Message);
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
    #endregion
    /// <summary>
    /// Insert product data to database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Guid imageGuid = default(Guid);
        try
        {
            int programID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int));
            if (programID != default(int))
            {
                Program program = new Program();
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                var programDoc = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID", CurrentSite.SiteID).Columns("NodeID").WhereEquals("ProgramID", programID).TopN(1).FirstOrDefault();
                if (programDoc != null)
                {
                    int programNodeID = programDoc.NodeID;
                    var document = DocumentHelper.GetDocument(programNodeID, CurrentSite.DefaultVisitorCulture, tree);
                    CMS.DocumentEngine.TreeNode createNode = tree.SelectSingleNode(SiteContext.CurrentSiteName, document.NodeAliasPath, CurrentSite.DefaultVisitorCulture);
                    if (createNode != null)
                    {
                        CampaignProduct products = new CampaignProduct();
                        products.DocumentName = ValidationHelper.GetString(txtProductName.Text, "");
                        products.DocumentCulture = SiteContext.CurrentSite.DefaultVisitorCulture;
                        products.ProgramID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, 0);
                        products.ProductName = ValidationHelper.GetString(txtProductName.Text, "");
                        products.State = ValidationHelper.GetString(txtState.Text, "");
                        products.LongDescription = ValidationHelper.GetString(txtLongDescription.Text, "");
                        products.ExpirationDate = DateTime.Now.Date;
                        products.EstimatedPrice = ValidationHelper.GetString(txtEstimatedprice.Text, "");
                        products.ActualPrice = ValidationHelper.GetString(txtActualPrice.Text, "");
                        products.BrandID = ValidationHelper.GetInteger(hfBrandItemID.Value, default(int));
                        products.CategoryID = ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, 0);
                        products.QtyPerPack = ValidationHelper.GetString(txtQty.Text, "");
                        products.POSNumber = ValidationHelper.GetInteger(ddlPos.SelectedValue, 0);
                        products.ItemSpecs = ValidationHelper.GetString(txtItemSpecs.Text, "");
                        if (productImage.HasFile)
                        {
                            imageGuid = UploadImage.UploadImageToMeadiaLibrary(productImage);
                            products.Image = imageGuid;
                        }
                        products.Insert(createNode, true);
                        //Response.Redirect("");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (imageGuid != default(Guid))
                UploadImage.DeleteImage(imageGuid);
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "btnSave_Click", ex.Message);
        }
    }
    /// <summary>
    /// Update the product data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Guid imageGuid = default(Guid);
        try
        {
            int programID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, 0);
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (ViewState["ProductNodeID"] != null)
            {
                CampaignProduct product = CampaignProductProvider.GetCampaignProduct(ValidationHelper.GetInteger(ViewState["ProductNodeID"], 0), CurrentDocument.DocumentCulture, CurrentSiteName);
                if (product != null)
                {
                    product.DocumentName = ValidationHelper.GetString(txtProductName.Text, "");
                    product.ProgramID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, 0);
                    product.ProductName = ValidationHelper.GetString(txtProductName.Text, "");
                    product.State = ValidationHelper.GetString(txtState.Text, "");
                    product.LongDescription = ValidationHelper.GetString(txtLongDescription.Text, "");
                    product.ExpirationDate = DateTime.Now.Date;
                    product.EstimatedPrice = ValidationHelper.GetString(txtEstimatedprice.Text, "");
                    product.ActualPrice = ValidationHelper.GetString(txtActualPrice.Text, "");
                    product.BrandID = ValidationHelper.GetInteger(hfBrandItemID.Value, default(int));
                    product.CategoryID = ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, 0);
                    product.QtyPerPack = ValidationHelper.GetString(txtQty.Text, "");
                    product.POSNumber = ValidationHelper.GetInteger(ddlPos.SelectedValue, 0);
                    product.ItemSpecs = ValidationHelper.GetString(txtItemSpecs.Text, "");
                    if (productImage.HasFile)
                    {
                        if (product.Image != default(Guid))
                            UploadImage.DeleteImage(product.Image);
                        imageGuid = UploadImage.UploadImageToMeadiaLibrary(productImage);
                        product.Image = imageGuid;
                    }
                    product.Update();
                }

                if (ViewState["ProgramID"] != null)
                {
                    if (ValidationHelper.GetInteger(ViewState["ProgramID"], 0) != programID)
                    {
                        var targetProgram = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("ProgramID", programID).Column("NodeID").FirstOrDefault();
                        if (targetProgram != null)
                        {
                            var tagetDocument = DocumentHelper.GetDocument(targetProgram.NodeID, CurrentSite.DefaultVisitorCulture, tree);
                            CMS.DocumentEngine.TreeNode targetPage = tree.SelectSingleNode(SiteContext.CurrentSiteName, tagetDocument.NodeAliasPath, CurrentSite.DefaultVisitorCulture);
                            if ((product != null) && (targetPage != null))
                            {
                                DocumentHelper.MoveDocument(product, targetPage, tree, true);
                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (imageGuid != default(Guid))
                UploadImage.DeleteImage(imageGuid);
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "btnUpdate_Click", ex.Message);
        }

    }
    /// <summary>
    /// Cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// Bind Brand name to textbox using program id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int programID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, 0);
            if (programID != 0)
            {
                var program = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("ProgramID", programID).TopN(1).FirstOrDefault();
                if (program != null)
                {
                    int brandItemID = program.BrandID;
                    if (brandItemID != 0)
                    {
                        string brandName = GetBrandName(brandItemID);
                        txtBrand.Text = brandName;
                        hfBrandItemID.Value = brandItemID.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "ddlProgram_SelectedIndexChanged", ex.Message);
        }
    }
    /// <summary>
    /// Get the brand name by brandItemId
    /// </summary>
    /// <param name="brandItemID"></param>
    /// <returns></returns>
    public string GetBrandName(int brandItemID)
    {
        string returnValue = "";
        try
        {
            var brand = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME).WhereEquals("ItemID", brandItemID).Column("BrandName").TopN(1).Select(x => new BrandItem { BrandName = x.Field<string>("BrandName") }).FirstOrDefault();
            returnValue = brand.BrandName;

        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "GetBrandName", ex.Message);
        }
        return returnValue;
    }
}



