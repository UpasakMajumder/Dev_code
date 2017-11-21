using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.DocumentEngine.Web.UI;
using CMS.EventLog;
using CMS.Globalization;
using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.ImageUpload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class AddInventoryProduct : CMSAbstractWebPart
    {
        private int productId = 0;
        private string folderpath = "/";
        private int PageSize = 10;
        private static List<AllocateProduct> lstUsers = new List<AllocateProduct>();
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
            if (this.StopProcessing)
            {
                // Do not process
            }
            else
            {
                BindData();
                BindUsers(1);
                if (Request.QueryString["ID"] != null)
                {

                    btnSave.Click += btnUpdate_Click;
                    productId = ValidationHelper.GetInteger(Request.QueryString["ID"], 0);
                    SetFeild(productId);
                }
                else
                {
                    btnSave.Click += btnSave_SavePOS;
                }
                btnSave.Click += btnSave_SavePOS;
                btnAllocateProduct.Click += AllocateProduct_Click;
                btnCancel.Click += btnCancel_Cancel;
                rfvBrand.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.BrandRequired");
                rfvActualPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ActualPriceRequired");
                rfvPosNo.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.POSCodeRequired");
                rfvProdCategory.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.POSCategroyRequired");
                rfvExpDate.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ExpiryDateRequired");
                rfvEstPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.EstimatedPriceRequired");
                rfvImage.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ImageRequired");
                rfvLongDes.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.LongDescritpionRequired");

                rfvShortDes.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ShortDescriptionRequired");
                rfvState.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.StateRequired");

                rfvBundleQnt.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.BundleQntRequired");
                folderpath = SettingsKeyInfoProvider.GetValue("KDA_CampaignFolderPath", CurrentSiteName);

            }
        }
        //This method will return the Brand list 
        private static ObjectQuery<CustomTableItem> GetBrands()
        {

            // Prepares the code name (class name) of the custom table
            ObjectQuery<CustomTableItem> items = new ObjectQuery<CustomTableItem>();
            string customTableClassName = "KDA.Brand";
            try
            {
                // Gets the custom table
                DataClassInfo brandTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (brandTable != null)
                {
                    // Gets a custom table records 
                    items = CustomTableItemProvider.GetItems(customTableClassName).OrderBy("BrandName");
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CMSWebParts_Kadena_POS_ProductForm", "GetBrands", ex.Message);
            }

            return items;
        }
        //This method will return the POSNUmber list 
        private static ObjectQuery<CustomTableItem> GetPosNumber()
        {

            // Prepares the code name (class name) of the custom table
            ObjectQuery<CustomTableItem> items = new ObjectQuery<CustomTableItem>();
            string customTableClassName = "KDA.POSNumber";
            try
            {
                // Gets the custom table
                DataClassInfo PosTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (PosTable != null)
                {
                    // Gets a custom table records 
                    items = CustomTableItemProvider.GetItems(customTableClassName).OrderBy("POSNumber");

                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CMSWebParts_Kadena_Product_ProductForm", "GetPosNumber", ex.Message);
            }

            return items;
        }
        private List<ProductCategory> GetProductCategory()
        {
            // Creates an instance of the Tree provider
            List<ProductCategory> lstProdcategroy = new List<ProductCategory>();
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            var pages = tree.SelectNodes("KDA.ProductCategory");
            foreach (CMS.DocumentEngine.TreeNode page in pages)
            {
                ProductCategory category = new ProductCategory();
                category.CategoryId = page.GetValue("ProductCategoryID", 0);
                category.CategoryName = page.GetValue("ProductCategoryTitle", string.Empty);
                lstProdcategroy.Add(category);
            }
            return lstProdcategroy;
        }
        // Method to bind the data to the dropdowns
        private void BindData()
        {
            try
            {
                //Binding data to Brand dropdownlist
                ddlBrand.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.POSFrom.BrandWaterMark"), "0"));
                ObjectQuery<CustomTableItem> Brands = GetBrands();
                int brandindex = 1;
                foreach (CustomTableItem Brand in Brands)
                {
                    ddlBrand.Items.Insert(brandindex++, new ListItem(Brand.GetValue("BrandName").ToString(), Brand.GetValue("BrandCode").ToString()));
                }
                // Adds the '(any)' and '(default)' filtering options
                ddlPosNo.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.POSFrom.FiscalYearWaterMark"), "0"));
                ObjectQuery<CustomTableItem> PosNumbers = GetPosNumber();
                int Posindex = 1;
                foreach (CustomTableItem PosNumber in PosNumbers)
                {
                    ddlPosNo.Items.Insert(Posindex++, new ListItem(PosNumber.GetValue("POSNumber").ToString(), PosNumber.GetValue("ItemID").ToString()));
                }
                //Product Category DropdownList
                int ProdCategoryindex = 1;
                ddlProdCategory.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.POSFrom.FiscalYearWaterMark"), "0"));
                List<ProductCategory> lstProdcategories = GetProductCategory();
                foreach (ProductCategory lstProdcategory in lstProdcategories)
                {
                    ddlProdCategory.Items.Insert(ProdCategoryindex++, new ListItem(lstProdcategory.CategoryName, lstProdcategory.CategoryId.ToString()));
                }
                //Bind all the state to dropdown
                // Gets the state
                ObjectQuery<StateInfo> States = StateInfoProvider.GetStates();
                ddlState.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.POSFrom.FiscalYearWaterMark"), "0"));
                int Stateindex = 1;
                foreach (StateInfo State in States)
                {
                    ddlState.Items.Insert(Stateindex++, new ListItem(State.StateDisplayName, State.StateCode));
                }

            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CMSWebParts_Kadena_POS_POSForm_BindDataToDropdowns", "BindData", ex.Message);
            }
        }

        protected void btnSave_SavePOS(object sender, EventArgs e)
        {
            Guid imageGuid = default(Guid);
            try
            {
                if (ddlBrand.SelectedIndex > 0 && ddlPosNo.SelectedIndex > 0 && ddlProdCategory.SelectedIndex > 0 && ddlState.SelectedIndex > 0)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    CMS.DocumentEngine.TreeNode parentPage = tree.SelectNodes().Path(folderpath).OnCurrentSite().Culture("en-us").FirstObject;
                    if (parentPage != null)
                    {
                        // Creates a new page of the "CMS.MenuItem" page type
                        CMS.DocumentEngine.TreeNode newProduct = CMS.DocumentEngine.TreeNode.New("KDA.CampaignProduct", tree);

                        // Sets the properties of the new page
                        newProduct.DocumentCulture = "en-us";
                        newProduct.DocumentName = txtShortDes.Text;
                        newProduct.SetValue("BundleQty", ValidationHelper.GetString(txtBundleQnt.Text, string.Empty));
                        newProduct.SetValue("POSNumber", ValidationHelper.GetInteger(ddlPosNo.SelectedValue, 0));
                        newProduct.SetValue("BrandID", ValidationHelper.GetInteger(ddlBrand.SelectedValue, 0));
                        newProduct.SetValue("EstimatedPrice", ValidationHelper.GetString(txtEstPrice.Text, string.Empty));
                        newProduct.SetValue("ActualPrice", ValidationHelper.GetString(txtActualPrice.Text, string.Empty));
                        newProduct.SetValue("ShortDescription", ValidationHelper.GetString(txtShortDes.Text, string.Empty));
                        newProduct.SetValue("State", ValidationHelper.GetString(ddlState.SelectedItem.Text, string.Empty));
                        newProduct.SetValue("ExpirationDate", ValidationHelper.GetString(txtExpDate.Text, string.Empty));
                        newProduct.SetValue("CategoryID", ValidationHelper.GetInteger(ddlProdCategory.SelectedValue, 0));
                        newProduct.SetValue("Cancelled", ValidationHelper.GetBoolean(chkcancel.Checked, false));
                        newProduct.SetValue("CVOProductID", ValidationHelper.GetString(txtCVOProductId.Text, ""));
                        newProduct.SetValue("TotalQty", ValidationHelper.GetString(txtQuantity.Text, ""));
                        newProduct.SetValue("Status", ValidationHelper.GetString(ddlStatus.SelectedValue, ""));
                        if (productImage.HasFile)
                        {
                            imageGuid = UploadImage.UploadImageToMeadiaLibrary(productImage);
                            newProduct.SetValue("Image", ValidationHelper.GetString(imageGuid, ""));
                        }
                        // Inserts the new page as a child of the parent page
                        newProduct.Insert(parentPage);
                        var productID = ValidationHelper.GetInteger(newProduct.GetValue("CampaignProductID"), 0);
                        AllocateProductToUsers(productID);
                        lblSuccessMsg.Visible = true;
                        lblFailureText.Visible = false;
                        EmptyFields();
                    }
                    else
                    {
                        lblFailureText.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("SaveProductFromButtonClick", "EXCEPTION", ex);
                if (imageGuid != default(Guid))
                    UploadImage.DeleteImage(imageGuid);
                EventLogProvider.LogInformation("CMSWebParts_Kadena_Add_inventory_Products", "btnSave_Click", ex.Message);
            }

        }
        protected void btnCancel_Cancel(object sender, EventArgs e)
        {
            try
            {
                EmptyFields();
                lstUsers = new List<AllocateProduct>();
                //var redirectUrl = RequestContext.CurrentURL;

                //if (!String.IsNullOrEmpty(DefaultTargetUrl))
                //{
                //    redirectUrl = ResolveUrl(DefaultTargetUrl);
                //}

                //URLHelper.Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CancePOSFormButtonClick", "EXCEPTION", ex);
            }

        }
        /// <summary>
        /// Handles the Click event of the submit button.
        /// </summary>
        protected void AllocateProduct_Click(object sender, EventArgs e)
        {
            // Assigns the value of the UniSelector control to be displayed by the Label
            //lstUsers = new List<AllocateProduct>();
            foreach (RepeaterItem ri in RepterDetails.Items)
            {
                CheckBox item_check = (CheckBox)ri.FindControl("chkAllocate");
                if (item_check.Checked)
                {
                    int index = lstUsers.FindIndex(item => item.UserId == ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0));
                    if (index == -1)
                    {
                        AllocateProduct objAllocateProduct = new AllocateProduct();
                        objAllocateProduct.UserId = ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0);
                        objAllocateProduct.UserName = ValidationHelper.GetString(((Label)ri.FindControl("lblUserName")).Text, "");
                        objAllocateProduct.EmailId = ValidationHelper.GetString(((Label)ri.FindControl("lblEmail")).Text, "");
                        objAllocateProduct.Quantity = ValidationHelper.GetInteger(((TextBox)ri.FindControl("txtQuantity")).Text, 0);
                        lstUsers.Add(objAllocateProduct);
                    }
                }
                else
                {
                    int index = lstUsers.FindIndex(item => item.UserId == ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0));
                    if (index != -1)
                    {
                        lstUsers.RemoveAt(index);
                    }
                }
            }
            RepSelectedUser.DataSource = lstUsers;
            RepSelectedUser.DataBind();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Guid imageGuid = default(Guid);
            try
            {
                if (ddlBrand.SelectedIndex > 0 && ddlPosNo.SelectedIndex > 0 && ddlProdCategory.SelectedIndex > 0 && ddlState.SelectedIndex > 0)
                {
                    CampaignProduct product = CampaignProductProvider.GetCampaignProduct(ValidationHelper.GetInteger(ViewState["ProductNodeID"], 0), CurrentDocument.DocumentCulture, CurrentSiteName);
                    if (product != null)
                    {

                        // Sets the properties of the new page
                        product.DocumentCulture = "en-us";
                        product.SetValue("BundleQty", ValidationHelper.GetString(txtBundleQnt.Text, string.Empty));
                        product.SetValue("POSNumber", ValidationHelper.GetInteger(ddlPosNo.SelectedValue, 0));
                        product.SetValue("BrandID", ValidationHelper.GetInteger(ddlBrand.SelectedValue, 0));
                        product.SetValue("EstimatedPrice", ValidationHelper.GetString(txtEstPrice.Text, string.Empty));
                        product.SetValue("ActualPrice", ValidationHelper.GetString(txtActualPrice.Text, string.Empty));
                        product.SetValue("ShortDescription", ValidationHelper.GetString(txtShortDes.Text, string.Empty));
                        product.SetValue("State", ValidationHelper.GetString(ddlState.SelectedItem.Text, string.Empty));
                        product.SetValue("ExpirationDate", ValidationHelper.GetString(txtExpDate.Text, string.Empty));
                        product.SetValue("CategoryID", ValidationHelper.GetInteger(ddlProdCategory.SelectedValue, 0));
                        product.SetValue("Cancelled", ValidationHelper.GetBoolean(chkcancel.Checked, false));
                        product.SetValue("CVOProductID", ValidationHelper.GetString(txtCVOProductId.Text, ""));
                        product.SetValue("TotalQty", ValidationHelper.GetString(txtQuantity.Text, ""));
                        product.SetValue("Status", ValidationHelper.GetBoolean(ddlStatus.SelectedValue, false));
                        if (productImage.HasFile)
                        {
                            imageGuid = UploadImage.UploadImageToMeadiaLibrary(productImage);
                            product.SetValue("Image", ValidationHelper.GetString(imageGuid, ""));
                        }
                        // Inserts the new page as a child of the parent page
                        product.Update();
                        var productID = ValidationHelper.GetInteger(product.GetValue("CampaignProductID"), 0);
                        UpdateAllocateProduct(productID);
                        lblSuccessMsg.Visible = true;
                        lblFailureText.Visible = false;
                        EmptyFields();
                    }
                    else
                    {
                        lblFailureText.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("SaveProductFromButtonClick", "EXCEPTION", ex);
                if (imageGuid != default(Guid))
                    UploadImage.DeleteImage(imageGuid);
                EventLogProvider.LogInformation("CMSWebParts_Kadena_Add_inventory_Products", "btnSave_Click", ex.Message);
            }
        }
        private void AllocateProductToUsers(int productID)
        {
            string customTableClassName = "KDA.UserAllocatedProducts";
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if (customTable != null)
            {
                // Creates a new custom table item
                CustomTableItem newCustomTableItem = CustomTableItem.New(customTableClassName);

                // Sets the values for the fields of the custom table (ItemText in this case)
                foreach (AllocateProduct User in lstUsers)
                {
                    newCustomTableItem.SetValue("UserID", User.UserId);
                    newCustomTableItem.SetValue("ProductID", productID);
                    newCustomTableItem.SetValue("Quantity", User.Quantity);
                    newCustomTableItem.SetValue("EmailID", User.EmailId);
                    // Save the new custom table record into the database
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

                    // Sets the values for the fields of the custom table (ItemText in this case)
                    // Loops through individual custom table records
                    foreach (CustomTableItem customitem in customTableData)
                    {
                        int index = lstUsers.FindIndex(item => item.UserId == ValidationHelper.GetInteger(customitem.GetValue("UserID"), 0));
                        if(index >-1)              //If Item is selected
                        {
                            customitem.SetValue("Quantity", lstUsers[index].Quantity);
                            customitem.Update();
                        }
                        else    //If item is not selected
                        {
                            customitem.Delete();
                        }
                        foreach (AllocateProduct User in lstUsers)
                        {
                            if (customitem.GetValue("UserID").ToString() != User.UserId.ToString())
                            {
                                customitem.SetValue("UserID", User.UserId);
                                customitem.SetValue("ProductID", productID);
                                customitem.SetValue("Quantity", User.Quantity);
                                customitem.SetValue("EmailID", User.EmailId);
                                // Save the new custom table record into the database
                                customitem.Insert();
                            }
                               
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Product allocation update", "EXCEPTION", ex);
            }

        }
        private void SetFeild(int productid)
        {
            Guid imageGuid = default(Guid);
            try
            {
                CampaignProduct product = CampaignProductProvider.GetCampaignProduct(ValidationHelper.GetInteger(productid, 0), CurrentDocument.DocumentCulture, CurrentSiteName);
                if (product != null)
                {
                    // Sets the properties of the new page
                    product.DocumentCulture = "en-us";
                    txtBundleQnt.Text = product.GetValue("BundleQty", string.Empty);
                    ddlPosNo.SelectedValue = product.GetValue("POSNumber", string.Empty);
                    ddlBrand.SelectedValue = product.GetValue("BrandID", string.Empty);
                    txtEstPrice.Text = product.GetValue("EstimatedPrice", string.Empty);
                    txtEstPrice.Text = product.GetValue("EstimatedPrice", string.Empty);
                    txtActualPrice.Text = product.GetValue("ActualPrice", string.Empty);
                    txtShortDes.Text = product.GetValue("ShortDescription", string.Empty);
                    ddlState.SelectedItem.Text = product.GetValue("State", string.Empty);
                    txtExpDate.Text = product.GetValue("ExpirationDate", string.Empty);
                    ddlProdCategory.SelectedValue = product.GetValue("CategoryID", string.Empty);
                    chkcancel.Checked = product.GetBooleanValue("Cancelled", false);
                    txtCVOProductId.Text = product.GetValue("CVOProductID", string.Empty);
                    txtQuantity.Text = product.GetValue("TotalQty", string.Empty);
                    ddlStatus.SelectedValue = product.GetValue("Status", string.Empty);

                    if (product.Image != default(Guid) && !product.Image.Equals(Guid.Empty))
                    {
                        MediaFileInfo image = MediaFileInfoProvider.GetMediaFileInfo(product.Image, SiteContext.CurrentSiteName);
                        if (image != null)
                        {
                            imgProduct.ImageUrl = MediaFileURLProvider.GetMediaFileAbsoluteUrl(product.Image, image.FileName);
                            imgProduct.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("GetProductFromButtonClick", "EXCEPTION", ex);
            }
        }
        private void EmptyFields()
        {
            ddlBrand.SelectedIndex = 0;
            ddlPosNo.SelectedIndex = 0;
            ddlProdCategory.SelectedIndex = 0;
            ddlState.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            txtActualPrice.Text = "";
            txtBundleQnt.Text = "";
            txtCVOProductId.Text = "";
            txtEstPrice.Text = "";
            txtExpDate.Text = "";
            txtLongDes.Text = "";
            txtQuantity.Text = "";
            txtShortDes.Text = "";
            txtStroeFrontId.Text = "";
            RepSelectedUser.DataSource = "";
            RepSelectedUser.DataBind();
            lstUsers = new List<Product.AllocateProduct>();
        }
        private void BindUsers(int pageIndex)
        {
            // var data = 
            RepterDetails.DataSource = UserInfoProvider.GetUsers().Skip(PageSize * (pageIndex - 1)).Take(PageSize);
            RepterDetails.DataBind();
            PopulatePager(UserInfoProvider.GetUsers().Count(), 1);
        }
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
        protected void Page_Changed(object sender, EventArgs e)
        {
            foreach (RepeaterItem ri in RepterDetails.Items)
            {
                CheckBox item_check = (CheckBox)ri.FindControl("chkAllocate");
                if (item_check.Checked)
                {
                    int index = lstUsers.FindIndex(item => item.UserId == ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0));
                    if (index == -1)
                    {
                        AllocateProduct objAllocateProduct = new AllocateProduct();
                        objAllocateProduct.UserId = ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0);
                        objAllocateProduct.UserName = ValidationHelper.GetString(((Label)ri.FindControl("lblUserName")).Text, "");
                        objAllocateProduct.EmailId = ValidationHelper.GetString(((Label)ri.FindControl("lblEmail")).Text, "");
                        objAllocateProduct.Quantity = ValidationHelper.GetInteger(((TextBox)ri.FindControl("txtQuantity")).Text, 0);
                        lstUsers.Add(objAllocateProduct);
                    }
                }
                else
                {
                    int index = lstUsers.FindIndex(item => item.UserId == ValidationHelper.GetInteger(((Label)ri.FindControl("lblUserid")).Text, 0));
                    if (index != -1)
                    {
                        lstUsers.RemoveAt(index);
                    }
                }
                int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
                this.BindUsers(pageIndex);

            }
        }
    }
    public class ProductCategory
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
    public class AllocateProduct
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public int Quantity { get; set; }
    }
}