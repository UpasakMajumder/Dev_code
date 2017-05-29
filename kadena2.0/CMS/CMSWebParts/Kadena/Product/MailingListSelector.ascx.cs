using CMS.PortalEngine.Web.UI;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class MailingListSelector : CMSAbstractWebPart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var mailingListData = Old_App_Code.Helpers.ServiceHelper.GetMailingLists();
            if (mailingListData.Count() > 0)
            {
                foreach (var d in mailingListData)
                {
                    var tr = new TableRow();
                    tr.Cells.Add(new TableCell { Text = d.createDate.ToString("MMM dd yyyy") });
                    tr.Cells.Add(new TableCell { Text = d.addressCount.ToString() });
                    tr.Cells.Add(new TableCell { Text = d.errorCount.ToString() });
                    tr.Cells.Add(new TableCell { Text = d.validTo.ToString("MMM dd yyyy") });
                    tr.Cells.Add(new TableCell { Text = d.state?.ToString() ?? string.Empty });

                    if (d.validTo > DateTime.Now.Date)
                    {
                        var btnCell = new TableCell();
                        var btn = new HtmlButton();
                        btn.ID = d.id;
                        btn.ClientIDMode = ClientIDMode.Static;
                        btn.InnerText = "Use";
                        btn.Attributes["class"] = "btn-action";
                        btn.ServerClick += btnUse_ServerClick;
                        btnCell.Controls.Add(btn);
                        tr.Cells.Add(btnCell);
                    }
                    else
                    {
                        tr.Cells.Add(new TableCell { Text = "List expired" });
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
        }
    }
}