using CMS.Ecommerce.Web.UI;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.BusinessLogic.Factories;
using Kadena.Container.Default;
using Kadena.Helpers.Data;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace Kadena.CMSModules.Kadena.Pages.Orders
{
    public partial class OrdersList : CMSEcommercePage
    {
        public IKenticoLogger Logger { get; set; }
            = DIContainer.Resolve<IKenticoLogger>();
        public IOrderReportService ReportService { get; set; }
            = DIContainer.Resolve<IOrderReportService>();

        public IKenticoSiteProvider KenticoSiteProvider { get; set; }
            = DIContainer.Resolve<IKenticoSiteProvider>();

        public IOrderReportFactory OrderReportFactory { get; set; }
            = DIContainer.Resolve<IOrderReportFactory>();

        protected void Page_Load(object sender, EventArgs e)
        {
            HideErrorMessage();

            if (!IsPostBack)
            {
                BindSites();
                PerformFullSearch();
            }
        }

        private void BindSites()
        {
            var sites = KenticoSiteProvider
                .GetSites()
                .Select(s => new ListItem(s.Name, s.Id.ToString()))
                .ToArray();
            siteDropDown.Items.AddRange(sites);
            siteDropDown.SelectedIndex = 0;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            PerformFullSearch();
        }

        protected string RenderTableStyle() => @"
            <style>
                td {
                    border-left: none;
                    border-right: none;
                }
                td.order-by-enabled {
                    text-decoration: underline;
                    cursor: pointer;
                }
            </style>
        ";

        private int FilterSelectedSiteID => Convert.ToInt32(siteDropDown.SelectedValue);
        private string FilterSelectedSiteName => FilterSelectedSiteID > 0
                ? KenticoSiteProvider.GetKenticoSite(FilterSelectedSiteID).Name
                : null;
        private DateTime? FilterDateFrom => dateFrom.SelectedDateTime > DateTime.MinValue 
            ? dateFrom.SelectedDateTime
            : (DateTime?)null;
        private DateTime? FilterDateTo => dateTo.SelectedDateTime > DateTime.MinValue
            ? dateTo.SelectedDateTime
            : (DateTime?)null;
        private string FilterOrderBy => "CreateDate-" + 
            (orderByOrderDateDesc.Value == "1" 
                ? OrderFilter.OrderByDirection.DESC 
                : OrderFilter.OrderByDirection.ASC);
        private OrderFilter Filter => new OrderFilter
        {
            FromDate = FilterDateFrom,
            ToDate = FilterDateTo,
            OrderByExpression = FilterOrderBy
        };
        private int CurrentPageSize => ReportService.OrdersPerPage;

        private int CurrentPage
        {
            get => jumpToPage.SelectedIndex + 1;
            set
            {
                var minPage = 1;
                var maxPage = jumpToPage.Items.Count;
                var pageToSet = value;
                if (pageToSet < minPage)
                {
                    pageToSet = minPage;
                }
                else if (maxPage < pageToSet)
                {
                    pageToSet = maxPage;
                }

                jumpToPage.SelectedIndex = pageToSet - 1;
            }
        }

        private int TotalPages
        {
            get => int.TryParse(totalPages.Text, out var value) ? value : 1;
            set => totalPages.Text = value.ToString();
        }

        private void BindJumpToPage()
        {
            jumpToPage.Items.Clear();

            foreach (var pageNumber in Enumerable.Range(1, TotalPages).Select(p => p.ToString()))
            {
                jumpToPage.Items.Add(pageNumber);
            }
        }

        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            PerformFullSearch();
        }

        private void PerformFullSearch()
        {
            CurrentPage = 1;

            var pagination = ReloadData();

            TotalPages = pagination.PagesCount;
            BindJumpToPage();
        }

        private Pagination ReloadData()
        {
            try
            {
                var orders = ReportService
                    .GetOrdersForSite(FilterSelectedSiteName, CurrentPage, Filter)
                    .Result;

                var ordersReport = OrderReportFactory.CreateReportView(orders.Data);

                var source = new DataView(ordersReport.Items.ToDataSet().Tables[0]);
                ordersDatagrid.DataSource = source;
                ordersDatagrid.DataBind();

                return orders.Pagination;
            }
            catch (AggregateException ex)
            {
                ShowErrorMessage(ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }

            return Pagination.Empty;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var exportFile = ReportService.GetOrdersExportForSite(FilterSelectedSiteName, Filter).Result;
            WriteFileToResponse(exportFile);
        }

        private void WriteFileToResponse(FileResult file)
        {
            Response.Clear();
            Response.ContentType = file.Mime;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

            Response.OutputStream.Write(file.Data, 0, file.Data.Length);
            Response.Flush();

            Response.Close();
        }

        protected void previousPage_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
                ReloadData();
            }
        }

        protected void nextPage_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            ReloadData();
        }

        protected void jumpToPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadData();
        }

        protected void ShowErrorMessage(string messageText)
        {
            messageContainer.Visible = true;
            message.Text = messageText;
        }

        protected void HideErrorMessage() => messageContainer.Visible = false;
    }
}