using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.DocumentEngine.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.EmailNotifications;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.Container.Default;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public partial class CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter : CMSAbstractBaseFilterControl
{
    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    private void SetupControl()
    {
        if (this.StopProcessing)
        {
            this.Visible = false;
        }
        else if (!RequestHelper.IsPostBack())
        {
            BindPrograms();
            BindCategories();
            BindButtons();
            BindReourceStrings();
        }
    }

    /// <summary>
    /// Binding the Resource string text to buttons
    /// </summary>
    public void BindReourceStrings()
    {
        btnNewProduct.Text = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.NewProductsText"), string.Empty);
        btnAllowUpates.Text = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.AllowProductsUpdateText"), string.Empty);
        btnNotifyAdmin.Text = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.NotifyGlobalAdminText"), string.Empty);
    }

    /// <summary>
    /// Generates a WHERE condition and ORDER BY clause based on the current filtering selection.
    /// </summary>
    private void SetFilter()
    {
        try
        {
            string where = null;
            if (!string.IsNullOrEmpty(txtSearchProducts.Text))
            {
                where += $"(p.ProductName like '%{ SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(txtSearchProducts.Text))}%' OR SKUProductCustomerReferenceNumber like '%{ SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(txtSearchProducts.Text))}%')";
            }
            if (ValidationHelper.GetInteger(ddlPrograms.SelectedValue, 0) != 0)
            {
                int programID = ValidationHelper.GetInteger(ddlPrograms.SelectedValue, 0);
                where += where != null ? $"and p.ProgramID={ programID}" : $"p.ProgramID={ programID}";
            }
            if (ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, 0) != 0)
            {
                int categoryID = ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, 0);
                where += where != null ? $"and p.CategoryID={ categoryID}" : $"p.CategoryID={ categoryID}";
            }
            if (where != null)
            {
                this.WhereCondition = where;
            }
            this.RaiseOnFilterChanged();
            BindButtons();
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "SetFilter", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Binding the busttons based on roles
    /// </summary>
    public void BindButtons()
    {
        try
        {
            var nodeGuid = CurrentDocument.NodeGUID;
            Campaign campaign = CampaignProvider.GetCampaign(nodeGuid, CurrentDocument.DocumentCulture, CurrentSite.SiteName);
            if (campaign != null)
            {
                var products = campaign.AllChildren.WithAllData
                    .Where(xx => xx.ClassName == CampaignsProduct.CLASS_NAME && xx.NodeSiteID == CurrentSite.SiteID)
                    .ToList();
                bool initiated = campaign.CampaignInitiate;
                bool openCampaign = campaign.OpenCampaign;
                bool closeCampaign = campaign.CloseCampaign;
                string gAdminRoleName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_GlobalAminRoleName");
                string adminRoleName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_AdminRoleName");
                Program program = ProgramProvider.GetPrograms()
                    .WhereEquals("ProgramId", ddlPrograms.SelectedValue)
                    .FirstOrDefault();
                var gAdminNotified = program != null ? program.GlobalAdminNotified : false;
                bool gAdminNotifiedAll = IsCampaignNotified();
                var productsExist = ProgramHasProducts(CurrentDocument.NodeID);
                if (CurrentUser.IsInRole(gAdminRoleName, SiteContext.CurrentSiteName))
                {
                    BindActionsForGlobalAdmin(initiated, openCampaign, closeCampaign, gAdminNotified, gAdminNotifiedAll, productsExist, program);
                }
                else if (CurrentUser.IsInRole(adminRoleName, SiteContext.CurrentSiteName))
                {
                    BindActionsForAdmin(initiated, openCampaign, closeCampaign, gAdminNotified, gAdminNotifiedAll, productsExist, program);
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "BindButtons", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Binds actions for global admin
    /// </summary>
    /// <param name="initiated"></param>
    /// <param name="openCampaign"></param>
    /// <param name="closeCampaign"></param>
    /// <param name="gAdminNotified"></param>
    /// <param name="gAdminNotifiedAll"></param>
    /// <param name="productsExist"></param>
    /// <param name="program"></param>
    public void BindActionsForGlobalAdmin(bool initiated, bool openCampaign, bool closeCampaign, bool gAdminNotified, bool gAdminNotifiedAll, bool productsExist, Program program)
    {
        try
        {
            if (!initiated)
            {
                btnAllowUpates.Visible = true;
                btnAllowUpates.Enabled = false;
                btnAllowUpates.CssClass = "disable btn-action";
                btnNewProduct.Visible = true;
                btnNewProduct.Enabled = true;
            }
            else if (initiated && !openCampaign)
            {
                GAdminActionsAfterCampaignInitiation(program, gAdminNotified);
            }
            else if (openCampaign && !closeCampaign)
            {
                GAdminActionsAfterCampaignOpen(program, gAdminNotified, gAdminNotifiedAll);
            }
            else if (closeCampaign)
            {
                btnAllowUpates.Visible = true;
                btnAllowUpates.Enabled = false;
                btnAllowUpates.CssClass = "disable btn-action";
                btnNewProduct.Visible = true;
                btnNewProduct.Enabled = false;
                btnNewProduct.CssClass = "disable btn-action";
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "BindActionsForGlobalAdmin", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Binds actions for global admin after initiating campaign
    /// </summary>
    /// <param name="program"></param>
    /// <param name="gAdminNotified"></param>
    public void GAdminActionsAfterCampaignInitiation(Program program, bool gAdminNotified)
    {
        try
        {
            if (program != null)
            {
                if (gAdminNotified)
                {
                    btnAllowUpates.Visible = true;
                    btnAllowUpates.Enabled = true;
                }
                else
                {
                    btnAllowUpates.Visible = true;
                    btnAllowUpates.Enabled = false;
                    btnAllowUpates.CssClass = "disable btn-action";
                }
            }
            else
            {
                btnAllowUpates.Visible = true;
                btnAllowUpates.Enabled = false;
                btnAllowUpates.CssClass = "disable btn-action";
                btnNewProduct.Visible = true;
                btnNewProduct.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "GAdminActionsAfterCampaignInitiation", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Binds actions for global admin after opening campaign
    /// </summary>
    /// <param name="program"></param>
    /// <param name="gAdminNotified"></param>
    /// <param name="gAdminNotifiedAll"></param>

    public void GAdminActionsAfterCampaignOpen(Program program, bool gAdminNotified, bool gAdminNotifiedAll)
    {
        try
        {
            if (program != null && gAdminNotified)
            {
                btnAllowUpates.Visible = true;
                btnAllowUpates.Enabled = true;
                btnNewProduct.Visible = true;
                btnNewProduct.Enabled = false;
                btnNewProduct.CssClass = "disable btn-action";
            }
            else if (program != null && !gAdminNotified)
            {
                btnAllowUpates.Visible = true;
                btnAllowUpates.Enabled = false;
                btnAllowUpates.CssClass = "disable btn-action";
                btnNewProduct.Visible = true;
                btnNewProduct.Enabled = true;
            }
            else
            {
                btnAllowUpates.Visible = true;
                btnAllowUpates.Enabled = false;
                btnAllowUpates.CssClass = "disable btn-action";
                if (gAdminNotifiedAll)
                {
                    btnNewProduct.Visible = true;
                    btnNewProduct.Enabled = false;
                    btnNewProduct.CssClass = "disable btn-action";
                }
                else
                {
                    btnNewProduct.Visible = true;
                    btnNewProduct.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "GAdminActionsAfterCampaignOpen", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Binds actions for admin
    /// </summary>
    /// <param name="initiated"></param>
    /// <param name="openCampaign"></param>
    /// <param name="closeCampaign"></param>
    /// <param name="gAdminNotified"></param>
    /// <param name="gAdminNotifiedAll"></param>
    /// <param name="productsExist"></param>
    /// <param name="program"></param>
    public void BindActionsForAdmin(bool initiated, bool openCampaign, bool closeCampaign, bool gAdminNotified, bool gAdminNotifiedAll, bool productsExist, Program program)
    {
        try
        {
            if (!initiated)
            {
                btnNotifyAdmin.Visible = true;
                btnNotifyAdmin.Enabled = false; ;
                btnNotifyAdmin.CssClass = "disable btn-action";
                btnNewProduct.Visible = true;
                btnNewProduct.Enabled = false;
                btnNewProduct.CssClass = "disable btn-action";
            }
            else if (initiated && !openCampaign)
            {
                AdminActionsAfterCampaignInitiation(program, productsExist, gAdminNotified);
            }
            else if (openCampaign && !closeCampaign)
            {
                AdminActionsAfterCampaignOpen(program, productsExist, gAdminNotified, gAdminNotifiedAll);
            }
            else if (closeCampaign)
            {
                btnNotifyAdmin.Visible = true;
                btnNotifyAdmin.Enabled = false;
                btnNotifyAdmin.CssClass = "disable btn-action";
                btnNewProduct.Visible = true;
                btnNewProduct.Enabled = false;
                btnNewProduct.CssClass = "disable btn-action";
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "BindActionsForAdmin", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Binds actions for admin after campaign initiation
    /// </summary>
    /// <param name="program"></param>
    /// <param name="productsExist"></param>
    /// <param name="gAdminNotified"></param>
    public void AdminActionsAfterCampaignInitiation(Program program, bool productsExist, bool gAdminNotified)
    {
        try
        {
            if (program != null && productsExist)
            {
                if (gAdminNotified)
                {
                    btnNotifyAdmin.Visible = true;
                    btnNotifyAdmin.Enabled = false; ;
                    btnNotifyAdmin.CssClass = "disable btn-action";
                    btnNewProduct.Visible = true;
                    btnNewProduct.Enabled = false;
                    btnNewProduct.CssClass = "disable btn-action";
                }
                else
                {
                    btnNotifyAdmin.Visible = true;
                    btnNotifyAdmin.Enabled = true; ;
                    btnNewProduct.Visible = true;
                    btnNewProduct.Enabled = true;
                }
            }
            else
            {
                btnNotifyAdmin.Visible = true;
                btnNotifyAdmin.Enabled = false;
                btnNotifyAdmin.CssClass = "disable btn-action";
                btnNewProduct.Visible = true;
                btnNewProduct.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "AdminActionsAfterCampaignInitiation", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Binds actions for admin after opening campaign
    /// </summary>
    /// <param name="program"></param>
    /// <param name="productsExist"></param>
    /// <param name="gAdminNotified"></param>
    /// <param name="gAdminNotifiedAll"></param>
    public void AdminActionsAfterCampaignOpen(Program program, bool productsExist, bool gAdminNotified, bool gAdminNotifiedAll)
    {
        try
        {
            if (program != null)
            {
                AdminActionsForProgram(productsExist, gAdminNotified);
            }
            else
            {
                btnNotifyAdmin.Visible = true;
                btnNotifyAdmin.Enabled = false;
                btnNotifyAdmin.CssClass = "disable btn-action";
                if (gAdminNotifiedAll)
                {
                    btnNewProduct.Visible = true;
                    btnNewProduct.Enabled = false;
                    btnNewProduct.CssClass = "disable btn-action";
                }
                else
                {
                    btnNewProduct.Visible = true;
                    btnNewProduct.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "AdminActionsAfterCampaignOpen", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Binds actions for admin
    /// </summary>
    /// <param name="productsExist"></param>
    /// <param name="gAdminNotified"></param>
    public void AdminActionsForProgram(bool productsExist, bool gAdminNotified)
    {
        try
        {
            if (productsExist)
            {
                if (gAdminNotified)
                {
                    btnNotifyAdmin.Visible = true;
                    btnNotifyAdmin.Enabled = false;
                    btnNotifyAdmin.CssClass = "disable btn-action";
                    btnNewProduct.Visible = true;
                    btnNewProduct.Enabled = false;
                    btnNewProduct.CssClass = "disable btn-action";
                }
                else
                {
                    btnNotifyAdmin.Visible = true;
                    btnNotifyAdmin.Enabled = true;
                    btnNewProduct.Visible = true;
                    btnNewProduct.Enabled = true;
                }
            }
            else
            {
                btnNotifyAdmin.Visible = true;
                btnNotifyAdmin.Enabled = false;
                btnNotifyAdmin.CssClass = "disable btn-action";
                btnNewProduct.Visible = true;
                btnNewProduct.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "AdminActionsForProgram", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Init event handler.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        SetupControl();
        base.OnInit(e);
    }

    /// <summary>
    /// PreRender event handler
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        if (RequestHelper.IsPostBack())
        {
            SetFilter();
        }
        base.OnPreRender(e);
    }

    /// <summary>
    /// search data by given text
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtSearchProducts_TextChanged(object sender, EventArgs e)
    {
        SetFilter();
    }

    /// <summary>
    /// Bind the prograns to dropdown
    /// </summary>
    public void BindPrograms()
    {
        try
        {
            int campaignID = CurrentDocument.GetIntegerValue("CampaignID", default(int));
            if (campaignID != default(int))
            {
                var programs = ProgramProvider.GetPrograms()
                    .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                    .WhereEquals("CampaignID", campaignID)
                    .Columns("ProgramID,ProgramName")
                    .ToList();
                if (programs != null)
                {
                    ddlPrograms.DataSource = programs;
                    ddlPrograms.DataTextField = "ProgramName";
                    ddlPrograms.DataValueField = "ProgramID";
                    ddlPrograms.DataBind();
                    string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectProgramText"), string.Empty);
                    ddlPrograms.Items.Insert(0, new ListItem(selectText, "0"));
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "BindPrograms", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Bind Categories to dropdown
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
            if (categories != null)
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
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "BindCategories", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// CHecks whether program has products or not
    /// </summary>
    public bool ProgramHasProducts(int campaignNodeId)
    {
        try
        {
            var query = new DataQuery(SQLQueries.getPrebuyProductCount);
            QueryDataParameters queryParams = new QueryDataParameters();
            queryParams.Add("@CampaignNodeId", campaignNodeId);
            queryParams.Add("@SiteId", CurrentSite.SiteID);
            GeneralConnection cn = ConnectionHelper.GetConnection();
            string where = null;
            if (!string.IsNullOrEmpty(txtSearchProducts.Text))
            {
                where += $"and (p.ProductName like '%{ SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(txtSearchProducts.Text))}%' OR SKUProductCustomerReferenceNumber like '%{ SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(txtSearchProducts.Text))}%')";
            }
            if (ValidationHelper.GetInteger(ddlPrograms.SelectedValue, 0) != 0)
            {
                int programID = ValidationHelper.GetInteger(ddlPrograms.SelectedValue, 0);
                where += $"and p.ProgramID={ programID}";
            }
            if (ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, 0) != 0)
            {
                int categoryID = ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, 0);
                where += $"and p.CategoryID={ categoryID}";
            }
            if (where != null)
            {
                this.WhereCondition = where;
            }
            QueryParameters qp = new QueryParameters(query.QueryText + where, queryParams, QueryTypeEnum.SQLQuery, false);
            var productData = cn.ExecuteQuery(qp);
            var productCount = productData != null ? ValidationHelper.GetInteger(productData.Tables[0].Rows[0][0], default(int)) : default(int);
            return productCount > 0 ? true : false;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "ProgramHasProducts", ex, CurrentSite.SiteID, ex.Message);
        }
        return false;
    }

    /// <summary>
    /// Checks all the programs are notified
    /// </summary>
    /// <returns></returns>
    public bool IsCampaignNotified()
    {
        try
        {
            var programs = DocumentHelper.GetDocuments(Program.CLASS_NAME)
                .Path(CurrentDocument.NodeAliasPath, PathTypeEnum.Children)
                .WhereEquals("NodeSiteId", CurrentSite.SiteID)
                .WhereEqualsOrNull("GlobalAdminNotified", false)
                .Columns("GlobalAdminNotified").Any();
            return !programs;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions", "AllProgramsNotified", ex, CurrentSite.SiteID, ex.Message);
        }
        return false;
    }

    /// <summary>
    /// filter data by category id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProductcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFilter();
    }

    /// <summary>
    /// filter data by brogram id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPrograms_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFilter();
    }

    /// <summary>
    /// Nottify Admin
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNotifyAdmin_Click(object sender, EventArgs e)
    {
        try
        {
            var nodeGuid = CurrentDocument.NodeGUID;
            var emailTemplate = DIContainer.Resolve<IKenticoResourceService>().GetSettingsKey(SiteContext.CurrentSiteID, "KDA_CampaignProductAddedTemplate");
            Campaign campaign = CampaignProvider.GetCampaign(nodeGuid, CurrentDocument.DocumentCulture, CurrentSite.SiteName);
            var program = ProgramProvider.GetPrograms()
                .WhereEquals("ProgramId", ddlPrograms.SelectedValue)
                .FirstOrDefault();
            if (program != null)
            {
                program.GlobalAdminNotified = true;
                program.Update();
                var roleName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_GlobalAminRoleName");
                var role = RoleInfoProvider.GetRoleInfo(roleName, CurrentSite.SiteID);
                if (role != null)
                {
                    var users = RoleInfoProvider.GetRoleUsers(role.RoleID);
                    if (users != null)
                    {
                        foreach (var user in users.AsEnumerable().ToList())
                        {
                            ProductEmailNotifications.CampaignEmail(campaign.DocumentName, user.Field<string>("Email"), emailTemplate, program.DocumentName);
                        }
                    }
                }
                Response.Redirect(CurrentDocument.DocumentUrlPath,false);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "btnNotifyAdmin_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Allow the Products updates
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAllowUpates_Click(object sender, EventArgs e)
    {
        try
        {
            var program = ProgramProvider.GetPrograms()
                .WhereEquals("ProgramId", ddlPrograms.SelectedValue)
                .FirstOrDefault();
            if (program != null)
            {
                program.GlobalAdminNotified = false;
                program.Update();
                Response.Redirect(CurrentDocument.DocumentUrlPath);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "btnAllowUpates_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Adding the new product
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNewProduct_Click(object sender, EventArgs e)
    {
        try
        {
            Guid nodeGUID = ValidationHelper.GetGuid(SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_ProductsPath"), Guid.Empty);
            {
                if (!nodeGUID.Equals(Guid.Empty))
                {
                    var document = new TreeProvider().SelectSingleNode(nodeGUID, CurrentDocument.DocumentCulture, CurrentSite.SiteName);
                    if (document != null)
                    {
                        Response.Redirect($"{document.DocumentUrlPath}?camp={CurrentDocument.NodeID}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "btnNewProduct_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }
}