using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Helpers;
using Kadena.Old_App_Code.Kadena.KSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.KSource
{
    public partial class BidsList : CMSAbstractWebPart
    {
        public int RecordsPerPage
        {
            get
            {
                return int.Parse(GetStringValue("RecordsPerPage", "10"));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IEnumerable<ProjectData> projects = null;
            try
            {
                projects = BidServiceHelper.GetProjects();
            }
            catch (HttpRequestException exc)
            {
                EventLogProvider.LogException("BidsList Load", "EXCEPTION", exc, CurrentSite.SiteID);
            }

            int openCount = 0, completedCount = 0;
            if (projects != null)
            {
                var openProjects = projects.Where(p => p.Active).OrderByDescending(p => p.UpdateDate).ToArray();
                var completedProjects = projects.Where(p => !p.Active).OrderByDescending(p => p.UpdateDate).ToArray();

                openCount = openProjects.Count();
                completedCount = completedProjects.Count();
                
                FillTable(tblOpenProjects, phOpenPagination, openProjects, RecordsPerPage);
                FillTable(tblCompletedProjects, phCompletedPagination, completedProjects, RecordsPerPage);
            }

            lblOpenProject.InnerText = string.Format(GetString("Kadena.KSource.OpenProjectsCaption"), openCount);
            lblCompletedProjects.InnerText = string.Format(GetString("Kadena.KSource.CompletedProjectsCaption"), completedCount);
        }

        private static void FillTable(HtmlTable table, PlaceHolder placeHolder, ProjectData[] data, int recordsPerPage)
        {
            for (int i = 0; i < data.Count(); i++)
            {
                var pr = data[i];
                var page = i / recordsPerPage + 1;
                var firstCol = new HtmlTableCell { InnerText = pr.Name };
                firstCol.Attributes["class"] = "show-table__text-cutted fixed-main-td";

                var row = new HtmlTableRow
                {
                    Cells = {
                        firstCol,
                        new HtmlTableCell { InnerText = pr.RequestId.ToString()},
                        new HtmlTableCell { InnerText = pr.Status},
                        new HtmlTableCell { InnerText = pr.UpdateDate.ToString("MMM dd yyyy")}
                        }
                };

                if (page == 1)
                {
                    row.Attributes["class"] = "active";
                }
                row.Attributes["data-page"] = page.ToString();
                table.Rows.Add(row);
            }

            // Adding pagination
            if (data.Count() > recordsPerPage)
            {
                var divBlock = new HtmlGenericControl("div");
                divBlock.Attributes["class"] = "block";

                var divRow = new HtmlGenericControl("div");
                divRow.Attributes["class"] = "row flex-align--center";

                var divMessage = new HtmlGenericControl("div");
                divMessage.Attributes["class"] = "col-6";

                var divPagesOuter = new HtmlGenericControl("div");
                divPagesOuter.Attributes["class"] = "col-6 text--right";

                var divPagesInner = new HtmlGenericControl("div");
                divPagesInner.Attributes["class"] = "js-table-paginator-wrapper generated-paginator";
                divPagesInner.Attributes["data-pages"] = (data.Count() / recordsPerPage + 1).ToString();
                divPagesInner.Attributes["data-rows-on-page"] = recordsPerPage.ToString();

                var span = new HtmlGenericControl("span");

                var spanFrom = new HtmlGenericControl("span");
                spanFrom.Attributes["class"] = "js-table-paginator-from";

                var spanTo = new HtmlGenericControl("span");
                spanTo.Attributes["class"] = "js-table-paginator-to";

                var spanTotal = new HtmlGenericControl("span");
                spanTotal.InnerText = $"{data.Count()}";

                divBlock.Controls.Add(divRow);
                divRow.Controls.Add(divMessage);
                divMessage.Controls.Add(span);
                span.Controls.Add(new LiteralControl($"{ResHelper.GetString("Kadena.KSource.Showing")} "));
                span.Controls.Add(spanFrom);
                span.Controls.Add(new LiteralControl($" {ResHelper.GetString("Kadena.KSource.To")} "));
                span.Controls.Add(spanTo);
                span.Controls.Add(new LiteralControl($" {ResHelper.GetString("Kadena.KSource.Of")} "));
                span.Controls.Add(spanTotal);
                span.Controls.Add(new LiteralControl($" {ResHelper.GetString("Kadena.KSource.Entries")}"));
                divRow.Controls.Add(divPagesOuter);
                divPagesOuter.Controls.Add(divPagesInner);

                placeHolder.Controls.Add(divBlock);
            }
        }
    }
}