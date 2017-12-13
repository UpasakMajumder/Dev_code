using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Globalization;
using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.ImageUpload;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public partial class CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// POSNumber Resource string
    /// </summary>
    public string POSNumberText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.POSNumberText"), string.Empty);
        }
        set
        {
            SetValue("POSNumberText", value);
        }
    }

    /// <summary>
    /// Product Name Resource string
    /// </summary>
    public string ProductNameText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.ProductNameText"), string.Empty);
        }
        set
        {
            SetValue("ProductNameText", value);
        }
    }

    /// <summary>
    /// Long Description Resource string
    /// </summary>
    public string LongDescriptionText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.LongDescriptionText"), string.Empty);
        }
        set
        {
            SetValue("LongDescriptionText", value);
        }
    }

    /// <summary>
    /// Program name Resource string
    /// </summary>
    public string ProgramNameText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.ProgramName"), string.Empty);
        }
        set
        {
            SetValue("ProgramNameText", value);
        }
    }

    /// <summary>
    /// Allowed states resource string
    /// </summary>
    public string AllowedStatesText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.AllowedStatesText"), string.Empty);
        }
        set
        {
            SetValue("AllowedStatesText", value);
        }
    }

    /// <summary>
    /// Expiration date resource string
    /// </summary>
    public string ExpairationDateText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.ExpairationDateText"), string.Empty);
        }
        set
        {
            SetValue("ExpairationDateText", value);
        }
    }

    /// <summary>
    /// Estimated price resource string
    /// </summary>
    public string EstimatedPriceText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.EstimatedPriceText"), string.Empty);
        }
        set
        {
            SetValue("EstimatedPriceText", value);
        }
    }

    /// <summary>
    /// Actual price resource string
    /// </summary>
    public string ActualPriceText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.ActualPriceText"), string.Empty);
        }
        set
        {
            SetValue("ActualPriceText", value);
        }
    }

    /// <summary>
    /// Brand name resource string
    /// </summary>
    public string BrandNameText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.BrandNameText"), string.Empty);
        }
        set
        {
            SetValue("BrandNameText", value);
        }
    }

    /// <summary>
    /// Category type resource string
    /// </summary>
    public string CategoryTypeText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.CategoryTypeText"), string.Empty);
        }
        set
        {
            SetValue("CategoryTypeText", value);
        }
    }

    /// <summary>
    /// Qty per pack resource string
    /// </summary>
    public string QtyPerPackText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.QtyPerPackText"), string.Empty);
        }
        set
        {
            SetValue("QtyPerPackText", value);
        }
    }

    /// <summary>
    /// Status resource string
    /// </summary>
    public string StatusText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.StatusText"), string.Empty);
        }
        set
        {
            SetValue("StatusText", value);
        }
    }

    /// <summary>
    /// Item specs resource string
    /// </summary>
    public string ItemSpecsText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.ItemSpecsText"), string.Empty);
        }
        set
        {
            SetValue("ItemSpecsText", value);
        }
    }

    /// <summary>
    /// Image resource string
    /// </summary>
    public string ImageText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.ImageText"), string.Empty);
        }
        set
        {
            SetValue("ImageText", value);
        }
    }

    /// <summary>
    /// Pos Error resource string
    /// </summary>
    public string PosError
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.PosError"), string.Empty);
        }
        set
        {
            SetValue("PosError", value);
        }
    }

    /// <summary>
    /// Product name error resouce string
    /// </summary>
    public string ProductNameError
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.ProductNameError"), string.Empty);
        }
        set
        {
            SetValue("ProductNameError", value);
        }
    }

    /// <summary>
    /// Long description resource string
    /// </summary>
    public string LongDescriptionError
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.LongDescriptionError"), string.Empty);
        }
        set
        {
            SetValue("LongDescriptionError", value);
        }
    }

    /// <summary>
    /// Program name error resource string
    /// </summary>
    public string ProgramNameError
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.ProgramNameError"), string.Empty);
        }
        set
        {
            SetValue("ProgramNameError", value);
        }
    }

    /// <summary>
    /// Estimated error resource string
    /// </summary>
    public string EstimatedPriceError
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.EstimatedPriceError"), string.Empty);
        }
        set
        {
            SetValue("EstimatedPriceError", value);
        }
    }

    /// <summary>
    /// Category error resouce string
    /// </summary>
    public string CategoryError
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.CategoryError"), string.Empty);
        }
        set
        {
            SetValue("CategoryError", value);
        }
    }

    /// <summary>
    /// Qty per pack error resource string
    /// </summary>
    public string QtyPerPackError
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.QtyPerPackError"), string.Empty);
        }
        set
        {
            SetValue("QtyPerPackError", value);
        }
    }

    /// <summary>
    /// Brand Name error resource string
    /// </summary>
    public string BrandNameError
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.BrandNameError"), string.Empty);
        }
        set
        {
            SetValue("BrandNameError", value);
        }
    }
    /// <summary>
    /// Numeric Field only error resource string
    /// </summary>
    public string NumberOnlyError
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.NumbericFieldError"), string.Empty);
        }
        set
        {
            SetValue("NumberOnlyError", value);
        }
    }

    /// <summary>
    /// Save button resource string
    /// </summary>
    public string SaveButtonText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SaveButtonText"), string.Empty);
        }
        set
        {
            SetValue("SaveButtonText", value);
        }
    }

    /// <summary>
    /// Update button resource string
    /// </summary>
    public string UpdateButtonText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.UpdateButtonText"), string.Empty);
        }
        set
        {
            SetValue("UpdateButtonText", value);
        }
    }

    /// <summary>
    /// Cancel button resource string
    /// </summary>
    public string CancelButtonText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.CancelButtonText"), string.Empty);
        }
        set
        {
            SetValue("CancelButtonText", value);
        }
    }

    /// <summary>
    /// Get the CalenderIcon Path
    /// </summary>
    public string CalenderIconPath
    {
        get
        {
            return ValidationHelper.GetString(SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_DatePickerPath"), string.Empty);
        }
        set
        {
            SetValue("CalenderIconPath", value);
        }
    }

    #endregion "Properties"

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
    /// Bind resource strings to labels, buttons and error messages
    /// </summary>
    public void BindResorceStrings()
    {
        lblPosNumber.InnerText = POSNumberText;
        lblProductName.InnerText = ProductNameText;
        lblLongDescription.InnerText = LongDescriptionText;
        lblProgramName.InnerText = ProgramNameText;
        lblState.InnerText = AllowedStatesText;
        lblExpirationDate.InnerText = ExpairationDateText;
        lblEstimatedPrice.InnerText = EstimatedPriceText;
        lblActualPrice.InnerText = ActualPriceText;
        lblBrand.InnerText = BrandNameText;
        lblProductCategory.InnerText = CategoryTypeText;
        lblQtyPerPack.InnerText = QtyPerPackText;
        lblStatus.InnerText = StatusText;
        lblItemSpecs.InnerText = ItemSpecsText;
        lblImage.InnerText = ImageText;
        btnSave.Text = SaveButtonText;
        btnUpdate.Text = UpdateButtonText;
        btnCancel.Text = CancelButtonText;
        rqPOS.ErrorMessage = PosError;
        rqProductName.ErrorMessage = ProductNameError;
        rqLongDescription.ErrorMessage = LongDescriptionError;
        rqProgram.ErrorMessage = ProgramNameError;
        rqEstimatePrice.ErrorMessage = EstimatedPriceError;
        rqBrand.ErrorMessage = BrandNameError;
        revActualPrice.ErrorMessage = NumberOnlyError;
        revEstPrice.ErrorMessage = NumberOnlyError;
        revQty.ErrorMessage = NumberOnlyError;
        rqProductCategory.ErrorMessage = CategoryError;
        rqQty.ErrorMessage = QtyPerPackError;
        hdnDatepickerUrl.Value = CalenderIconPath;
        ddlStatus.Items.Insert(0, new ListItem(ResHelper.GetString("KDA.Common.Status.Active"), "1"));
        ddlStatus.Items.Insert(1, new ListItem(ResHelper.GetString("KDA.Common.Status.Inactive"), "0"));
    }

    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (!this.StopProcessing)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindPrograms();
                    BindCategories();
                    BindResorceStrings();
                    BindPOS();
                    GetStateGroup();
                    GetBrandName();
                    int productID = ValidationHelper.GetInteger(Request.QueryString["id"], 0);
                    if (productID != 0)
                    {
                        CampaignsProduct product = CampaignsProductProvider
                            .GetCampaignsProducts()
                            .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                            .WhereEquals("CampaignsProductID", productID)
                            .FirstOrDefault();
                        if (product != null)
                        {
                            SKUInfo skuDetails = SKUInfoProvider
                                .GetSKUs()
                                .WhereEquals("SKUID", product.NodeSKUID)
                                .FirstObject;
                            if (skuDetails != null)
                            {
                                string folderName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_ImagesFolderName");
                                folderName = !string.IsNullOrEmpty(folderName) ? folderName.Replace(" ", "") : "CampaignProducts";
                                txtLongDescription.Text = skuDetails.SKUDescription;
                                txtEstimatedprice.Text = ValidationHelper.GetString(skuDetails.SKUPrice, string.Empty);
                                ddlPos.SelectedValue = ValidationHelper.GetString(skuDetails.SKUNumber, string.Empty);
                                txtProductName.Text = skuDetails.SKUName;
                                txtActualPrice.Text = ValidationHelper.GetString(skuDetails.SKUPrice, string.Empty);
                                ddlStatus.SelectedValue = skuDetails.SKUEnabled == true ? "1" : "0";
                                imgProduct.ImageUrl = MediaFileURLProvider.GetMediaFileUrl(CurrentSiteName, folderName, ValidationHelper.GetString(skuDetails.SKUImagePath, string.Empty));
                                imgProduct.Visible = imgProduct.ImageUrl != string.Empty ? true : false;
                                txtExpireDate.Text = ValidationHelper.GetString(skuDetails.SKUValidUntil, string.Empty);
                            }
                            ddlProgram.SelectedValue = ValidationHelper.GetString(product.ProgramID, string.Empty);
                            ddlState.SelectedValue = ValidationHelper.GetString(product.State, string.Empty);
                            ddlBrand.SelectedValue = product.BrandID.ToString();
                            ddlProductcategory.SelectedValue = product.CategoryID.ToString();
                            txtQty.Text = ValidationHelper.GetString(product.QtyPerPack, string.Empty);
                            txtItemSpecs.Text = ValidationHelper.GetString(product.ItemSpecs, string.Empty);
                            ViewState["ProgramID"] = product.ProgramID;
                            ViewState["ProductNodeID"] = product.NodeID;
                            btnSave.Visible = false;
                            btnUpdate.Visible = true;
                        }
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
                EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "SetupControl", ex, CurrentSite.SiteID, ex.Message);
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
                var campaign = DocumentHelper.GetDocument(capaignNodeID, CurrentDocument.DocumentCulture, tree);
                if (!DataHelper.DataSourceIsEmpty(campaign))
                {
                    int campaignID = campaign.GetIntegerValue("CampaignID", default(int));
                    if (campaignID != default(int))
                    {
                        var programs = ProgramProvider.GetPrograms()
                            .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                            .WhereEquals("CampaignID", campaignID)
                            .Columns("ProgramName,ProgramID")
                            .Select(x => new Program { ProgramID = x.ProgramID, ProgramName = x.ProgramName })
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
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "BindPrograms", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Bind categories to dropdown
    /// </summary>
    public void BindCategories()
    {
        try
        {
            var categories = ProductCategoryProvider.GetProductCategories()
                .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                .Columns("ProductCategoryID,ProductCategoryTitle")
                .Select(x => new ProductCategory { ProductCategoryID = x.ProductCategoryID, ProductCategoryTitle = x.ProductCategoryTitle })
                .ToList();
            if (!DataHelper.DataSourceIsEmpty(categories))
            {
                ddlProductcategory.DataSource = categories;
                ddlProductcategory.DataTextField = "ProductCategoryTitle";
                ddlProductcategory.DataValueField = "ProductCategoryID";
                ddlProductcategory.DataBind();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectCategoryText"), string.Empty);
                ddlProductcategory.Items.Insert(0, new ListItem(selectText, "0"));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "BindCategories", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Bind POS Number
    /// </summary>
    public void BindPOS()
    {
        try
        {
            var pos = CustomTableItemProvider.GetItems(POSNumberItem.CLASS_NAME)
                .Columns("ItemID,POSNumber")
                .ToList();
            if (!DataHelper.DataSourceIsEmpty(pos))
            {
                ddlPos.DataSource = pos;
                ddlPos.DataTextField = "POSNumber";
                ddlPos.DataValueField = "POSNumber";
                ddlPos.DataBind();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectPOSText"), string.Empty);
                ddlPos.Items.Insert(0, new ListItem(selectText, "0"));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "BindPOS", ex, CurrentSite.SiteID, ex.Message);
        }
    }
    /// <summary>
    /// Get the brand list
    /// </summary>
    /// <param name="brandItemID"></param>
    /// <returns></returns>
    public string GetBrandName()
    {
        string returnValue = string.Empty;
        try
        {
            var brands = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME)
                .Columns("ItemID,BrandName")
                .ToList();
            if (!DataHelper.DataSourceIsEmpty(brands))
            {
                ddlBrand.DataSource = brands;
                ddlBrand.DataTextField = "BrandName";
                ddlBrand.DataValueField = "ItemID";
                ddlBrand.DataBind();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.InvProductForm.BrandWaterMark"), string.Empty);
                ddlBrand.Items.Insert(0, new ListItem(selectText, "0"));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "GetBrandName", ex, CurrentSite.SiteID, ex.Message);
        }
        return returnValue;
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
    /// Insert product data to database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string imagePath = string.Empty;
        try
        {
            int programID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int));
            if (programID != default(int))
            {
                Program program = new Program();
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                var programDoc = ProgramProvider.GetPrograms()
                    .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                    .Columns("NodeID")
                    .WhereEquals("ProgramID", programID)
                    .FirstOrDefault();
                if (!DataHelper.DataSourceIsEmpty(programDoc))
                {
                    int programNodeID = programDoc.NodeID;
                    var document = DocumentHelper.GetDocument(programNodeID, CurrentDocument.DocumentCulture, tree);
                    var createNode = tree.SelectSingleNode(SiteContext.CurrentSiteName, document.NodeAliasPath, CurrentDocument.DocumentCulture);
                    if (createNode != null)
                    {
                        CampaignsProduct products = new CampaignsProduct()
                        {
                            ProgramID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)),
                            EstimatedPrice = ValidationHelper.GetDouble(txtEstimatedprice.Text, default(double)),
                            BrandID = ValidationHelper.GetInteger(hfBrandItemID.Value, default(int)),
                            CategoryID = ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, default(int)),
                            QtyPerPack = ValidationHelper.GetInteger(txtQty.Text, default(int)),
                            ItemSpecs = ValidationHelper.GetInteger(txtItemSpecs.Text, default(int)),
                            State = ValidationHelper.GetInteger(ddlState.SelectedValue, default(int)),
                            ProductName = ValidationHelper.GetString(txtProductName.Text, string.Empty)
                        };
                        products.DocumentName = ValidationHelper.GetString(txtProductName.Text, string.Empty);
                        products.DocumentCulture = CurrentDocument.DocumentCulture;
                        if (productImage.HasFile)
                        {
                            string libraryFolderName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_ImagesFolderName");
                            imagePath = UploadImage.UploadImageToMeadiaLibrary(productImage, libraryFolderName);
                        }
                        SKUInfo newProduct = new SKUInfo()
                        {
                            SKUName = ValidationHelper.GetString(txtProductName.Text, string.Empty),
                            SKUNumber = ValidationHelper.GetString(ddlPos.SelectedValue, string.Empty),
                            SKUShortDescription = ValidationHelper.GetString(txtProductName.Text, string.Empty),
                            SKUDescription = ValidationHelper.GetString(txtLongDescription.Text, string.Empty),
                            SKUPrice = ValidationHelper.GetDouble(txtActualPrice.Text, default(double)),
                            SKUValidUntil = ValidationHelper.GetDate(txtExpireDate.Text, DateTime.Now.Date),
                            SKUEnabled = ValidationHelper.GetString(ddlStatus.SelectedValue, "1") == "1" ? true : false,
                            SKUImagePath = imagePath,
                            SKUSiteID = CurrentSite.SiteID,
                            SKUProductType = SKUProductTypeEnum.EProduct
                        };
                        SKUInfoProvider.SetSKUInfo(newProduct);
                        products.NodeSKUID = newProduct.SKUID;
                        products.Insert(createNode, true);
                        int capaignNodeID = ValidationHelper.GetInteger(Request.QueryString["camp"], default(int));
                        var campDoc = DocumentHelper.GetDocument(capaignNodeID, CurrentDocument.DocumentCulture, tree);
                        if (campDoc != null)
                        {
                            Response.Redirect(campDoc.DocumentUrlPath);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "btnSave_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Update the product data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int programID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, 0);
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (ViewState["ProductNodeID"] != null)
            {
                CampaignsProduct product = CampaignsProductProvider.GetCampaignsProduct(ValidationHelper.GetInteger(ViewState["ProductNodeID"], 0), CurrentDocument.DocumentCulture, CurrentSiteName);
                if (product != null)
                {
                    product.DocumentName = ValidationHelper.GetString(txtProductName.Text, string.Empty);
                    product.ProgramID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, 0);
                    product.State = ValidationHelper.GetInteger(ddlState.SelectedValue, default(int));
                    product.BrandID = ValidationHelper.GetInteger(hfBrandItemID.Value, default(int));
                    product.CategoryID = ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, 0);
                    product.QtyPerPack = ValidationHelper.GetInteger(txtQty.Text, default(int));
                    product.ItemSpecs = ValidationHelper.GetInteger(txtItemSpecs.Text, default(int));
                    product.ProductName = ValidationHelper.GetString(txtProductName.Text, string.Empty);
                    SKUInfo updateProduct = SKUInfoProvider.GetSKUs().WhereEquals("SKUID", product.NodeSKUID).FirstObject;
                    if (updateProduct != null)
                    {
                        if (productImage.HasFile)
                        {
                            string libraryFolderName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_ImagesFolderName");
                            if (updateProduct.SKUImagePath != string.Empty)
                            {
                                UploadImage.DeleteImage(updateProduct.SKUImagePath, libraryFolderName);
                            }
                            updateProduct.SKUImagePath = UploadImage.UploadImageToMeadiaLibrary(productImage, libraryFolderName);
                        }
                        updateProduct.SKUName = ValidationHelper.GetString(txtProductName.Text, string.Empty);
                        updateProduct.SKUNumber = ValidationHelper.GetString(ddlPos.SelectedValue, string.Empty);
                        updateProduct.SKUShortDescription = ValidationHelper.GetString(txtProductName.Text, string.Empty);
                        updateProduct.SKUDescription = ValidationHelper.GetString(txtLongDescription.Text, string.Empty);
                        updateProduct.SKUPrice = ValidationHelper.GetDouble(txtActualPrice.Text, default(double));
                        updateProduct.SKUValidUntil = ValidationHelper.GetDate(txtExpireDate.Text, DateTime.Now.Date);
                        updateProduct.SKUEnabled = ValidationHelper.GetString(ddlStatus.SelectedValue, "1") == "1" ? true : false;
                        updateProduct.SKUSiteID = CurrentSite.SiteID;
                        updateProduct.SKUProductType = SKUProductTypeEnum.EProduct;
                        SKUInfoProvider.SetSKUInfo(updateProduct);
                    }
                    product.Update();
                }

                if (ViewState["ProgramID"] != null)
                {
                    if (ValidationHelper.GetInteger(ViewState["ProgramID"], 0) != programID)
                    {
                        var targetProgram = ProgramProvider.GetPrograms()
                            .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                            .WhereEquals("ProgramID", programID)
                            .Column("NodeID")
                            .FirstOrDefault();
                        if (targetProgram != null)
                        {
                            var tagetDocument = DocumentHelper.GetDocument(targetProgram.NodeID, CurrentDocument.DocumentCulture, tree);
                            var targetPage = tree.SelectSingleNode(SiteContext.CurrentSiteName, tagetDocument.NodeAliasPath, CurrentDocument.DocumentCulture);
                            if ((product != null) && (targetPage != null))
                            {
                                DocumentHelper.MoveDocument(product, targetPage, tree, true);
                            }
                        }
                    }
                }
                int capaignNodeID = ValidationHelper.GetInteger(Request.QueryString["camp"], default(int));
                var campDoc = DocumentHelper.GetDocument(capaignNodeID, CurrentDocument.DocumentCulture, tree);
                if (campDoc != null)
                {
                    Response.Redirect(campDoc.DocumentUrlPath);
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "btnUpdate_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Cancel current action
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            int capaignNodeID = ValidationHelper.GetInteger(Request.QueryString["camp"], default(int));
            var campDoc = DocumentHelper.GetDocument(capaignNodeID, CurrentDocument.DocumentCulture, tree);
            if (campDoc != null)
            {
                Response.Redirect(campDoc.DocumentUrlPath);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "btnCancel_Click", ex, CurrentSite.SiteID, ex.Message);
        }
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
                var program = ProgramProvider.GetPrograms()
                    .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                    .WhereEquals("ProgramID", programID)
                    .FirstOrDefault();
                if (program != null)
                {
                    int brandItemID = program.BrandID;
                    if (brandItemID != 0)
                    {
                        ddlBrand.SelectedValue = brandItemID.ToString();
                        hfBrandItemID.Value = brandItemID.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "ddlProgram_SelectedIndexChanged ", ex, CurrentSite.SiteID, ex.Message);
        }
    }
    /// <summary>
    /// Get all The State Groups
    /// </summary>
    /// <returns></returns>
    public string GetStateGroup()
    {
        string returnValue = string.Empty;
        try
        {
            var states = CustomTableItemProvider.GetItems(StatesGroupItem.CLASS_NAME)
                .Columns("ItemID,States")
                .ToList();
            if (!DataHelper.DataSourceIsEmpty(states))
            {
                ddlState.DataSource = states;
                ddlState.DataTextField = "States";
                ddlState.DataValueField = "ItemID";
                ddlState.DataBind();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.StateStateText"), string.Empty);
                ddlState.Items.Insert(0, new ListItem(selectText, "0"));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "GetStateGroup", ex, CurrentSite.SiteID, ex.Message);
        }
        return returnValue;
    }

    #endregion "Methods"
}