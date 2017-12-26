using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Linq;

public partial class CMSWebParts_Kadena_Brand_Brand : CMSAbstractWebPart
{
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
        if (!this.StopProcessing)
        {
            BrandItem brandData = null;
            brandData = new BrandItem();
            form.AlternativeFormFullName = "KDA.Brand.KDA_Brand";
            form.Info = brandData;
            form.VisibilityFormName = "KDA.Brand.KDA_Brand";
            form.SubmitButton.Visible = false;
            btnCancel.Text = ResHelper.GetString("Kadena.CreateBrand.Cancel");
            form.ValidationErrorMessage = ResHelper.GetString("Kadena.Brands.FormError");
            if (QueryHelper.GetInteger("ItemID", 0) > 0)
            {
                brandData = CustomTableItemProvider.GetItem<BrandItem>(QueryHelper.GetInteger("ItemID", 0));
                form.Mode = CMS.Base.Web.UI.FormModeEnum.Update;
                form.Info = brandData;
                btnSave.Text = ResHelper.GetString("Kadena.CreateBrand.Update");
            }
            else
            {
                form.Mode = CMS.Base.Web.UI.FormModeEnum.Insert;
                form.Info = brandData;
                btnSave.Text = ResHelper.GetString("Kadena.CreateBrand.Save");
            }
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

    #endregion "Methods"

    /// <summary>
    /// Saves and updates the brand details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (form.ValidateData())
        {
            if (QueryHelper.GetInteger("ItemID", 0) > 0)
            {
                var brandData = CustomTableItemProvider.GetItem<BrandItem>(QueryHelper.GetInteger("ItemID", 0));
                brandData.BrandCode = ValidationHelper.GetInteger(form.GetFieldValue("BrandCode"), 0);
                form.SaveData(CurrentDocument.Parent.AbsoluteURL);
            }
            else
            {
                var brandData = new BrandItem();
                brandData.BrandCode = ValidationHelper.GetInteger(form.GetFieldValue("BrandCode"), 0);
                brandData.BrandName = ValidationHelper.GetString(form.GetFieldValue("BrandName"), string.Empty);
                brandData.BrandDescription = ValidationHelper.GetString(form.GetFieldValue("BrandDescription"), string.Empty);
                brandData.Status= ValidationHelper.GetBoolean(form.GetFieldValue("Status"), true);
                form.SaveData(CurrentDocument.Parent.AbsoluteURL);
            }
        }
        else
        {
            form.ShowValidationErrorMessage = true;
        }
    }

    /// <summary>
    /// Redirects to parent page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(CurrentDocument.Parent.AbsoluteURL);
    }

    /// <summary>
    /// Checks whether the given brand already exists
    /// </summary>
    /// <param name="BrandCode">brand code</param>
    /// <param name="ItemID">itemid</param>
    /// <returns>flase if already exist else true</returns>
    private bool IsBrandCodeValid(int BrandCode, int ItemID)
    {
        var flag = false;
        if (ItemID > 0)
        {
            var brandData = CustomTableItemProvider.GetItems<BrandItem>().
                WhereEquals("BrandCode", BrandCode).
                And().
                WhereNotEquals("ItemID", ItemID).
                Columns("BrandCode").
                FirstOrDefault();
            return flag = brandData != null ? false : true;
        }
        else
        {
            var brandData = CustomTableItemProvider.GetItems<BrandItem>().
                WhereEquals("BrandCode", BrandCode).
                Columns("BrandCode").
                FirstOrDefault();
            return flag = brandData != null ? false : true;
        }
    }
}