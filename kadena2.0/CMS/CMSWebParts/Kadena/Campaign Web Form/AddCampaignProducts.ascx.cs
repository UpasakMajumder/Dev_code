using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.EventLog;
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
        rqProductCategory.ErrorMessage = CategoryError;
        rqQty.ErrorMessage = QtyPerPackError;
        hdnDatepickerUrl.Value = CalenderIconPath;
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
                    int productID = ValidationHelper.GetInteger(Request.QueryString["id"], 0);
                    if (productID != 0)
                    {
                        CampaignProduct product = CampaignProductProvider.GetCampaignProducts().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignProductID", productID).FirstOrDefault();
                        if (product != null)
                        {
                            ddlProgram.SelectedValue = product.ProgramID.ToString();
                            ddlPos.SelectedValue = product.POSNumber.ToString();
                            txtProductName.Text = product.ProductName;
                            txtState.Text = product.State;
                            txtLongDescription.Text = product.LongDescription;
                            txtExpireDate.Text = ValidationHelper.GetDate(txtExpireDate.Text, DateTime.Now.Date).ToString();
                            txtBrand.Text = GetBrandName(product.BrandID);
                            ddlProductcategory.SelectedValue = product.CategoryID.ToString();
                            txtQty.Text = product.QtyPerPack;
                            txtItemSpecs.Text = product.ItemSpecs;
                            txtActualPrice.Text = product.ActualPrice;
                            txtEstimatedprice.Text = product.EstimatedPrice;
                            ddlStatus.SelectedValue = product.Status == true ? "1" : "0";
                            if (product.Image != default(Guid))
                            {
                                string folderName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_ImagesFolderName");
                                MediaLibraryInfo library = MediaLibraryInfoProvider.GetMediaLibraryInfo(!string.IsNullOrEmpty(folderName) ? folderName : "CampaignProducts", SiteContext.CurrentSiteName);
                                if (!DataHelper.DataSourceIsEmpty(library))
                                {
                                    MediaFileInfo image = MediaFileInfoProvider.GetMediaFileInfo(product.Image, SiteContext.CurrentSiteName);
                                    if (!DataHelper.DataSourceIsEmpty(image))
                                    {
                                        imgProduct.ImageUrl = MediaFileURLProvider.GetMediaFileAbsoluteUrl(product.Image, image.FileName);
                                        imgProduct.Visible = true;
                                    }
                                    else
                                        imgProduct.Visible = false;
                                }
                                else
                                    imgProduct.Visible = false;
                            }
                            else
                                imgProduct.Visible = false;

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
                        var programs = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignID", campaignID).Columns("ProgramName,ProgramID").Select(x => new Program { ProgramID = x.ProgramID, ProgramName = x.ProgramName }).ToList();
                        if (!DataHelper.DataSourceIsEmpty(programs))
                        {
                            ddlProgram.DataSource = programs;
                            ddlProgram.DataTextField = "ProgramName";
                            ddlProgram.DataValueField = "ProgramID";
                            ddlProgram.DataBind();
                            string selectText = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_SelectProgramText");
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
            var categories = ProductCategoryProvider.GetProductCategories().WhereEquals("NodeSiteID", CurrentSite.SiteID).Columns("ProductCategoryID,ProductCategoryTitle").Select(x => new ProductCategory { ProductCategoryID = x.ProductCategoryID, ProductCategoryTitle = x.ProductCategoryTitle }).ToList();
            if (!DataHelper.DataSourceIsEmpty(categories))
            {
                ddlProductcategory.DataSource = categories;
                ddlProductcategory.DataTextField = "ProductCategoryTitle";
                ddlProductcategory.DataValueField = "ProductCategoryID";
                ddlProductcategory.DataBind();
                string selectText = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_SelectCategoryText");
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
            var pos = CustomTableItemProvider.GetItems(POSNumberItem.CLASS_NAME).Columns("ItemID,POSNumber").ToList();
            if (!DataHelper.DataSourceIsEmpty(pos))
            {
                ddlPos.DataSource = pos;
                ddlPos.DataTextField = "POSNumber";
                ddlPos.DataValueField = "ItemID";
                ddlPos.DataBind();
                string selectText = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_SelectPOSText");
                ddlPos.Items.Insert(0, new ListItem(selectText, "0"));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "BindPOS", ex, CurrentSite.SiteID, ex.Message);
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
                var programDoc = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID", CurrentSite.SiteID).Columns("NodeID").WhereEquals("ProgramID", programID).FirstOrDefault();
                if (!DataHelper.DataSourceIsEmpty(programDoc))
                {
                    int programNodeID = programDoc.NodeID;
                    var document = DocumentHelper.GetDocument(programNodeID, CurrentDocument.DocumentCulture, tree);
                    var createNode = tree.SelectSingleNode(SiteContext.CurrentSiteName, document.NodeAliasPath, CurrentDocument.DocumentCulture);
                    if (createNode != null)
                    {
                        CampaignProduct products = new CampaignProduct()
                        {
                            ProgramID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)),
                            ProductName = ValidationHelper.GetString(txtProductName.Text, string.Empty),
                            State = ValidationHelper.GetString(txtState.Text, string.Empty),
                            LongDescription = ValidationHelper.GetString(txtLongDescription.Text, string.Empty),
                            ExpirationDate = ValidationHelper.GetDate(txtExpireDate.Text, DateTime.Now.Date),
                            EstimatedPrice = ValidationHelper.GetString(txtEstimatedprice.Text, string.Empty),
                            ActualPrice = ValidationHelper.GetString(txtActualPrice.Text, string.Empty),
                            BrandID = ValidationHelper.GetInteger(hfBrandItemID.Value, default(int)),
                            CategoryID = ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, default(int)),
                            QtyPerPack = ValidationHelper.GetString(txtQty.Text, string.Empty),
                            POSNumber = ValidationHelper.GetInteger(ddlPos.SelectedValue, default(int)),
                            ItemSpecs = ValidationHelper.GetString(txtItemSpecs.Text, string.Empty),
                            Status = ValidationHelper.GetString(ddlStatus.SelectedValue, "1") == "1" ? true : false,
                        };
                        products.DocumentName = ValidationHelper.GetString(txtProductName.Text, string.Empty);
                        products.DocumentCulture = CurrentDocument.DocumentCulture;
                        if (productImage.HasFile)
                        {
                            string libraryFolderName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_ImagesFolderName");
                            imageGuid = UploadImage.UploadImageToMeadiaLibrary(productImage, libraryFolderName);
                            products.Image = imageGuid;
                        }
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
                    product.DocumentName = ValidationHelper.GetString(txtProductName.Text, string.Empty);
                    product.ProgramID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, 0);
                    product.ProductName = ValidationHelper.GetString(txtProductName.Text, string.Empty);
                    product.State = ValidationHelper.GetString(txtState.Text, string.Empty);
                    product.LongDescription = ValidationHelper.GetString(txtLongDescription.Text, string.Empty);
                    product.ExpirationDate = ValidationHelper.GetDate(txtExpireDate.Text, DateTime.Now.Date);
                    product.EstimatedPrice = ValidationHelper.GetString(txtEstimatedprice.Text, string.Empty);
                    product.ActualPrice = ValidationHelper.GetString(txtActualPrice.Text, string.Empty);
                    product.BrandID = ValidationHelper.GetInteger(hfBrandItemID.Value, default(int));
                    product.CategoryID = ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, 0);
                    product.QtyPerPack = ValidationHelper.GetString(txtQty.Text, string.Empty);
                    product.POSNumber = ValidationHelper.GetInteger(ddlPos.SelectedValue, 0);
                    product.ItemSpecs = ValidationHelper.GetString(txtItemSpecs.Text, string.Empty);
                    product.Status = ValidationHelper.GetString(ddlStatus.SelectedValue, "1") == "1" ? true : false;
                    if (productImage.HasFile)
                    {
                        string libraryFolderName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_ImagesFolderName");
                        if (product.Image != default(Guid))
                        {
                            UploadImage.DeleteImage(product.Image, libraryFolderName);
                        }
                        imageGuid = UploadImage.UploadImageToMeadiaLibrary(productImage, libraryFolderName);
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
                var program = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("ProgramID", programID).FirstOrDefault();
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
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "ddlProgram_SelectedIndexChanged ", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Get the brand name by brandItemId
    /// </summary>
    /// <param name="brandItemID"></param>
    /// <returns></returns>
    public string GetBrandName(int brandItemID)
    {
        string returnValue = string.Empty;
        try
        {
            var brand = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME).WhereEquals("ItemID", brandItemID).Column("BrandName").Select(x => new BrandItem { BrandName = x.Field<string>("BrandName") }).FirstOrDefault();
            returnValue = brand.BrandName;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "GetBrandName", ex, CurrentSite.SiteID, ex.Message);
        }
        return returnValue;
    }

    #endregion "Methods"
}