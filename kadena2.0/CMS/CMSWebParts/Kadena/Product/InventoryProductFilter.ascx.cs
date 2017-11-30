using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using System;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class InventoryProductFilter : CMSAbstractBaseFilterControl
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

        }

        /// <summary>
        /// Generates a WHERE condition and ORDER BY clause based on the current filtering selection.
        /// </summary>
        private void SetFilter()
        {
            string where = null;
            // Generates a WHERE condition based on the search text
            if (!string.IsNullOrEmpty(txtProductSearch.Text))
            {
                var searchText = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(txtProductSearch.Text.Trim()));

                where += $"SKUName like '% { searchText}%' or SKUDescription like '%{ searchText }%' or b.BrandName like '%{searchText }%' or s.SKUNumber like '%{ searchText }%' or BundleQty like '%{ searchText }%'";
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
    }
}