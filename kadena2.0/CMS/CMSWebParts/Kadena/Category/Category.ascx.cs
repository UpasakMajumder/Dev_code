using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.EventLog;
using Kadena.Old_App_Code.Kadena.Constants;
using CMS.DocumentEngine.Types.KDA;
using System.Linq;

public partial class CMSWebParts_Kadena_Category : CMSAbstractWebPart
{
    #region "Variables"

    private string folderpath = "/";
    private int categoyId = 0;
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
            if (Request.QueryString["ID"] != null)
            {

                btnSave.Click += btnSave_Edit;
                categoyId = ValidationHelper.GetInteger(Request.QueryString["ID"], 0);
                SetFeild(categoyId);
            }
            else
            {
                btnSave.Click += btnSave_Save;
            }
            BindStatus();
            txtName.Attributes.Add("PlaceHolder", ResHelper.GetString("Kadena.Categoryform.NameWatermark"));
            txtDescription.Attributes.Add("PlaceHolder", ResHelper.GetString("Kadena.Categoryform.DesWatermark"));
            rfvUserNameRequired.ErrorMessage = ResHelper.GetString("Kadena.Categoryform.NameValidation");
            revDescription.ErrorMessage = ResHelper.GetString("Kadena.Categoryform.DesValidation");
            revName.ErrorMessage = ResHelper.GetString("Kadena.Categoryform.NameRangeValidation");
            cvCatName.ErrorMessage = ResHelper.GetString("Kadena.Categoryform.NameUniqueValidation");
            folderpath = SettingsKeyInfoProvider.GetValue("KDA_CategoryFolderPath", CurrentSiteName);
        }
    }
    /// <summary>
    /// Click Event which will save a category
    /// // Creates a new page of the "CMS.MenuItem" page type
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Save(object sender, EventArgs e)
    {
        string categoryName = txtName.Text;
        string categroyDes = txtDescription.Text;
        try
        {
            if (!string.IsNullOrEmpty(categoryName) && Page.IsValid)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                CMS.DocumentEngine.TreeNode parentPage = tree.SelectNodes().Path(folderpath).OnCurrentSite().Culture(DocumentContext.CurrentDocument.DocumentCulture).FirstObject;
                if (parentPage != null)
                {
                    CMS.DocumentEngine.TreeNode newPage = CMS.DocumentEngine.TreeNode.New("KDA.ProductCategory", tree);
                    newPage.DocumentName = categoryName;
                    newPage.DocumentCulture = DocumentContext.CurrentDocument.DocumentCulture;
                    newPage.SetValue("ProductCategoryTitle", categoryName);
                    newPage.SetValue("ProductCategoryDescription", categroyDes);
                    newPage.SetValue("Status", ValidationHelper.GetBoolean(ddlStatus.SelectedValue, false));
                    newPage.Insert(parentPage);
                    lblSuccessMsg.Visible = true;
                    lblFailureText.Visible = false;
                    Response.Cookies["status"].Value = QueryStringStatus.Added;
                    Response.Cookies["status"].HttpOnly = false;
                    URLHelper.Redirect($"{CurrentDocument.Parent.DocumentUrlPath}?status={QueryStringStatus.Added}");
                }
                else
                {
                    lblFailureText.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CategroyCreateForm", "EXCEPTION", ex);
        }
    }
    protected void btnCancel_Cancel(object sender, EventArgs e)
    {
        URLHelper.Redirect(CurrentDocument.Parent.DocumentUrlPath);
    }
    /// <summary>
    ///   update the  campaign
    ///   Sets the properties of the new page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Edit(object sender, EventArgs e)
    {
        string categoryName = txtName.Text;
        string categroyDes = txtDescription.Text;
        try
        {
            if (!string.IsNullOrEmpty(categoryName) && Page.IsValid)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                CMS.DocumentEngine.TreeNode editPage = tree.SelectNodes("KDA.ProductCategory").OnCurrentSite().Where("ProductCategoryID", QueryOperator.Equals, categoyId);
                if (editPage != null)
                {
                    editPage.DocumentName = categoryName;
                    editPage.DocumentCulture = DocumentContext.CurrentDocument.DocumentCulture;
                    editPage.SetValue("ProductCategoryTitle", categoryName);
                    editPage.SetValue("ProductCategoryDescription", categroyDes);
                    editPage.SetValue("Status", ValidationHelper.GetBoolean(ddlStatus.SelectedValue, false));
                    editPage.Update();
                    Response.Cookies["status"].Value = QueryStringStatus.Updated;
                    Response.Cookies["status"].HttpOnly = false;
                    URLHelper.Redirect($"{CurrentDocument.Parent.DocumentUrlPath}?status={QueryStringStatus.Updated}");
                }
                else
                {
                    lblFailureText.Visible = true;
                }

            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CategroyCreateFormEdit", "EXCEPTION", ex);
        }
    }
    /// <summary>
    /// Dropdown for Status
    /// </summary>
    public void BindStatus()
    {
        ddlStatus.Items.Clear();
        ddlStatus.Items.Insert(0, new ListItem(ResHelper.GetString("KDA.Common.Status.Active"), "1"));
        ddlStatus.Items.Insert(1, new ListItem(ResHelper.GetString("KDA.Common.Status.Inactive"), "0"));
    }

    /// <summary>
    /// Method to fetch the record which user wants to edit and populates the fields with those values 
    /// </summary>
    /// <param name="_categoryId">category id of editing record</param>

    private void SetFeild(int _categoryId)
    {
        try
        {
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            CMS.DocumentEngine.TreeNode editPage = tree.SelectNodes("KDA.ProductCategory").OnCurrentSite().Where("ProductCategoryID", QueryOperator.Equals, _categoryId);
            if (editPage != null)
            {
                // get the properties of the page

                txtName.Text = editPage.GetValue("ProductCategoryTitle").ToString();
                txtDescription.Text = editPage.GetValue("ProductCategoryDescription").ToString();

            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CampaignCreateFormEdit", "EXCEPTION", ex);
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
    /// Validates the category name is unique or not
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void CatName_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
    {
        try
        {

            if (Request.QueryString["ID"] != null)
            {
                var catId = Request.QueryString["ID"];
                var categoryData = ProductCategoryProvider.GetProductCategories()
                    .WhereEquals("ProductCategoryTitle", txtName.Text.Trim())
                    .And()
                    .WhereNotEquals("ProductCategoryID", catId)
                    .Columns("ProductCategoryTitle")
                    .FirstOrDefault();
                args.IsValid = DataHelper.DataSourceIsEmpty(categoryData);
            }
            else
            {
                var categoryData = ProductCategoryProvider.GetProductCategories()
                                                    .WhereEquals("ProductCategoryTitle", txtName.Text.Trim())
                                                    .Columns("ProductCategoryTitle")
                                                    .FirstOrDefault();
                args.IsValid = DataHelper.DataSourceIsEmpty(categoryData);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Category.ascx.cs", "CatName_ServerValidate()", ex);
        }
    }

    #endregion
}



