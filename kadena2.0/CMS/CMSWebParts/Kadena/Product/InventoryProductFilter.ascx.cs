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
            if (!string.IsNullOrEmpty(txtProductSearch.Text))
            {
                var searchText = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(txtProductSearch.Text.Trim()));
                where += $"SKUName like '% { searchText}%' or SKUDescription like '%{ searchText }%' or b.BrandName like '%{searchText }%' or s.SKUProductCustomerReferenceNumber like '%{ searchText }%' or s.SKUNumberOfItemsInPackage like '%{ searchText }%'";
            }
            if (where != null)
            {
                this.WhereCondition = where;
            }
            this.RaiseOnFilterChanged();
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
    }
}