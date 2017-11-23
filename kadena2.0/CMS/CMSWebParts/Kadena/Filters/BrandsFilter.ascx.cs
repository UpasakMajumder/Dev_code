using System;
using CMS.Helpers;

using CMS.DocumentEngine.Web.UI;
using CMS.DataEngine;
using CMS.Ecommerce;
using System.Web.UI.WebControls;
using CMS.EventLog;
using System.Linq;


namespace CMSApp.CMSWebParts.Kadena.Filters
{
    public partial class BrandsFilter : CMSAbstractBaseFilterControl
    {
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

            // Initializes only if the current request is NOT a postback
            else if (!RequestHelper.IsPostBack())
            {
                
            }
        }

        /// <summary>
        /// Generates a WHERE condition and ORDER BY clause based on the current filtering selection.
        /// </summary>
        private void SetFilter()
        {
            string where = null;
            string order = null;

            
            if (!string.IsNullOrEmpty(txtSearchBrand.Text))
            {
                string filterText = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(txtSearchBrand.Text));

                where += "BrandName like '%" + filterText + "%' or BrandCode like '%" + filterText + "%'";
            }



            if (where != null)
            {
                // Sets the Where condition
                this.WhereCondition = where;
            }

            if (order != null)
            {
                // Sets the OrderBy clause
                this.OrderBy = order;
            }

            // Raises the filter changed event
            this.RaiseOnFilterChanged();
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

        protected void txtSearchBrand_TextChanged(object sender, EventArgs e)
        {
            SetFilter();

        }
    }
}