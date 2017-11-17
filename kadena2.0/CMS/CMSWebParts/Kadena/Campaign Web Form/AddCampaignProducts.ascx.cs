using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DocumentEngine.Types.KDA;
using System.Collections.Generic;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.EventLog;

public partial class CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts : CMSAbstractWebPart
{
    #region "Properties"

    

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
            BindPrograms();
        }
    }
    public void BindPrograms()
    {
        var programs = ProgramProvider.GetPrograms().Select(x => new Program { ProgramID = x.ProgramID, ProgramName = x.ProgramName }).ToList();
        if(programs != null)
        {
            ddlProgram.DataSource = programs;
            ddlProgram.DataBind();
            ddlProgram.DataTextField = "ProgramName";
            ddlProgram.DataValueField = "ProgramID";
            ddlProgram.DataBind();
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int programID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int));
            if (programID != default(int))
            {
                Program program = new Program();
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                var programDoc = ProgramProvider.GetPrograms().WhereEquals("ProgramID", programID).TopN(1).FirstOrDefault();
                if (programDoc != null)
                {
                    int programNodeID = programDoc.NodeID;
                    var document = TreeHelper.SelectSingleNode(programNodeID);
                    CMS.DocumentEngine.TreeNode createNode = tree.SelectSingleNode(SiteContext.CurrentSiteName, document.NodeAliasPath, CurrentSite.DefaultVisitorCulture);
                    if (createNode != null)
                    {
                        CampaignProducts products = new CampaignProducts();
                        products.DocumentName = ValidationHelper.GetString(txtProductName.Text, "");
                        products.DocumentCulture = SiteContext.CurrentSite.DefaultVisitorCulture;
                        products.ProductName = ValidationHelper.GetString(txtProductName.Text, "");
                        products.State = ValidationHelper.GetString(txtState.Text, "");
                        products.LongDescription = ValidationHelper.GetString(txtLongDescription.Text, "");
                        // products.ExpirationDate=ValidationHelper.GetString()
                        products.BrandName = ValidationHelper.GetString(txtBrand.Text, "");
                        //products.ProductCategory = ValidationHelper.GetString();
                        products.QtyPerPack = ValidationHelper.GetString(txtQty.Text, "");
                        products.ItemSpecs = ValidationHelper.GetString(txtItemSpecs.Text, "");
                        //products.Image=ValidationHelper.
                        products.Insert(createNode, true);
                        //Response.Redirect(ProgramListURL);
                    }
                }
            }
        }
        catch(Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "btnSave_Click", ex.Message);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int programID = ValidationHelper.GetInteger(ddlProgram.SelectedValue, 0);
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
        }
        catch(Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "btnUpdate_Click", ex.Message);
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBrand.Text = ddlProgram.SelectedValue.ToString();
    }
}



