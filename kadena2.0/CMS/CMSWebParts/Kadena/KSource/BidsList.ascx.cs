using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Helpers;
using Kadena.Old_App_Code.Kadena.KSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.UI.HtmlControls;

namespace Kadena.CMSWebParts.Kadena.KSource
{
    public partial class BidsList : CMSAbstractWebPart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IEnumerable<ProjectData> projects = null;
            try
            {
                projects = BidServiceHelper.GetProjects();
            }
            catch(HttpRequestException) { }

            int openCount = 0, completedCount = 0;
            if (projects != null)
            {
                var openProjects = projects.Where(p => p.Active).OrderBy(p => p.UpdateDate);
                var completedProjects = projects.Where(p => !p.Active).OrderBy(p => p.UpdateDate);

                openCount = openProjects.Count();
                completedCount = completedProjects.Count();

                FillTable(tblOpenProjects, openProjects);
                FillTable(tblCompletedProjects, completedProjects);
            }

            lblOpenProject.InnerText = string.Format(GetString("Kadena.KSource.OpenProjectsCaption"), openCount);
            lblCompletedProjects.InnerText = string.Format(GetString("Kadena.KSource.CompletedProjectsCaption"), completedCount);
        }

        private static void FillTable(HtmlTable table, IEnumerable<ProjectData> data)
        {
            foreach (var i in data)
            {
                table.Rows.Add(
                    new HtmlTableRow
                    {
                        Cells = {
                        new HtmlTableCell { InnerText = i.Name},
                        new HtmlTableCell { InnerText = i.RequestId.ToString()},
                        new HtmlTableCell { InnerText = i.Status},
                        new HtmlTableCell { InnerText = i.UpdateDate.ToString("MMM dd yyyy")}
                        }
                    }
                    );
            }
        }
    }
}