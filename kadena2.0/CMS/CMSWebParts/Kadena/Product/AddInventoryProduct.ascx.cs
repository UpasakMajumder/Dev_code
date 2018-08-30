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
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class AddInventoryProduct : CMSAbstractWebPart
    {
        #region variables

        private int productId = 0;
        private int ProductId => productId == 0
            ? productId = QueryHelper.GetInteger("ID", 0)
            : productId;

        private bool IsEdittingExistingProduct => ProductId > 0;

        private string folderpath = "/";
        private string libraryFolderName = string.Empty;

        private int PageSize => 10;

        private int SelectedBrandId => ValidationHelper.GetInteger(ddlBrand.SelectedValue, 0);
        private int SelectedPosCategoryId => ValidationHelper.GetInteger(ddlPosCategory.SelectedValue, 0);
        private int SelectedProductCategoryId => ValidationHelper.GetInteger(ddlProdCategory.SelectedValue, 0);
        private string SelectedPosNo => ddlPosNo.SelectedIndex > 0 ? ddlPosNo.SelectedValue : string.Empty;

        List <ProductAllocationValue> _allocationsCurrent;
        List<ProductAllocationValue> _allocationsUncommittedChanges;

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
            if (StopProcessing)
            {
                return;
            }

            BindLabelText();

            if (!IsPostBack)
            {
                BindData();

                if (IsEdittingExistingProduct)
                {
                    BindProductDataToForm();
                }

                BindUsers(pageIndex: 1);
            }

            btnAllocateProduct.Click += AllocateProduct_Click;
            btnCancel.Click += (snd, args) => Response.Redirect(CurrentDocument.Parent.DocumentUrlPath, false);

            if (IsEdittingExistingProduct)
            {
                btnSave.Click += (snd, args) => UpdateProduct();
            }
            else
            {
                btnSave.Click += (snd, args) => SaveProduct();
            }
        }

        public void BindLabelText()
        {
            hdnDatepickerUrl.Value = SettingsKeyInfoProvider.GetValue("KDA_DatePickerPath", CurrentSiteName);
            rfvBrand.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.BrandRequired");
            rfvActualPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ActualPriceRequired");
            rfvPosNo.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.POSCodeRequired");
            rfvProdCategory.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.POSCategroyRequired");
            revQuantity.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.NumberOnly");
            rfvShortDes.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ShortDescriptionRequired");
            revBundleQnt.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.NumberOnly");
            rfvBundleQnt.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.BundleQntRequired");
            rfvWeight.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.WeightRequired");
            revWeigth.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.NumberOnly");
            revEstPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.NumberOnly");
            revActualPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.NumberOnly");
            rfvPosCategory.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ProductCategoryErrorMessage");

            folderpath = SettingsKeyInfoProvider.GetValue("KDA_InventoryProductFolderPath", CurrentSiteName);
            libraryFolderName = SettingsKeyInfoProvider.GetValue("KDA_InventoryProductImageFolderName", CurrentSiteName);
        }

        #endregion WebpartSetupMethods

        #region ButtonclickEvents

        private List<ProductAllocationValue> GetAllocationsFromForm()
        {
            var allocations = new List<ProductAllocationValue>();
            foreach (RepeaterItem row in RepterDetails.Items)
            {
                allocations.Add(new ProductAllocationValue
                {
                    UserID = ValidationHelper.GetInteger(((Label)row.FindControl("lblUserid")).Text, 0),
                    Selected = ((CheckBox)row.FindControl("chkAllocate")).Checked,
                    Quantity = ValidationHelper.GetInteger(((TextBox)row.FindControl("txtAllQuantity")).Text, 0)
                });
            }

            return allocations;
        }

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
                            ProductAllocation objAllocateProduct = new ProductAllocation();
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
                            ProductAllocation objAllocateProduct = new ProductAllocation();
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
                foreach (ProductAllocation User in lstUsers)
                {
                    newCustomTableItem.SetValue("UserID", User.UserID);
                    newCustomTableItem.SetValue("ProductID", productID);
                    newCustomTableItem.SetValue("Quantity", User.Quantity);
                    newCustomTableItem.SetValue("EmailID", User.EmailID);
                    newCustomTableItem.Insert();
                }
            }
        }
        
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
                        }
                    }
                }
                CustomTableItem newCustomTableItem = CustomTableItem.New(customTableClassName);
                foreach (ProductAllocation User in lstUsers)
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

        private void SaveProduct()
        {
            if (SelectedBrandId == 0 || SelectedProductCategoryId == 0 || string.IsNullOrEmpty(SelectedPosNo))
            {
                return;
            }

            var tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            var parentPage = tree.SelectNodes()
                .Path(folderpath)
                .OnCurrentSite()
                .Culture(DocumentContext.CurrentDocument.DocumentCulture)
                .FirstObject;
            if (parentPage != null)
            {
                // sku
                var newSkuProduct = new SKUInfo()
                {
                    SKUNumber = "00000",
                };
                newSkuProduct.SetValue("SKUProductCustomerReferenceNumber", SelectedPosNo);

                MapFormFieldsToSku(newSkuProduct);

                SKUInfoProvider.SetSKUInfo(newSkuProduct);

                // product
                var product = new CampaignsProduct()
                {
                    DocumentCulture = CurrentDocument.DocumentCulture,
                    NodeSKUID = newSkuProduct.SKUID
                };

                MapFormFieldsToProduct(product);

                if (productImage.HasFile)
                {
                    product.ProductImage = UploadImage.UploadImageToMeadiaLibrary(productImage, libraryFolderName);
                }

                var templateName = SettingsKeyInfoProvider.GetValue("KDA_InventoryProductPageTemplateName", CurrentSiteName);
                var template = PageTemplateInfoProvider.GetPageTemplateInfo(templateName);
                if (template != null)
                {
                    product.DocumentPageTemplateID = template.PageTemplateId;
                }
                product.Insert(parentPage, true);

                AllocateProductToUsers(product.CampaignsProductID);

                SetStatusAndRedirect(QueryStringStatus.Added);
            }
            else
            {
                lblFailureText.Visible = true;
            }
        }

        private void MapFormFieldsToProduct(CampaignsProduct product)
        {
            product.BrandID = SelectedBrandId;
            product.CategoryID = SelectedProductCategoryId;
            product.EstimatedPrice = ValidationHelper.GetDouble(txtEstPrice.Text, default(double));
            product.State = ValidationHelper.GetInteger(ddlState.SelectedValue, default(int));
            product.DocumentName = product.ProductName = ValidationHelper.GetString(txtShortDes.Text, string.Empty);
        }

        private void MapFormFieldsToSku(SKUInfo sku)
        {
            sku.SKUShortDescription = sku.SKUName = ValidationHelper.GetString(txtShortDes.Text, string.Empty);
            sku.SKUDescription = ValidationHelper.GetString(txtLongDes.Text, string.Empty);
            sku.SKUPrice = ValidationHelper.GetDouble(txtActualPrice.Text, default(double));
            sku.SKUEnabled = ValidationHelper.GetBoolean(ddlStatus.SelectedValue, false);
            sku.SKUAvailableItems = ValidationHelper.GetInteger(txtQuantity.Text, 0);
            sku.SKUSiteID = CurrentSite.SiteID;
            sku.SKUProductType = SKUProductTypeEnum.EProduct;
            sku.SKUWeight = ValidationHelper.GetDouble(txtWeight.Text, default(double));
            sku.SKUValidUntil = ValidationHelper.GetDateTime(txtExpDate.Text, DateTime.MinValue);
            sku.SetValue("SKUNumberOfItemsInPackage", ValidationHelper.GetInteger(txtBundleQnt.Text, default(int)));
        }

        private void UpdateProduct()
        {
            if (SelectedProductCategoryId == 0)
            {
                return;
            }

            var productID = ProductId;
            var product = CampaignsProductProvider
                .GetCampaignsProducts()
                .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                .WhereEquals("CampaignsProductID", productID)
                .FirstOrDefault();
            if (product != null)
            {
                // product
                MapFormFieldsToProduct(product);

                if (productImage.HasFile)
                {
                    if (product.ProductImage != string.Empty)
                    {
                        UploadImage.DeleteImage(product.ProductImage, libraryFolderName);
                    }
                    product.ProductImage = UploadImage.UploadImageToMeadiaLibrary(productImage, libraryFolderName);
                }

                product.Update();
                UpdateAllocateProduct(productID);

                // sku
                var sku = SKUInfoProvider.GetSKUInfo(product.NodeSKUID);
                if (sku != null)
                {
                    MapFormFieldsToSku(sku);
                    SKUInfoProvider.SetSKUInfo(sku);
                }

                SetStatusAndRedirect(QueryStringStatus.Updated);
            }
            else
            {
                lblFailureText.Visible = true;
            }
        }

        private void SetStatusAndRedirect(string status)
        {
            Response.Cookies["status"].Value = status;
            Response.Cookies["status"].HttpOnly = false;
            URLHelper.Redirect($"{CurrentDocument.Parent.DocumentUrlPath}?status={status}");
        }

        private void BindProductDataToForm(CampaignsProduct product, SKUInfo sku)
        {
            if (product != null)
            {
                imgProduct.ImageUrl = ValidationHelper.GetString(product.ProductImage, string.Empty);
                imgProduct.Visible = imgProduct.ImageUrl != string.Empty;
                ddlBrand.SelectedValue = ValidationHelper.GetString(product.BrandID, string.Empty);
                ddlBrand.Enabled = !IsEdittingExistingProduct;
                ddlState.SelectedValue = ValidationHelper.GetString(product.State, string.Empty);
                ddlProdCategory.SelectedValue = ValidationHelper.GetString(product.CategoryID, string.Empty);
                txtEstPrice.Text = ValidationHelper.GetString(product.EstimatedPrice, string.Empty);

                BindEditProduct(product.CampaignsProductID);
            }

            if (sku != null)
            {
                txtBundleQnt.Text = ValidationHelper.GetString(sku.GetIntegerValue("SKUNumberOfItemsInPackage", 1), string.Empty);
                txtLongDes.Text = sku.SKUDescription;
                txtShortDes.Text = sku.SKUName;
                txtSKUNumber.Text = sku.SKUNumber;
                txtActualPrice.Text = ValidationHelper.GetString(sku.SKUPrice, string.Empty);
                ddlStatus.SelectedValue = sku.SKUEnabled ? "1" : "0";
                txtQuantity.Text = ValidationHelper.GetString(sku.SKUAvailableItems, string.Empty);
                txtWeight.Text = ValidationHelper.GetString(sku.SKUWeight, string.Empty);

                if (IsEdittingExistingProduct)
                {
                    var posNumber = sku.GetValue("SKUProductCustomerReferenceNumber", string.Empty);

                    ddlPosCategory.SelectedValue = GetPosCategoryId(posNumber);
                    ddlPosCategory.Enabled = false;
                    ddlPosNo.Visible = false;
                    txtPOSNumber.Text = posNumber;
                    txtPOSNumber.Visible = true;

                    if (sku.SKUValidUntil != DateTime.MinValue)
                    {
                        txtExpDate.Text = ValidationHelper.GetString(sku.SKUValidUntil.ToShortDateString(), string.Empty);
                    }
                }
            }
        }

        private void BindProductDataToForm()
        {
            CampaignsProduct product = CampaignsProductProvider.GetCampaignsProducts()
                .OnCurrentSite()
                .WhereEquals("CampaignsProductID", ProductId)
                .FirstOrDefault();
            if (product != null)
            {
                SKUInfo sku = SKUInfoProvider.GetSKUs()
                    .WhereEquals("SKUID", product.NodeSKUID)
                    .FirstObject;

                BindProductDataToForm(product, sku);
            }
        }

        /// <summary>
        /// This method will get all the users and bind it to repeater
        /// </summary>
        /// <param name="pageIndex"></param>
        private void BindUsers(int pageIndex)
        {
            string customTableClassName = "KDA.UserAllocatedProducts";
            List<ProductAllocation> lstAllocatedProd = new List<ProductAllocation>();
            var users = UserInfoProvider.GetUsers().Columns("Email", "UserID", "FullName").OnSite(CurrentSite.SiteID).OrderBy("FullName")
                .Skip(PageSize * (pageIndex - 1))
                .Take(PageSize);
            foreach (UserInfo user in users)
            {
                ProductAllocation objProduct = new ProductAllocation();
                objProduct.EmailID = user.Email;
                objProduct.UserID = user.UserID;
                objProduct.UserName = user.FullName;
                objProduct.Quantity = CustomTableItemProvider.GetItems(customTableClassName)
                                                             .WhereEquals("ProductID", ProductId)
                                                             .WhereEquals("UserID", user.UserID)
                                                             .FirstOrDefault()?.GetValue("Quantity", default(int)) ?? 0;
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
        
        private void PopulatePager(int recordCount, int currentPage)
        {
            double dblPageCount = (double)(recordCount / Convert.ToDecimal(PageSize));
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
                lstUsers = new List<ProductAllocation>();
                List<ProductAllocation> lstProduct = new List<ProductAllocation>();
                string customTableClassName = "KDA.UserAllocatedProducts";
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    var customTableData = CustomTableItemProvider.GetItems(customTableClassName)
                                                             .WhereStartsWith("ProductID", ProductId.ToString());
                    foreach (CustomTableItem item in customTableData)
                    {
                        ProductAllocation objProduct = new ProductAllocation();
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

        private void BindData()
        {
            BindStatus();
            BindBrand();
            BindCategory();
            BindStateGroup();
            BindPosCategory();
            BindAvailablePOSNumbers();
        }

        public void BindBrand()
        {
            var brands = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME)
                .Columns("ItemId", "BrandName")
                .OrderBy("BrandName")
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

        public void BindStateGroup()
        {
            var states = CustomTableItemProvider.GetItems(StatesGroupItem.CLASS_NAME)
                .Columns("ItemID,States,GroupName")
                .ToList();
            if (!DataHelper.DataSourceIsEmpty(states))
            {
                // states in dropdown
                ddlState.DataSource = states;
                ddlState.DataTextField = "GroupName";
                ddlState.DataValueField = "ItemID";
                ddlState.DataBind();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.InvProductForm.StateWaterMark"), string.Empty);
                ddlState.Items.Insert(0, new ListItem(selectText, "0"));

                // states (again) in pop-up
                RepStateInfo.DataSource = states;
                RepStateInfo.DataBind();
            }
        }

        public void BindCategory()
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

        public void BindStatus()
        {
            ddlStatus.Items.Clear();
            ddlStatus.Items.AddRange(new[]
            {
                new ListItem(ResHelper.GetString("KDA.Common.Status.Active"), "1"),
                new ListItem(ResHelper.GetString("KDA.Common.Status.Inactive"), "0")
            });
        }

        #endregion PrivateMethods

        protected void ddlPosNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sku = SKUInfoProvider.GetSKUs()
                .WhereEquals("SKUProductCustomerReferenceNumber", SelectedPosNo)
                .FirstObject;
            if (sku != null)
            {
                var product = CampaignsProductProvider.GetCampaignsProducts()
                    .WhereEquals("NodeSKUID", sku.SKUID)
                    .FirstObject;

                BindProductDataToForm(product, sku);
            }
        }

        public void BindPosCategory()
        {
            var posCategories = CustomTableItemProvider.GetItems<POSCategoryItem>()
                .Columns("PosCategoryName,ItemID")
                .ToList();
            ddlPosCategory.DataSource = posCategories;
            ddlPosCategory.DataTextField = "PosCategoryName";
            ddlPosCategory.DataValueField = "ItemID";
            ddlPosCategory.DataBind();
            ddlPosCategory.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.InvProductForm.SelectPosCategory"), "0"));
        }

        protected void ddlPosCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindAvailablePOSNumbers();
        }

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindAvailablePOSNumbers();
        }

        public void BindAvailablePOSNumbers()
        {
            var pos = ConnectionHelper.ExecuteQuery("KDA.CampaignsProduct.GetGIPos", null);

            var filterClauses = new List<string>();

            if (SelectedPosCategoryId > 0)
            {
                filterClauses.Add("POSCategoryID=" + SelectedPosCategoryId);
            }

            if (SelectedBrandId > 0)
            {
                var brandCode = CustomTableItemProvider.GetItems<BrandItem>()
                    .WhereEquals("ItemID", SelectedBrandId)
                    .Columns("BrandCode")
                    .FirstOrDefault()?.BrandCode ?? 0;
                filterClauses.Add("BrandID=" + brandCode);
            }

            var where = string.Join(" AND ", filterClauses);

            ddlPosNo.Items.Clear();

            if (!DataHelper.DataSourceIsEmpty(pos))
            {
                var posData = pos.Tables[0].Select(where);
                if (posData.Length > 0)
                {
                    ddlPosNo.DataSource = posData.CopyToDataTable();
                    ddlPosNo.DataTextField = "POSNumber";
                    ddlPosNo.DataValueField = "POSNumber";
                    ddlPosNo.DataBind();
                }
            }

            ddlPosNo.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.InvProductForm.SelectPosNO"), "0"));
        }

        public string GetPosCategoryId(string posNo)
        {
            if (string.IsNullOrEmpty(posNo))
                return string.Empty;

            var posCatName = CustomTableItemProvider.GetItems<POSNumberItem>()
                .WhereEquals("POSNumber", posNo)
                .Columns("POSCategoryID")
                .FirstOrDefault()?.POSCategoryID;
            return posCatName?.ToString();
        }
    }

    #region class

    public class ProductAllocation
    {
        public bool Selected { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public int Quantity { get; set; }
    }

    public class ProductAllocationValue
    {
        public bool Selected { get; set; }
        public int UserID { get; set; }
        public int Quantity { get; set; }
    }

    #endregion class
}