using CMS.DocumentEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.WebControls;

using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.Ecommerce;
using CMS.DataEngine;

namespace CMSApp.CMSWebParts.Kadena.Campaign
{
    public partial class CampaignFilter : CMSAbstractDataFilterControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSearch.Attributes.Add("onKeyPress",
                  "doClick('" + btnFilter.ClientID + "',event)");
            }

        }
        /// <summary>
        /// Init event handler.
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            // Creates the child controls
            SetupControl();
            base.OnInit(e);
        }
        /// <summary>
        /// Sets up the inner child controls.
        /// </summary>
        private void SetupControl()
        {
            // Hides the filter if StopProcessing is enabled
            if (this.StopProcessing)
            {
                this.Visible = false;
            }
            else
            {
                txtSearch.Attributes.Add("placeholder", ResHelper.GetString("Kadena.Campaignlst.Searchtext"));
            }

        }
        /// <summary>
        /// Generates a WHERE condition and ORDER BY clause based on the current filtering selection.
        /// </summary>
        private void SetFilter()
        {
            string where = null;
            //string order = null;

            // Generates a WHERE condition based on the selected product department
            if (this.txtSearch.Text != null)
            {
                // Gets the ID of the selected department
                string campaignname = ValidationHelper.GetString(this.txtSearch.Text, "");
                campaignname= SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(campaignname));
                where = "Name like '%" + campaignname + "%' OR Description like '%" + campaignname + "%'";
                
            }
            if (where != null)
            {
                // Sets the Where condition
                this.WhereCondition = where;
            }

            // Raises the filter changed event
            this.RaiseOnFilterChanged();
        }
        /// <summary>
        /// PreRender event handler
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            // Checks if the current request is a postback
            if (RequestHelper.IsPostBack())
            {
                // Applies the filter to the displayed data
                SetFilter();
            }

            base.OnPreRender(e);
        }
    }
}