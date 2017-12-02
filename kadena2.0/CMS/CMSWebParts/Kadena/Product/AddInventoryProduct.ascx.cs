﻿using CMS.CustomTables;
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
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.ImageUpload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class AddInventoryProduct : CMSAbstractWebPart
    {
        #region variables

        private int productId = 0;
        private string folderpath = "/";
        private int PageSize = 10;
        private static List<AllocateProduct> lstUsers = new List<AllocateProduct>();
        private string libraryFolderName = string.Empty;
        private string redirectUrl = string.Empty;

        #endregion variables

        #region WebpartSetupMethods

        public override void OnContentLoaded()
        {
            base.OnContentLoaded();
            SetupControl();
        }

        public override void ReloadData()
        {
            base.ReloadData();
            SetupControl();
        }

        protected void SetupControl()
        {
            if (!this.StopProcessing)
            {

                if (Request.QueryString["ID"] != null)
                {
                    btnSave.Click += btnUpdate_Click;
                    productId = ValidationHelper.GetInteger(Request.QueryString["ID"], 0);
                }
                else
                {
                    btnSave.Click += btnSave_SavePOS;
                }
                if (!IsPostBack)
                {
                    if (Request.QueryString["ID"] != null)
                    {
                        SetFeild(productId);
                        if (Request.UrlReferrer != null)
                        {
                            ViewState["LastPageUrl"] = Request.UrlReferrer.ToString();
                        }
                    }
                    BindUsers(1);
                    BindData();
                }

                btnAllocateProduct.Click += AllocateProduct_Click;
                btnCancel.Click += BtnCancel_Cancel;
                hdnDatepickerUrl.Value = SettingsKeyInfoProvider.GetValue("KDA_DatePickerPath", CurrentSiteName);
                rfvBrand.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.BrandRequired");
                rfvActualPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ActualPriceRequired");
                rfvPosNo.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.POSCodeRequired");
                rfvProdCategory.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.POSCategroyRequired");
                rfvExpDate.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ExpiryDateRequired");
                rfvEstPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.EstimatedPriceRequired");
                rfvLongDes.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.LongDescritpionRequired");
                rfvShortDes.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ShortDescriptionRequired");
                rfvState.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.StateRequired");
                rfvBundleQnt.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.BundleQntRequired");
                folderpath = SettingsKeyInfoProvider.GetValue("KDA_InventoryProductFolderPath", CurrentSiteName);
                libraryFolderName = SettingsKeyInfoProvider.GetValue("KDA_InventoryProductImageFolderName", CurrentSiteName);
            }
        }

        #endregion WebpartSetupMethods

        #region ButtonclickEvents

        /// <summary>
        /// Save the Product to Product pagetype
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_SavePOS(object sender, EventArgs e)
        {
            try
            {
                if (ddlBrand.SelectedIndex > 0 && ddlPosNo.SelectedIndex > 0 && ddlProdCategory.SelectedIndex > 0 && ddlState.SelectedIndex > 0)
                {
                    if (ViewState["ProductId"] !=null)
                    {
                        UpdateProduct(ValidationHelper.GetInteger(ViewState["ProductId"],0));
                    }
                    else
                    {
                        SaveProduct();
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CMSWebParts_Kadena_Add_inventory_Products", "btnSave_Click", ex.Message);
            }
        }

        /// <summary>
        /// on Cancel button Click It will redirect to the last page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCancel_Cancel(object sender, EventArgs e)
        {
            try
            {
                EmptyFields(true);
                lstUsers = new List<AllocateProduct>();
                var redirectUrl = ValidationHelper.GetString(ViewState["LastPageUrl"], string.Empty);
                Response.Redirect(redirectUrl, false);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CancePOSFormButtonClick", "EXCEPTION", ex);
            }
        }

        /// <summary>
        /// Handles the Click event of the Allocate product button click .
        /// </summary>
        protected void AllocateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem ri in RepterDetails.Items)
                {
                    CheckBox item_check = (CheckBox)ri.FindControl("chkAllocate");
                    if (item_check.Checked)
                    {
                        int index = lstUsers.FindIndex(item => item.UserID == ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0));
                        if (index == -1)
                        {
                            AllocateProduct objAllocateProduct = new AllocateProduct();
                            objAllocateProduct.UserID = ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0);
                            objAllocateProduct.UserName = ValidationHelper.GetString(((Label)ri.FindControl("lblUserName")).Text, string.Empty);
                            objAllocateProduct.EmailID = ValidationHelper.GetString(((Label)ri.FindControl("lblEmail")).Text, string.Empty);
                            objAllocateProduct.Quantity = ValidationHelper.GetInteger(((TextBox)ri.FindControl("txtAllQuantity")).Text, 0);
                            lstUsers.Add(objAllocateProduct);
                        }
                    }
                    else
                    {
                        int index = lstUsers.FindIndex(item => item.UserID == ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0));
                        if (index != -1)
                        {
                            lstUsers.RemoveAt(index);
                        }
                    }
                }
                RepSelectedUser.DataSource = lstUsers;
                RepSelectedUser.DataBind();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CMSWebParts_Kadena_Add_inventory_Products_AllocateProduct", "AllocateProduct_Click", ex.Message);
            }
        }

        /// <summary>
        /// This method will get the users record when user click on pagination
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Changed(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem ri in RepterDetails.Items)
                {
                    CheckBox item_check = (CheckBox)ri.FindControl("chkAllocate");
                    if (item_check.Checked)
                    {
                        int index = lstUsers.FindIndex(item => item.UserID == ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0));
                        if (index == -1)
                        {
                            AllocateProduct objAllocateProduct = new AllocateProduct();
                            objAllocateProduct.UserID = ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0);
                            objAllocateProduct.UserName = ValidationHelper.GetString(((Label)ri.FindControl("lblUserName")).Text, string.Empty);
                            objAllocateProduct.EmailID = ValidationHelper.GetString(((Label)ri.FindControl("lblEmail")).Text, string.Empty);
                            objAllocateProduct.Quantity = ValidationHelper.GetInteger(((TextBox)ri.FindControl("txtAllQuantity")).Text, 0);
                            lstUsers.Add(objAllocateProduct);
                        }
                    }
                    else
                    {
                        int index = lstUsers.FindIndex(item => item.UserID == ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0));
                        if (index != -1)
                        {
                            lstUsers.RemoveAt(index);
                        }
                    }
                }
                int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
                this.BindUsers(pageIndex);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CMSWebParts_Kadena_Add_inventory_Products_Pagination", "Page_Changed", ex.Message);
            }
        }

        /// <summary>
        /// Method to update the product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string imagePath = string.Empty;
            try
            {
                if (ddlBrand.SelectedIndex > 0 && ddlPosNo.SelectedIndex > 0 && ddlProdCategory.SelectedIndex > 0 && ddlState.SelectedIndex > 0)
                {
                    UpdateProduct(productId);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CMSWebParts_Kadena_Add_inventory_Products", "btnSave_Click", ex.Message);
            }
        }

        #endregion ButtonclickEvents

        #region PrivateMethods

        /// <summary>
        /// This method will save the allocated product w.r.t to User in custome tabel
        /// </summary>
        /// <param name="productID">The id of the product which user wants to Allocate</param>
        private void AllocateProductToUsers(int productID)
        {
            string customTableClassName = "KDA.UserAllocatedProducts";
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if (customTable != null)
            {
                CustomTableItem newCustomTableItem = CustomTableItem.New(customTableClassName);
                foreach (AllocateProduct User in lstUsers)
                {
                    newCustomTableItem.SetValue("UserID", User.UserID);
                    newCustomTableItem.SetValue("ProductID", productID);
                    newCustomTableItem.SetValue("Quantity", User.Quantity);
                    newCustomTableItem.SetValue("EmailID", User.EmailID);
                    newCustomTableItem.Insert();
                }
            }
        }

        /// <summary>
        /// Update the Product allocation
        /// </summary>
        /// <param name="productID">The id of the product which user wants to Update</param>
        private void UpdateAllocateProduct(int productID)
        {
            try
            {
                string customTableClassName = "KDA.UserAllocatedProducts";
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    var customTableData = CustomTableItemProvider.GetItems(customTableClassName)
                        .WhereStartsWith("ProductID", productID.ToString());
                    foreach (CustomTableItem customitem in customTableData)
                    {
                        int index = lstUsers.FindIndex(item => item.UserID == ValidationHelper.GetInteger(customitem.GetValue("UserID"), 0));
                        if (index > -1)
                        {
                            customitem.SetValue("Quantity", lstUsers[index].Quantity);
                            customitem.Update();
                            lstUsers.RemoveAt(index);
                        }
                        else
                        {
                            customitem.Delete();
                            lstUsers.RemoveAt(index);
                        }
                    }
                }
                CustomTableItem newCustomTableItem = CustomTableItem.New(customTableClassName);
                foreach (AllocateProduct User in lstUsers)
                {
                    newCustomTableItem.SetValue("UserID", User.UserID);
                    newCustomTableItem.SetValue("ProductID", productID);
                    newCustomTableItem.SetValue("Quantity", User.Quantity);
                    newCustomTableItem.SetValue("EmailID", User.EmailID);
                    newCustomTableItem.Insert();
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Product allocation update", "EXCEPTION", ex);
            }
        }
        /// <summary>
        /// For saving the product
        /// </summary>
        private void SaveProduct()
        {
            string imagePath = string.Empty;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            CMS.DocumentEngine.TreeNode parentPage = tree.SelectNodes()
                .Path(folderpath)
                .OnCurrentSite()
                .Culture(DocumentContext.CurrentDocument.DocumentCulture)
                .FirstObject;
            if (parentPage != null)
            {
                CampaignsProduct products = new CampaignsProduct()
                {
                    BundleQty = ValidationHelper.GetInteger(txtBundleQnt.Text, default(int)),
                    BrandID = ValidationHelper.GetInteger(ddlBrand.SelectedValue, default(int)),
                    EstimatedPrice = ValidationHelper.GetDouble(txtEstPrice.Text, default(double)),
                    State = ValidationHelper.GetInteger(ddlState.SelectedValue, default(int)),
                    CategoryID = ValidationHelper.GetInteger(ddlProdCategory.SelectedValue, default(int)),
                    Cancelled = ValidationHelper.GetBoolean(chkcancel.Checked, false),
                    CVOProductID = ValidationHelper.GetInteger(txtCVOProductId.Text, default(int)),
                    StoreFrontProductID = ValidationHelper.GetInteger(txtStroeFrontId.Text, default(int)),
                    ProductName = ValidationHelper.GetString(txtShortDes.Text, string.Empty)
                };
                if (productImage.HasFile)
                {
                    imagePath = UploadImage.UploadImageToMeadiaLibrary(productImage, libraryFolderName);
                }
                SKUInfo newSkuProduct = new SKUInfo()
                {
                    SKUName = ValidationHelper.GetString(txtShortDes.Text, string.Empty),
                    SKUNumber = ValidationHelper.GetString(ddlPosNo.SelectedValue, string.Empty),
                    SKUDescription = ValidationHelper.GetString(txtLongDes.Text, string.Empty),
                    SKUPrice = ValidationHelper.GetDouble(txtEstPrice.Text, default(double)),
                    SKUEnabled = ValidationHelper.GetBoolean(ddlStatus.SelectedValue, false),
                    SKUAvailableItems = ValidationHelper.GetInteger(txtQuantity.Text, 0),
                    SKUImagePath = ValidationHelper.GetString(imagePath, string.Empty),
                    SKUSiteID = CurrentSite.SiteID,
                    SKUProductType = SKUProductTypeEnum.EProduct,
                };
                products.DocumentName = ValidationHelper.GetString(txtShortDes.Text, string.Empty);
                products.DocumentCulture = CurrentDocument.DocumentCulture;
                SKUInfoProvider.SetSKUInfo(newSkuProduct);
                products.NodeSKUID = newSkuProduct.SKUID;
                PageTemplateInfo template = PageTemplateInfoProvider.GetPageTemplateInfo("_Campaign Products");
                if (template != null)
                {
                    products.DocumentPageTemplateID = template.PageTemplateId;
                }
                products.Insert(parentPage, true);
                var productID = ValidationHelper.GetInteger(products.CampaignsProductID, 0);
                AllocateProductToUsers(productID);
                lblSuccessMsg.Visible = true;
                lblFailureText.Visible = false;
                EmptyFields(true);
            }
            else
            {
                lblFailureText.Visible = true;
            }
        }
        private void UpdateProduct(int productID)
        {
            // CampaignsProduct product = CampaignsProductProvider.GetCampaignsProduct(ValidationHelper.GetInteger(productID, 0), CurrentDocument.DocumentCulture, CurrentSiteName);
            CampaignsProduct product = CampaignsProductProvider
                           .GetCampaignsProducts()
                           .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                           .WhereEquals("CampaignsProductID", productID)
                           .FirstOrDefault();
            if (product != null)
            {
                product.DocumentName = ValidationHelper.GetString(txtShortDes.Text, string.Empty);
                product.BundleQty = ValidationHelper.GetInteger(txtBundleQnt.Text, default(int));
                product.BrandID = ValidationHelper.GetInteger(ddlBrand.SelectedValue, default(int));
                product.CategoryID = ValidationHelper.GetInteger(ddlProdCategory.SelectedValue, default(int));
                product.Cancelled = ValidationHelper.GetBoolean(chkcancel.Checked, false);
                product.CVOProductID = ValidationHelper.GetInteger(txtCVOProductId.Text, default(int));
                product.StoreFrontProductID = ValidationHelper.GetInteger(txtStroeFrontId.Text, default(int));
                product.EstimatedPrice = ValidationHelper.GetInteger(txtEstPrice.Text, default(int));
                product.ProductName = ValidationHelper.GetString(txtShortDes.Text, string.Empty);
                SKUInfo updateProduct = SKUInfoProvider.GetSKUs().WhereEquals("SKUID", product.SKUID).FirstObject;
                if (updateProduct != null)
                {
                    if (productImage.HasFile)
                    {
                        if (updateProduct.SKUImagePath != string.Empty)
                        {
                            UploadImage.DeleteImage(updateProduct.SKUImagePath, libraryFolderName);
                        }
                        updateProduct.SKUImagePath = UploadImage.UploadImageToMeadiaLibrary(productImage, libraryFolderName);
                    }
                    updateProduct.SKUName = ValidationHelper.GetString(txtShortDes.Text, string.Empty);
                    updateProduct.SKUNumber = ValidationHelper.GetString(ddlPosNo.SelectedValue, string.Empty);
                    updateProduct.SKUShortDescription = ValidationHelper.GetString(txtShortDes.Text, string.Empty);
                    updateProduct.SKUDescription = ValidationHelper.GetString(txtLongDes.Text, string.Empty);
                    updateProduct.SKUPrice = ValidationHelper.GetDouble(txtActualPrice.Text, default(double));
                    updateProduct.SKUValidUntil = ValidationHelper.GetDate(txtExpDate.Text, DateTime.Now.Date);
                    updateProduct.SKUEnabled = ValidationHelper.GetString(ddlStatus.SelectedValue, "1") == "1" ? true : false;
                    updateProduct.SKUSiteID = CurrentSite.SiteID;
                    updateProduct.SKUProductType = SKUProductTypeEnum.EProduct;
                    updateProduct.SKUAvailableItems = ValidationHelper.GetInteger(txtQuantity.Text, 0);
                    SKUInfoProvider.SetSKUInfo(updateProduct);
                }
                product.Update();
                var saveproductID = ValidationHelper.GetInteger(product.CampaignsProductID, 0);
                UpdateAllocateProduct(saveproductID);
                lblSuccessMsg.Visible = true;
                lblFailureText.Visible = false;
                EmptyFields(true);
                var redirectUrl = ValidationHelper.GetString(ViewState["LastPageUrl"], string.Empty);
                Response.Redirect(redirectUrl, false);
            }
            else
            {
                lblFailureText.Visible = true;
            }
        }
        /// <summary>
        /// Set the field with the record which user wants to edit
        /// </summary>
        /// <param name="productid">The id of the product which user wants to edit</param>
        private void SetFeild(int productID)
        {
            try
            {
                if (productID != 0)
                {
                    CampaignsProduct product = CampaignsProductProvider.GetCampaignsProducts()
                        .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                        .WhereEquals("CampaignsProductID", productID)
                        .FirstOrDefault();
                    if (product != null)
                    {
                        SKUInfo skuDetails = SKUInfoProvider.GetSKUs().WhereEquals("SKUID", product.SKUID).FirstObject;
                        if (skuDetails != null)
                        {
                            string folderName = libraryFolderName;
                            folderName = !string.IsNullOrEmpty(folderName) ? folderName.Replace(" ", "") : "CampaignProducts";
                            txtLongDes.Text = skuDetails.SKUDescription;
                            txtEstPrice.Text = ValidationHelper.GetString(skuDetails.SKUPrice, string.Empty);
                            ddlPosNo.SelectedValue = ValidationHelper.GetString(skuDetails.SKUNumber, string.Empty);
                            txtShortDes.Text = skuDetails.SKUName;
                            txtActualPrice.Text = ValidationHelper.GetString(skuDetails.SKUPrice, string.Empty);
                            ddlStatus.SelectedValue = skuDetails.SKUEnabled == true ? "1" : "0";
                            imgProduct.ImageUrl = MediaFileURLProvider.GetMediaFileUrl(CurrentSiteName, folderName, ValidationHelper.GetString(skuDetails.SKUImagePath, string.Empty));
                            imgProduct.Visible = imgProduct.ImageUrl != string.Empty ? true : false;
                            txtExpDate.Text = ValidationHelper.GetString(skuDetails.SKUValidUntil, string.Empty);
                            txtQuantity.Text = ValidationHelper.GetString(skuDetails.SKUAvailableItems, string.Empty);
                        }
                        txtBundleQnt.Text = ValidationHelper.GetString(product.BundleQty, string.Empty);
                        ddlBrand.SelectedValue = ValidationHelper.GetString(product.BrandID, string.Empty);
                        ddlState.SelectedValue = ValidationHelper.GetString(product.State, string.Empty);
                        ddlProdCategory.SelectedValue = ValidationHelper.GetString(product.CategoryID, string.Empty);
                        chkcancel.Checked = ValidationHelper.GetBoolean(product.Cancelled, false);
                        txtCVOProductId.Text = ValidationHelper.GetString(product.CVOProductID, string.Empty);
                        txtStroeFrontId.Text = ValidationHelper.GetString(product.StoreFrontProductID, string.Empty);
                        BindEditProduct(ValidationHelper.GetInteger(product.CampaignsProductID, 0));
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("GetProductFromButtonClick", "EXCEPTION", ex);
            }
        }

        /// <summary>
        /// Empty all the field of the form
        /// </summary>
        private void EmptyFields(bool IsChanged)
        {
            if (IsChanged)
            {
                ddlPosNo.SelectedIndex = 0;

            }
            ddlBrand.SelectedIndex = 0;
            ddlProdCategory.SelectedIndex = 0;
            ddlState.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            txtActualPrice.Text = string.Empty;
            txtBundleQnt.Text = string.Empty;
            txtCVOProductId.Text = string.Empty;
            txtEstPrice.Text = string.Empty;
            txtExpDate.Text = string.Empty;
            txtLongDes.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtShortDes.Text = string.Empty;
            txtStroeFrontId.Text = string.Empty;
            RepSelectedUser.DataSource = string.Empty;
            RepSelectedUser.DataBind();
            lstUsers = new List<Product.AllocateProduct>();
        }

        /// <summary>
        /// This method will get all the users and bind it to repeater
        /// </summary>
        /// <param name="pageIndex"></param>
        private void BindUsers(int pageIndex)
        {
            List<AllocateProduct> lstAllocatedProd = new List<AllocateProduct>();
            var users = UserInfoProvider.GetUsers().Columns("Email", "UserID", "FullName")
                .Skip(PageSize * (pageIndex - 1))
                .Take(PageSize);
            foreach (UserInfo user in users)
            {
                AllocateProduct objProduct = new AllocateProduct();
                objProduct.EmailID = user.Email;
                objProduct.UserID = user.UserID;
                objProduct.UserName = user.FullName;
                if (lstUsers.FindIndex(item => item.UserID == user.UserID) > -1)
                {
                    objProduct.Selected = true;
                }
                lstAllocatedProd.Add(objProduct);
            }
            RepterDetails.DataSource = lstAllocatedProd;
            RepterDetails.DataBind();
            PopulatePager(UserInfoProvider.GetUsers().Count(), pageIndex);
        }

        /// <summary>
        /// This method will create pagination for repeater
        /// </summary>
        /// <param name="recordCount">The no of record to fetch</param>
        /// <param name="currentPage">Current page no</param>

        private void PopulatePager(int recordCount, int currentPage)
        {
            double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(PageSize));
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();
        }

        /// <summary>
        ///  This method will get all the allocated user
        /// </summary>
        /// <param name="ProductId"></param>
        private void BindEditProduct(int ProductId)
        {
            try
            {
                lstUsers = new List<AllocateProduct>();
                List<AllocateProduct> lstProduct = new List<AllocateProduct>();
                string customTableClassName = "KDA.UserAllocatedProducts";
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    var customTableData = CustomTableItemProvider.GetItems(customTableClassName)
                                                             .WhereStartsWith("ProductID", ProductId.ToString());
                    foreach (CustomTableItem item in customTableData)
                    {
                        AllocateProduct objProduct = new AllocateProduct();
                        objProduct.EmailID = ValidationHelper.GetString(item.GetValue("EmailID"), string.Empty);
                        objProduct.UserName = UserInfoProvider.GetUserInfo(ValidationHelper.GetInteger(item.GetValue("UserID"), 0)).FullName;
                        objProduct.Quantity = ValidationHelper.GetInteger(item.GetValue("Quantity"), 0);
                        objProduct.UserID = ValidationHelper.GetInteger(item.GetValue("UserID"), 0);
                        lstProduct.Add(objProduct);
                    }
                    lstUsers.AddRange(lstProduct);
                }
                RepSelectedUser.DataSource = lstProduct;
                RepSelectedUser.DataBind();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Product allocation update BindEdit", "EXCEPTION", ex);
            }
        }

        /// <summary>
        ///  Method to bind the data to the dropdowns
        /// </summary>
        private void BindData()
        {
            try
            {
                GetBrandName();
                var pos = CustomTableItemProvider.GetItems(POSNumberItem.CLASS_NAME).Columns("POSNumber").WhereEquals("Enable", 1).ToList();
                if (!DataHelper.DataSourceIsEmpty(pos))
                {
                    ddlPosNo.DataSource = pos;
                    ddlPosNo.DataTextField = "POSNumber";
                    ddlPosNo.DataValueField = "POSNumber";
                    ddlPosNo.DataBind();
                    string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.InvProductForm.PosNoWaterMark"), string.Empty);
                    ddlPosNo.Items.Insert(0, new ListItem(selectText, "0"));
                }
                BindCategories();
                var states = StateInfoProvider.GetStates().Columns("StateID,StateName").ToList();
                if (!DataHelper.DataSourceIsEmpty(states))
                {
                    ddlState.DataSource = states;
                    ddlState.DataTextField = "StateName";
                    ddlState.DataValueField = "StateID";
                    ddlState.DataBind();
                    string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.InvProductForm.StateWaterMark"), string.Empty);
                    ddlState.Items.Insert(0, new ListItem(selectText, "0"));
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CMSWebParts_Kadena_POS_POSForm_BindDataToDropdowns", "BindData", ex.Message);
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
                var brands = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME).Columns("ItemID,BrandName").ToList();
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
        /// Bind categories to dropdown
        /// </summary>
        public void BindCategories()
        {
            try
            {
                var categories = ProductCategoryProvider.GetProductCategories()
                    .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                    .Columns("ProductCategoryID,ProductCategoryTitle")
                    .ToList();
                if (!DataHelper.DataSourceIsEmpty(categories))
                {
                    ddlProdCategory.DataSource = categories;
                    ddlProdCategory.DataTextField = "ProductCategoryTitle";
                    ddlProdCategory.DataValueField = "ProductCategoryID";
                    ddlProdCategory.DataBind();
                    string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.InvProductForm.CategoryWaterMark"),
                        string.Empty);
                    ddlProdCategory.Items.Insert(0, new ListItem(selectText, "0"));
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CMSWebParts_Kadena_Inventroy_Web_Products", "BindCategories", ex, CurrentSite.SiteID, ex.Message);
            }
        }

        #endregion PrivateMethods

        protected void ddlPosNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                string selectedPos = ddlPosNo.SelectedValue;
                SKUInfo skuDetails = SKUInfoProvider.GetSKUs().WhereEquals("SKUNumber", selectedPos).FirstObject;
                if (skuDetails != null)
                {
                    string folderName = libraryFolderName;
                    folderName = !string.IsNullOrEmpty(folderName) ? folderName.Replace(" ", "") : "CampaignProducts";
                    txtLongDes.Text = skuDetails.SKUDescription;
                    txtEstPrice.Text = ValidationHelper.GetString(skuDetails.SKUPrice, string.Empty);
                    ddlPosNo.SelectedValue = ValidationHelper.GetString(skuDetails.SKUNumber, string.Empty);
                    txtShortDes.Text = skuDetails.SKUName;
                    txtActualPrice.Text = ValidationHelper.GetString(skuDetails.SKUPrice, string.Empty);
                    ddlStatus.SelectedValue = skuDetails.SKUEnabled == true ? "1" : "0";
                    imgProduct.ImageUrl = MediaFileURLProvider.GetMediaFileUrl(CurrentSiteName, folderName, ValidationHelper.GetString(skuDetails.SKUImagePath, string.Empty));
                    imgProduct.Visible = imgProduct.ImageUrl != string.Empty ? true : false;
                    txtExpDate.Text = ValidationHelper.GetString(skuDetails.SKUValidUntil, string.Empty);
                    txtQuantity.Text = ValidationHelper.GetString(skuDetails.SKUAvailableItems, string.Empty);
                    CampaignsProduct product = CampaignsProductProvider.GetCampaignsProducts().WhereEquals("NodeSKUID", skuDetails.SKUID).FirstObject;
                    if (product != null)
                    {
                        txtBundleQnt.Text = ValidationHelper.GetString(product.BundleQty, string.Empty);
                        ddlBrand.SelectedValue = ValidationHelper.GetString(product.BrandID, string.Empty);
                        ddlState.SelectedValue = ValidationHelper.GetString(product.State, string.Empty);
                        ddlProdCategory.SelectedValue = ValidationHelper.GetString(product.CategoryID, string.Empty);
                        chkcancel.Checked = ValidationHelper.GetBoolean(product.Cancelled, false);
                        txtCVOProductId.Text = ValidationHelper.GetString(product.CVOProductID, string.Empty);
                        txtStroeFrontId.Text = ValidationHelper.GetString(product.StoreFrontProductID, string.Empty);
                        BindEditProduct(ValidationHelper.GetInteger(product.CampaignsProductID, 0));
                        ViewState["ProductId"] = product.CampaignsProductID;
                    }
                   
                }
                else
                {
                    ViewState["ProductId"] = null;
                    EmptyFields(false);
                }
                BindData();
                ddlPosNo.SelectedValue = selectedPos;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CMSWebParts_Kadena_Inventory_porduct_POSchanged", "BindCategories", ex, CurrentSite.SiteID, ex.Message);
            }
        }
    }

    #region class

    /// <summary>
    /// Properties related to product allocation
    /// </summary>
    public class AllocateProduct
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public bool Selected { get; set; }
        public int Quantity { get; set; }
    }

    #endregion class
}