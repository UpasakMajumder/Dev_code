using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Kadena.Old_App_Code.Kadena.Chili;

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
            var mailingListData = Old_App_Code.Helpers.ServiceHelper.GetMailingLists().Where(l => l.AddressCount > 0);
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

                    if (d.ValidTo > DateTime.Now.Date)
                    {
                        var btnCell = new TableCell();
                        var btn = new HtmlButton();
                        btn.ID = d.Id;
                        btn.ClientIDMode = ClientIDMode.Static;
                        btn.InnerText = GetString("Kadena.MailingList.Use");
                        btn.Attributes["class"] = "btn-action";
                        btn.ServerClick += btnUse_ServerClick;
                        btnCell.Controls.Add(btn);
                        tr.Cells.Add(btnCell);
                    }
                    else
                    {
                        tr.Cells.Add(new TableCell { Text = GetString("Kadena.MailingList.ListExpired") });
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
                if (!string.IsNullOrWhiteSpace(containerId) && !string.IsNullOrWhiteSpace(templateId) && !string.IsNullOrWhiteSpace(workspaceId))
                {
                    new TemplateServiceHelper().SetMailingList(containerId, templateId, workspaceId);
                    url += "&containerId=" + containerId;
                    Response.Redirect(url);
                }
            }
        }
    }
}