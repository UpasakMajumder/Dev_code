using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Kadena.MicroserviceClients.Contracts;
using CMS.EventLog;
using Kadena.Container.Default;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Dto.TemplatedProduct.MicroserviceRequests;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Helpers;
using Kadena.Dto.MailingList.MicroserviceResponses;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class MailingListSelector : CMSAbstractWebPart
    {
        private IKenticoResourceService _resources = DIContainer.Resolve<IKenticoResourceService>();
        private IKenticoSiteProvider _site = DIContainer.Resolve<IKenticoSiteProvider>();

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
                    tr.Cells.Add(new TableCell { Text = d.State.GetDisplayName() });

                    TableCell btnCell = null;
                    if (btnCell == null && d.ValidTo <= DateTime.Now.Date)
                    {
                        btnCell = new TableCell { Text = GetString("Kadena.MailingList.ListExpired") };
                    }

                    if (btnCell == null
                        && d.State != ContainerState.AddressesVerified
                        && d.State != ContainerState.AddressesNeedVerification)
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
                var customerName = _site.GetKenticoSite().Name;
                if (!string.IsNullOrWhiteSpace(containerId) && !string.IsNullOrWhiteSpace(templateId) && !string.IsNullOrWhiteSpace(workspaceId))
                {
                    var templateClient = DIContainer.Resolve<ITemplatedClient>();
                    var setMailingRequest = new SetMailingListRequestDTO
                    {
                        ContainerId = containerId,
                        TemplateId = templateId,
                        WorkSpaceId = workspaceId,
                        Use3d = use3d,
                        CustomerName = customerName
                    };

                    var setResult = templateClient.SetMailingList(setMailingRequest).Result;
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