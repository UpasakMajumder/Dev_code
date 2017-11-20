using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.EventLog;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
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
        private string folderpath = "/";
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
                btnSave.Click += btnSave_SavePOS;
                OKButton.Click += OKButton_Click;
                btnCancel.Click += btnCancel_Cancel;
                rfvBrand.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.BrandRequired");
                rfvActualPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ActualPriceRequired");
                rfvPosNo.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.POSCodeRequired");
                rfvProdCategory.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.POSCategroyRequired");
                rfvExpDate.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ExpiryDateRequired");
                rfvEstPrice.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.EstimatedPriceRequired");
                rfvImage.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ImageRequired");
                rfvLongDes.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.LongDescritpionRequired");
                rfvProdAllocation.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.ProdAllocationRequired");
                rfvShortDes.ErrorMessage= ResHelper.GetString("Kadena.InvProductForm.ShortDescriptionRequired");
                rfvState.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.StateRequired");
                rfvStatus.ErrorMessage = ResHelper.GetString("Kadena.InvProductForm.StatusRequired");
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
            try
            {
                if (ddlBrand.SelectedIndex>0 && ddlPosNo.SelectedIndex > 0 && ddlProdCategory.SelectedIndex > 0 && ddlState.SelectedIndex > 0 && ddlStatus.SelectedIndex>0)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    CMS.DocumentEngine.TreeNode parentPage = tree.SelectNodes().Path(folderpath).OnCurrentSite().Culture("en-us").FirstObject;
                    if (parentPage != null)
                    {
                        // Creates a new page of the "CMS.MenuItem" page type
                        CMS.DocumentEngine.TreeNode newProduct = CMS.DocumentEngine.TreeNode.New("KDA.CampaignProduct", tree);

                        // Sets the properties of the new page
                        newProduct.DocumentCulture = "en-us";
                        newProduct.SetValue("BundleQty", ValidationHelper.GetString(txtBundleQnt.Text,string.Empty));
                        newProduct.SetValue("POSNumber", ValidationHelper.GetInteger(ddlPosNo.SelectedValue, 0));
                        newProduct.SetValue("BrandID", ValidationHelper.GetInteger(ddlBrand.SelectedValue, 0));
                        newProduct.SetValue("EstimatedPrice", ValidationHelper.GetString(txtEstPrice.Text, string.Empty));
                        newProduct.SetValue("ActualPrice", ValidationHelper.GetString(txtActualPrice.Text, string.Empty));
                        newProduct.SetValue("ShortDescription", ValidationHelper.GetString(txtShortDes.Text, string.Empty));
                        newProduct.SetValue("State", ValidationHelper.GetString(ddlState.SelectedItem.Text, string.Empty));
                        newProduct.SetValue("ExpirationDate", ValidationHelper.GetString(txtExpDate, string.Empty));
                        newProduct.SetValue("CategoryID", ValidationHelper.GetInteger(ddlProdCategory.SelectedValue, 0));
                        newProduct.SetValue("Cancelled", ValidationHelper.GetInteger(chkCancel.SelectedValue, 0));
                        newProduct.SetValue("CVOProductID", ValidationHelper.GetString(txtCVOProductId.Text, ""));
                        newProduct.SetValue("TotalQty", ValidationHelper.GetString(txtQuantity.Text, ""));
                        newProduct.SetValue("Status", ValidationHelper.GetString(ddlStatus.SelectedValue, ""));
                        // Inserts the new page as a child of the parent page
                        newProduct.Insert(parentPage);
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
                //lblError.Visible = true;
                //lblSuccess.Visible = false;
            }

        }
        protected void btnCancel_Cancel(object sender, EventArgs e)
        {
            try
            {
                EmptyFields();
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
        protected void OKButton_Click(object sender, EventArgs e)
        {
            // Assigns the value of the UniSelector control to be displayed by the Label
            lblButton.Visible = true;
            List<string> selectedUsers= UniGrid1.SelectedItems;
            lblButton.Text = ValidationHelper.GetString(selectedUsers, null);
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
        }
    }
    public class ProductCategory
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}