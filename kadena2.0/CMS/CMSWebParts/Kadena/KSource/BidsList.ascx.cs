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
                new { OrderDate = "Mar 3 2017", projectName = "Brochure", requestId = "123545", projectStatus="Waiting for bids", lastUpdate= "May 24 2017 16:45"},
                new { OrderDate = "Mar 3 2017", projectName = "Brochure2", requestId = "123545", projectStatus="Waiting for bids", lastUpdate= "May 24 2017 16:45"}
            };
            foreach (var i in items)
            {
                tblOpenProjects.Rows.Add(
                    new HtmlTableRow
                    {
                        Cells = {
                        new HtmlTableCell { InnerText = i.OrderDate },
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