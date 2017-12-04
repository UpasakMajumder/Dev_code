using System;
using CMS.Helpers;

using CMS.DocumentEngine.Web.UI;
using CMS.DataEngine;


namespace CMSApp.CMSWebParts.Kadena.Filters
{
    public partial class BrandsFilter : CMSAbstractBaseFilterControl
    {
        /// <summary>
        /// Sets up the inner child controls.
        /// </summary>
        private void SetupControl()
        {
            if (this.StopProcessing)
                this.Visible = false;
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
                where += $"BrandName like '%{filterText}'% or BrandCode like '%{filterText}'%";
            }
            if (where != null)
                this.WhereCondition = where;
            if (order != null)
                this.OrderBy = order;
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
                SetFilter();
            base.OnPreRender(e);
        }
        /// <summary>
        /// Filters the data based on text enterder in textbox
        /// </summary>
        protected void txtSearchBrand_TextChanged(object sender, EventArgs e)
        {
            SetFilter();
        }
    }
}