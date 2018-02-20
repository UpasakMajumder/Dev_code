using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.Constants;
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
                productId = QueryHelper.GetInteger("ID", 0);
                BindLabelText();
                if (productId > 0)
                {
                    btnSave.Click += btnUpdate_Click;
                }
                else
                {
                    btnSave.Click += btnSave_SavePOS;
                }
                if (!IsPostBack)
                {
                    BindData();
                    if (productId > 0)
                    {
                        SetFeild(productId);
                    }
                    else
                    {
                        var pos = ConnectionHelper.ExecuteQuery("KDA.CampaignsProduct.GetGIPos", null);
                        if (!DataHelper.DataSourceIsEmpty(pos))
                        {
                            ddlPosNo.DataSource = pos;
                            ddlPosNo.DataTextField = "POSNumber";
                            ddlPosNo.DataValueField = "POSNumber";
                            ddlPosNo.DataBind();
                            string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.InvProductForm.PosNoWaterMark"), string.Empty);
                            ddlPosNo.Items.Insert(0, new ListItem(selectText, "0"));
                        }
                    }
                    BindUsers(1);

                }

                btnAllocateProduct.Click += AllocateProduct_Click;
                btnCancel.Click += BtnCancel_Cancel;
                if (!IsPostBack)
                {
                    string currentDate = DateTime.Today.ToShortDateString();
                    compareDate.ValueToCompare = currentDate;
                }
            }
        }
        /// <summary>
        /// Binding the resource string text
        /// </summary>
        public void BindLabelText()
        {
            hdnDatepickerUrl.Value = SettingsKeyInfoProvider.GetValue("KDA_DatePickerPath", CurrentSiteName);
            rfvBrand.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.BrandRequired");
            rfvActualPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ActualPriceRequired");
            rfvPosNo.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.POSCodeRequired");
            rfvProdCategory.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.POSCategroyRequired");
            revQuantity.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.NumberOnly");
            rfvLongDes.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.LongDescritpionRequired");
            rfvShortDes.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ShortDescriptionRequired");
            revBundleQnt.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.NumberOnly");
            rfvBundleQnt.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.BundleQntRequired");
            rfvWeight.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.WeightRequired");
            revWeigth.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.NumberOnly");
            revEstPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.NumberOnly");
            revActualPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.NumberOnly");
            folderpath = SettingsKeyInfoProvider.GetValue("KDA_InventoryProductFolderPath", CurrentSiteName);
            libraryFolderName = SettingsKeyInfoProvider.GetValue("KDA_InventoryProductImageFolderName", CurrentSiteName);
            compareDate.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ExpiryDaterangeMessage");
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
                if (ddlBrand.SelectedIndex > 0 && ddlPosNo.SelectedIndex > 0 && ddlProdCategory.SelectedIndex > 0)
                {
                    if (ViewState["ProductId"] != null)
                    {
                        UpdateProduct(ValidationHelper.GetInteger(ViewState["ProductId"], 0));
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
            EmptyFields(true);
            lstUsers = new List<AllocateProduct>();
            Response.Redirect(CurrentDocument.Parent.DocumentUrlPath, false);
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
                        var userEmail = ValidationHelper.GetString(((Label)ri.FindControl("lblEmail")).Text, string.Empty);
                        if (index == -1)
                        {
                            AllocateProduct objAllocateProduct = new AllocateProduct();
                            objAllocateProduct.UserID = ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0);
                            objAllocateProduct.UserName = ValidationHelper.GetString(((Label)ri.FindControl("lblUserName")).Text, string.Empty);
                            objAllocateProduct.EmailID = ValidationHelper.GetString(((Label)ri.FindControl("lblEmail")).Text, string.Empty);
                            objAllocateProduct.Quantity = ValidationHelper.GetInteger(((TextBox)ri.FindControl("txtAllQuantity")).Text, 0);
                            lstUsers.Add(objAllocateProduct);
                        }
                        else
                        {
                            if (lstUsers.Count > 0)
                            {
                                lstUsers.Where(x => x.EmailID == userEmail).FirstOrDefault().Quantity = ValidationHelper.GetInteger(((TextBox)ri.FindControl("txtAllQuantity")).Text, 0);
                            }
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
                if (ddlProdCategory.SelectedIndex > 0)
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
                    QtyPerPack = ValidationHelper.GetInteger(txtBundleQnt.Text, default(int)),
                    BrandID = ValidationHelper.GetInteger(ddlBrand.SelectedValue, default(int)),
                    EstimatedPrice = ValidationHelper.GetDouble(txtEstPrice.Text, default(double)),
                    State = ValidationHelper.GetInteger(ddlState.SelectedValue, default(int)),
                    CategoryID = ValidationHelper.GetInteger(ddlProdCategory.SelectedValue, default(int)),
                    ProductName = ValidationHelper.GetString(txtShortDes.Text, string.Empty)
                };
                if (productImage.HasFile)
                {
                    imagePath = UploadImage.UploadImageToMeadiaLibrary(productImage, libraryFolderName);
                }
                SKUInfo newSkuProduct = new SKUInfo()
                {
                    SKUName = ValidationHelper.GetString(txtShortDes.Text, string.Empty),
                    SKUNumber = ValidationHelper.GetString("00000", string.Empty),
                    SKUDescription = ValidationHelper.GetString(txtLongDes.Text, string.Empty),
                    SKUPrice = ValidationHelper.GetDouble(txtActualPrice.Text, default(double)),
                    SKUEnabled = ValidationHelper.GetBoolean(ddlStatus.SelectedValue, false),
                    SKUAvailableItems = ValidationHelper.GetInteger(txtQuantity.Text, 0),
                    SKUImagePath = ValidationHelper.GetString(imagePath, string.Empty),
                    SKUSiteID = CurrentSite.SiteID,
                    SKUProductType = SKUProductTypeEnum.EProduct,
                    SKUWeight = ValidationHelper.GetDouble(txtWeight.Text, default(double))
                };
                if (!string.IsNullOrEmpty(txtExpDate.Text))
                {
                    newSkuProduct.SKUValidUntil = ValidationHelper.GetDateTime(txtExpDate.Text, DateTime.Now);
                }
                newSkuProduct.SetValue("SKUProductCustomerReferenceNumber", ValidationHelper.GetString(ddlPosNo.SelectedValue, string.Empty));
                products.DocumentName = ValidationHelper.GetString(txtShortDes.Text, string.Empty);
                products.DocumentCulture = CurrentDocument.DocumentCulture;
                SKUInfoProvider.SetSKUInfo(newSkuProduct);
                products.NodeSKUID = newSkuProduct.SKUID;
                PageTemplateInfo template = PageTemplateInfoProvider.GetPageTemplateInfo(SettingsKeyInfoProvider.GetValue("KDA_InventoryProductPageTemplateName", CurrentSiteName));
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
                Response.Cookies["status"].Value = QueryStringStatus.Added;
                Response.Cookies["status"].HttpOnly = false;
                URLHelper.Redirect($"{CurrentDocument.Parent.DocumentUrlPath}?status={QueryStringStatus.Added}");
            }
            else
            {
                lblFailureText.Visible = true;
            }
        }
        private void UpdateProduct(int productID)
        {
            CampaignsProduct product = CampaignsProductProvider
                           .GetCampaignsProducts()
                           .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                           .WhereEquals("CampaignsProductID", productID)
                           .FirstOrDefault();
            if (product != null)
            {
                product.DocumentName = ValidationHelper.GetString(txtShortDes.Text, string.Empty);
                product.QtyPerPack = ValidationHelper.GetInteger(txtBundleQnt.Text, default(int));
                product.BrandID = ValidationHelper.GetInteger(ddlBrand.SelectedValue, default(int));
                product.CategoryID = ValidationHelper.GetInteger(ddlProdCategory.SelectedValue, default(int));
                product.EstimatedPrice = ValidationHelper.GetInteger(txtEstPrice.Text, default(int));
                product.ProductName = ValidationHelper.GetString(txtShortDes.Text, string.Empty);
                product.State = ValidationHelper.GetInteger(ddlState.SelectedValue, default(int));
                SKUInfo updateProduct = SKUInfoProvider.GetSKUs().WhereEquals("SKUID", product.NodeSKUID).FirstObject;
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
                    updateProduct.SKUShortDescription = ValidationHelper.GetString(txtShortDes.Text, string.Empty);
                    updateProduct.SKUDescription = ValidationHelper.GetString(txtLongDes.Text, string.Empty);
                    updateProduct.SKUPrice = ValidationHelper.GetDouble(txtActualPrice.Text, default(double));
                    updateProduct.SKUEnabled = ValidationHelper.GetString(ddlStatus.SelectedValue, "1") == "1" ? true : false;
                    updateProduct.SKUSiteID = CurrentSite.SiteID;
                    updateProduct.SKUProductType = SKUProductTypeEnum.EProduct;
                    updateProduct.SKUAvailableItems = ValidationHelper.GetInteger(txtQuantity.Text, 0);
                    updateProduct.SKUWeight = ValidationHelper.GetDouble(txtWeight.Text, default(double));
                    updateProduct.SKUValidUntil = ValidationHelper.GetDateTime(txtExpDate.Text, DateTime.MinValue);
                    SKUInfoProvider.SetSKUInfo(updateProduct);
                }
                product.Update();
                var saveproductID = ValidationHelper.GetInteger(product.CampaignsProductID, 0);
                UpdateAllocateProduct(saveproductID);
                lblSuccessMsg.Visible = true;
                lblFailureText.Visible = false;
                EmptyFields(true);
                Response.Cookies["status"].Value = QueryStringStatus.Updated;
                Response.Cookies["status"].HttpOnly = false;
                URLHelper.Redirect($"{CurrentDocument.Parent.DocumentUrlPath}?status={QueryStringStatus.Updated}");
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
                        SKUInfo skuDetails = SKUInfoProvider.GetSKUs()
                            .WhereEquals("SKUID", product.NodeSKUID)
                            .FirstObject;
                        if (skuDetails != null)
                        {
                            string folderName = libraryFolderName;
                            folderName = !string.IsNullOrEmpty(folderName) ? folderName.Replace(" ", "") : "InventoryProducts";
                            txtLongDes.Text = skuDetails.SKUDescription;
                            txtEstPrice.Text = ValidationHelper.GetString(product.EstimatedPrice, string.Empty);
                            ddlPosNo.Items.Add(new ListItem(ValidationHelper.GetString(skuDetails.GetValue("SKUProductCustomerReferenceNumber", string.Empty), string.Empty), ValidationHelper.GetString(skuDetails.GetValue("SKUProductCustomerReferenceNumber", string.Empty), string.Empty)));
                            ddlPosNo.Enabled = false;
                            txtShortDes.Text = skuDetails.SKUName;
                            txtActualPrice.Text = ValidationHelper.GetString(skuDetails.SKUPrice, string.Empty);
                            ddlStatus.SelectedValue = skuDetails.SKUEnabled == true ? "1" : "0";
                            imgProduct.ImageUrl = ValidationHelper.GetString(skuDetails.SKUImagePath, string.Empty);
                            imgProduct.Visible = imgProduct.ImageUrl != string.Empty ? true : false;
                            if(skuDetails.SKUValidUntil!=DateTime.MinValue)
                            {
                              txtExpDate.Text = ValidationHelper.GetString(skuDetails.SKUValidUntil.ToShortDateString(), string.Empty);
                            }
                            txtQuantity.Text = ValidationHelper.GetString(skuDetails.SKUAvailableItems, string.Empty);
                            txtWeight.Text = ValidationHelper.GetString(skuDetails.SKUWeight, string.Empty);
                        }
                        txtBundleQnt.Text = ValidationHelper.GetString(product.QtyPerPack, string.Empty);
                        ddlBrand.SelectedValue = ValidationHelper.GetString(product.BrandID, string.Empty);
                        ddlState.SelectedValue = ValidationHelper.GetString(product.State, string.Empty);
                        ddlProdCategory.SelectedValue = ValidationHelper.GetString(product.CategoryID, string.Empty);
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
            txtEstPrice.Text = string.Empty;
            txtExpDate.Text = string.Empty;
            txtLongDes.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtShortDes.Text = string.Empty;
            txtWeight.Text = string.Empty;
            RepSelectedUser.DataSource = string.Empty;
            RepSelectedUser.DataBind();
            lstUsers = new List<Product.AllocateProduct>();
            imgProduct.ImageUrl = string.Empty;
        }

        /// <summary>
        /// This method will get all the users and bind it to repeater
        /// </summary>
        /// <param name="pageIndex"></param>
        private void BindUsers(int pageIndex)
        {
            string customTableClassName = "KDA.UserAllocatedProducts";
            List<AllocateProduct> lstAllocatedProd = new List<AllocateProduct>();
            var users = UserInfoProvider.GetUsers().Columns("Email", "UserID", "FullName").OnSite(CurrentSite.SiteID).OrderBy("FullName")
                .Skip(PageSize * (pageIndex - 1))
                .Take(PageSize);
            foreach (UserInfo user in users)
            {
                AllocateProduct objProduct = new AllocateProduct();
                objProduct.EmailID = user.Email;
                objProduct.UserID = user.UserID;
                objProduct.UserName = user.FullName;
                objProduct.Quantity = CustomTableItemProvider.GetItems(customTableClassName)
                                                             .WhereEquals("ProductID", productId).WhereEquals("UserID", user.UserID).FirstOrDefault()?.GetValue("Quantity", default(int)) ?? 0;
                if (lstUsers.FindIndex(item => item.UserID == user.UserID) > -1)
                {
                    objProduct.Selected = true;
                }
                lstAllocatedProd.Add(objProduct);
            }
            RepterDetails.DataSource = lstAllocatedProd;
            RepterDetails.DataBind();
            PopulatePager(UserInfoProvider.GetUsers().OnSite(CurrentSite.SiteID).Count(), pageIndex);
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
                BindStatus();
                GetBrandName();
                BindCategories();
                GetStateGroup();
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
                var brands = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME)
                    .Columns("ItemId,BrandName")
                    .ToList();
                if (!DataHelper.DataSourceIsEmpty(brands))
                {
                    ddlBrand.DataSource = brands;
                    ddlBrand.DataTextField = "BrandName";
                    ddlBrand.DataValueField = "ItemId";
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
        /// Get the State list
        /// </summary>
        /// <returns></returns>
        public string GetStateGroup()
        {
            string returnValue = string.Empty;
            try
            {
                var states = CustomTableItemProvider.GetItems(StatesGroupItem.CLASS_NAME)
                    .Columns("ItemID,States,GroupName")
                    .ToList();
                if (!DataHelper.DataSourceIsEmpty(states))
                {
                    ddlState.DataSource = states;
                    ddlState.DataTextField = "GroupName";
                    ddlState.DataValueField = "ItemID";
                    ddlState.DataBind();
                    string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.InvProductForm.StateWaterMark"), string.Empty);
                    ddlState.Items.Insert(0, new ListItem(selectText, "0"));
                    RepStateInfo.DataSource = states;
                    RepStateInfo.DataBind();
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "GetStateGroup", ex, CurrentSite.SiteID, ex.Message);
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
                    .WhereEquals("Status", 1)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BindStatus()
        {
            ddlStatus.Items.Clear();
            ddlStatus.Items.Insert(0, new ListItem(ResHelper.GetString("KDA.Common.Status.Active"), "1"));
            ddlStatus.Items.Insert(1, new ListItem(ResHelper.GetString("KDA.Common.Status.Inactive"), "0"));
        }

        #endregion PrivateMethods

        protected void ddlPosNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedPos = ddlPosNo.SelectedValue;
                BindData();
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
                    imgProduct.ImageUrl = ValidationHelper.GetString(skuDetails.SKUImagePath, string.Empty);
                    imgProduct.Visible = imgProduct.ImageUrl != string.Empty ? true : false;
                    txtExpDate.Text = ValidationHelper.GetString(skuDetails.SKUValidUntil, string.Empty);
                    txtQuantity.Text = ValidationHelper.GetString(skuDetails.SKUAvailableItems, string.Empty);
                    txtWeight.Text = ValidationHelper.GetString(skuDetails.SKUWeight, string.Empty);
                    CampaignsProduct product = CampaignsProductProvider.GetCampaignsProducts().WhereEquals("NodeSKUID", skuDetails.SKUID).FirstObject;
                    if (product != null)
                    {
                        txtBundleQnt.Text = ValidationHelper.GetString(product.QtyPerPack, string.Empty);
                        ddlBrand.SelectedValue = ValidationHelper.GetString(product.BrandID, string.Empty);
                        ddlState.SelectedValue = ValidationHelper.GetString(product.State, string.Empty);
                        ddlProdCategory.SelectedValue = ValidationHelper.GetString(product.CategoryID, string.Empty);
                        BindEditProduct(ValidationHelper.GetInteger(product.CampaignsProductID, 0));
                        ViewState["ProductId"] = product.CampaignsProductID;
                    }
                }
                else
                {
                    ViewState["ProductId"] = null;
                    EmptyFields(false);
                    var pos = CustomTableItemProvider.GetItems(POSNumberItem.CLASS_NAME)
                        .WhereEquals("POSNumber", selectedPos)
                         .FirstOrDefault();
                    if (pos != null && pos.GetValue("BrandID") != null)
                    {
                        var brand = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME)
                       .WhereEquals("BrandCode", pos.GetValue("BrandID"))
                       .FirstOrDefault();
                        if (brand != null)
                        {
                            ddlBrand.SelectedValue = brand.GetValue("ItemId").ToString();
                        }
                    }
                }

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