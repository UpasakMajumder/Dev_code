using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMSApp.CMSWebParts.Kadena.POS
{
    public partial class POSFilter : CMSAbstractDataFilterControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
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
                txtPOSSearch.Attributes.Add("placeholder", ResHelper.GetString("Kadena.CategoryForm.txtsearchWatermark"));
            }

        }
        /// <summary>
        /// Generates a WHERE condition and ORDER BY clause based on the current filtering selection.
        /// </summary>
        private void SetFilter()
        {
            string where = null;
         
            // Generates a WHERE condition based on the selected product department
            if (this.txtPOSSearch.Text != null)
            {
                // Gets the ID of the selected department
                string posNo = ValidationHelper.GetString(this.txtPOSSearch.Text, "");
                posNo = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(posNo));
                where = $"Year LIKE N'%  {posNo} %' OR POSCode LIKE N'% {posNo} %' OR POSCategoryName LIKE N'% {posNo} %' OR POSNumber LIKE N'% { posNo } %' OR BrandName LIKE N'% {posNo}%'";
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