using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Kadena.MicroserviceClients.Contracts;
using Kadena.Old_App_Code.Kadena.MailingList;
using CMS.EventLog;
using Kadena.WebAPI.KenticoProviders;
using Kadena2.Container.Default;
using Kadena2.MicroserviceClients.Contracts;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class MailingListSelector : CMSAbstractWebPart
    {
        private KenticoResourceService _resources = new KenticoResourceService();
        public string NewMailingListUrl
        {
            get
            {
                return GetStringValue("NewMailingListUrl", string.Empty);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var client = DIContainer.Resolve<IMailingListClient>();

            var mailingListData = client.GetMailingListsForCustomer().Result.Payload
                .Where(l => l.AddressCount > 0);

            if (mailingListData.Count() > 0)
            {
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

                    TableCell btnCell = null;
                    if (btnCell == null && d.ValidTo <= DateTime.Now.Date)
                    {
                        btnCell = new TableCell { Text = GetString("Kadena.MailingList.ListExpired") };
                    }

                    if (btnCell == null
                        && !d.State.Equals(MailingListState.AddressesVerified)
                        && !d.State.Equals(MailingListState.AddressesNeedToBeVerified))
                    {
                        btnCell = new TableCell { Text = GetString("Kadena.MailingList.ListInProgress") };
                    }

                    if (btnCell == null)
                    {
                        btnCell = new TableCell();
                        var btn = new HtmlButton();
                        btn.ID = d.Id;
                        btn.Attributes.Add("quantity", d.AddressCount.ToString());
                        btn.ClientIDMode = ClientIDMode.Static;
                        btn.InnerText = GetString("Kadena.MailingList.Use");
                        btn.Attributes["class"] = "btn-action";
                        btn.ServerClick += btnUse_ServerClick;
                        btnCell.Controls.Add(btn);
                    }

                    if (btnCell != null)
                    {
                        tr.Cells.Add(btnCell);
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

        private void btnUse_ServerClick(object sender, EventArgs e)
        {
            var btn = sender as HtmlButton;
            if (btn != null)
            {
                var url = URLHelper.URLDecode(Request.QueryString["url"]);
                var containerId = btn.ID;
                var templateId = string.IsNullOrWhiteSpace(url) ? string.Empty : URLHelper.GetUrlParameter(url, "templateid");
                var workspaceId = string.IsNullOrWhiteSpace(url) ? string.Empty : URLHelper.GetUrlParameter(url, "workspaceid");
                var use3d = string.IsNullOrWhiteSpace(url) ? false : bool.Parse(URLHelper.GetUrlParameter(url, "use3d"));
                var quantity = btn.Attributes["quantity"];
                if (!string.IsNullOrWhiteSpace(containerId) && !string.IsNullOrWhiteSpace(templateId) && !string.IsNullOrWhiteSpace(workspaceId))
                {
                    var templateClient = DIContainer.Resolve<ITemplatedClient>();
                    var setResult = templateClient.SetMailingList(containerId, templateId, workspaceId, use3d).Result;
                    if (!setResult.Success)
                    {
                        EventLogProvider.LogEvent(EventType.ERROR, "SET MAILING LIST", "ERROR", setResult.ErrorMessages);
                    }
                    url += $"&containerId={containerId}&quantity={quantity}";
                    Response.Redirect(url);
                }
            }
        }
    }
}