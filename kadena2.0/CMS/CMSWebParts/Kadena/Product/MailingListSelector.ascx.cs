using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Kadena.Old_App_Code.Kadena.Chili;
using CMS.DataEngine;
using CMS.SiteProvider;
using Kadena2.MicroserviceClients.Clients;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class MailingListSelector : CMSAbstractWebPart
    {
        public string NewMailingListUrl
        {
            get
            {
                return GetStringValue("NewMailingListUrl", string.Empty);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var url = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.KDA_GetMailingListsUrl");
            var client = new MailingListClient();

            var mailingListData = client.GetMailingListsForCustomer(url, SiteContext.CurrentSiteName).Result.Payload
                .Where(l => l.AddressCount > 0);
            if (mailingListData.Count() > 0)
            {
                var useUrl = URLHelper.URLDecode(Request.QueryString["url"]);
                var templateId = string.IsNullOrWhiteSpace(useUrl) ? string.Empty : URLHelper.GetUrlParameter(useUrl, "templateId");
                var workspaceId = string.IsNullOrWhiteSpace(useUrl) ? string.Empty : URLHelper.GetUrlParameter(useUrl, "workspaceid");

                foreach (var d in mailingListData)
                {
                    // Generate table
                    var tr = new TableRow();
                    tr.Cells.Add(new TableCell { Text = d.Name });
                    tr.Cells.Add(new TableCell { Text = d.CreateDate.ToString("MMM dd yyyy") });
                    tr.Cells.Add(new TableCell { Text = d.AddressCount.ToString() });
                    tr.Cells.Add(new TableCell { Text = d.ErrorCount.ToString() });
                    tr.Cells.Add(new TableCell { Text = d.ValidTo.ToString("MMM dd yyyy") });
                    tr.Cells.Add(new TableCell { Text = d.State?.ToString() ?? string.Empty });

                    TableCell cellButton = null;
                    if (cellButton == null && d.ValidTo <= DateTime.Now.Date)
                    {
                        cellButton = new TableCell { Text = GetString("Kadena.MailingList.ListExpired") };
                    }

                    if (cellButton == null 
                        && !d.State.Equals("Addresses verified") 
                        && !d.State.Equals("Addresses need to be verified"))
                    {
                        cellButton = new TableCell { Text = GetString("Kadena.MailingList.ListInProgress") };
                    }

                    if (cellButton == null)
                    {
                        cellButton = new TableCell();
                        var link = new HyperLink();
                        var containerId = d.Id;
                        if (!string.IsNullOrWhiteSpace(containerId) && !string.IsNullOrWhiteSpace(templateId) && !string.IsNullOrWhiteSpace(workspaceId))
                        {
                            new TemplateServiceHelper().SetMailingList(containerId, templateId, workspaceId);
                            link.NavigateUrl = $"{useUrl}&containerId={containerId}&quantity={d.AddressCount}";
                        }
                        link.Text = GetString("Kadena.MailingList.Use");
                        link.Attributes["class"] = "btn-action";
                        cellButton.Controls.Add(link);
                    }

                    if (cellButton != null)
                    {
                        tr.Cells.Add(cellButton);
                    }

                    tblMalilingList.Rows.Add(tr);
                }
                tblMalilingList.Visible = true;
                pnlNewList.Visible = false;
            }
            else
            {
                tblMalilingList.Visible = false;
                pnlNewList.Visible = true;
            }
        }
    }
}