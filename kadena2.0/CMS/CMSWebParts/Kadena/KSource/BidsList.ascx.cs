using CMS.PortalEngine.Web.UI;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;

namespace Kadena.CMSWebParts.Kadena.KSource
{
    public partial class BidsList : CMSAbstractWebPart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var items = new[] {
                new { projectName = "Brochure", requestId = "123545", projectStatus="Waiting for bids", lastUpdate= "May 24 2017 16:45"},
                new { projectName = "Brochure2", requestId = "123545", projectStatus="Waiting for bids", lastUpdate= "May 24 2017 16:45"}
            };
            lblOpenProject.InnerText = string.Format(GetString("Kadena.KSource.OpenProjectsCaption"), items.Count());
            foreach (var i in items)
            {
                tblOpenProjects.Rows.Add(
                    new HtmlTableRow
                    {
                        Cells = {
                        new HtmlTableCell { InnerText = i.projectName},
                        new HtmlTableCell { InnerText = i.requestId},
                        new HtmlTableCell { InnerText = i.projectStatus},
                        new HtmlTableCell { InnerText = i.lastUpdate}
                        }
                    }
                    );
                tblCompletedProjects.Rows.Add(
                    new HtmlTableRow
                    {
                        Cells = {
                        new HtmlTableCell { InnerText = i.projectName},
                        new HtmlTableCell { InnerText = i.requestId},
                        new HtmlTableCell { InnerText = i.projectStatus},
                        new HtmlTableCell { InnerText = i.lastUpdate}
                        }
                    }
                    );
            }
        }
    }
}