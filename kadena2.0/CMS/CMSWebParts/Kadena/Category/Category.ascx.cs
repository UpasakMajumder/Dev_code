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

            txtName.Attributes.Add("PlaceHolder", ResHelper.GetString("Kadena.Categoryform.NameWatermark"));
            txtDescription.Attributes.Add("PlaceHolder", ResHelper.GetString("Kadena.Categoryform.DesWatermark"));
            rfvUserNameRequired.ErrorMessage = ResHelper.GetString("Kadena.Categoryform.NameValidation");
            revDescription.ErrorMessage = ResHelper.GetString("Kadena.Categoryform.DesValidation");
            revName.ErrorMessage = ResHelper.GetString("Kadena.Categoryform.NameRangeValidation");
            folderpath = SettingsKeyInfoProvider.GetValue("KDA_CategoryFolderPath",CurrentSiteName);
        }
    }
    /// <summary>
    /// Click Event which will save a category
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Save(object sender, EventArgs e)
    {
        string categoryName = txtName.Text;
        string categroyDes = txtDescription.Text;
        try
        {
            if (!string.IsNullOrEmpty(categoryName))
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                CMS.DocumentEngine.TreeNode parentPage = tree.SelectNodes().Path(folderpath).OnCurrentSite().Culture(DocumentContext.CurrentDocument.DocumentCulture).FirstObject;
                if (parentPage != null)
                {
                    // Creates a new page of the "CMS.MenuItem" page type
                    CMS.DocumentEngine.TreeNode newPage = CMS.DocumentEngine.TreeNode.New("KDA.ProductCategory", tree);

                    // Sets the properties of the new page
                    newPage.DocumentName = categoryName;
                    newPage.DocumentCulture = DocumentContext.CurrentDocument.DocumentCulture;
                    newPage.SetValue("ProductCategoryTitle", categoryName);
                    newPage.SetValue("ProductCategoryDescription", categroyDes);

                    // Inserts the new page as a child of the parent page
                    newPage.Insert(parentPage);
                    lblSuccessMsg.Visible = true;
                    lblFailureText.Visible = false;
                    txtName.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    Response.Redirect(CurrentDocument.Parent.DocumentUrlPath, false);
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
    protected void btnSave_Edit(object sender, EventArgs e)
    {
        string categoryName = txtName.Text;
        string categroyDes = txtDescription.Text;
        try
        {
            if (!string.IsNullOrEmpty(categoryName))
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                CMS.DocumentEngine.TreeNode editPage = tree.SelectNodes("KDA.ProductCategory").OnCurrentSite().Where("ProductCategoryID", QueryOperator.Equals, categoyId);
                if (editPage != null)
                {
                    // Sets the properties of the new page
                    editPage.DocumentName = categoryName;
                    editPage.DocumentCulture = DocumentContext.CurrentDocument.DocumentCulture;
                    editPage.SetValue("ProductCategoryTitle", categoryName);
                    editPage.SetValue("ProductCategoryDescription", categroyDes);

                    // update the  campaign
                    editPage.Update();

                    URLHelper.Redirect(CurrentDocument.Parent.DocumentUrlPath);
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

    #endregion
}



