using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Helpers;
using Kadena.Old_App_Code.Kadena.MailingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class AddressViewer : CMSAbstractWebPart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var containerId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["containerid"]))
            {
                containerId = new Guid(Request.QueryString["containerid"]);
            }

            if (containerId != Guid.Empty)
            {
                var addresses = ServiceHelper.GetMailingAddresses(containerId);
                var badAddresses = addresses.Where(a => a.Error != null);
                var goodAddresses = addresses.Where(a => a.Error == null);

                if (badAddresses.Count() > 0)
                {
                    pnlBadAddresses.Visible = true;

                    FillTable(tblBadAddresses, badAddresses);
                }

                if (goodAddresses.Count() > 0)
                {
                    pnlGoodAddresses.Visible = true;

                    FillTable(tblGoodAddresses, goodAddresses);
                }
            }
        }

        private static void FillTable(Table table, IEnumerable<MailingAddressData> addresses)
        {
            var haveTitle = addresses.Count(a => a.Title != null) > 0;
            var haveLastName = addresses.Count(a => a.LastName != null) > 0;

            #region Add Header
            var header = new TableRow();
            if (haveTitle)
            {
                header.Cells.Add(new TableHeaderCell { Text = "Title" });
            }
            header.Cells.Add(new TableHeaderCell { Text = "Name" });
            if (haveLastName)
            {
                header.Cells.Add(new TableHeaderCell { Text = "LastName" });
            }
            header.Cells.Add(new TableHeaderCell { Text = "Address1" });
            header.Cells.Add(new TableHeaderCell { Text = "Address2" });
            header.Cells.Add(new TableHeaderCell { Text = "City" });
            header.Cells.Add(new TableHeaderCell { Text = "State" });
            header.Cells.Add(new TableHeaderCell { Text = "Zip" });
            table.Rows.Add(header);
            #endregion

            #region Add Data
            foreach (var a in addresses)
            {
                var row = new TableRow();
                if (haveTitle)
                {
                    row.Cells.Add(new TableCell { Text = a.Title ?? "-" });
                }
                row.Cells.Add(new TableCell { Text = a.Name ?? "-" });
                if (haveLastName)
                {
                    row.Cells.Add(new TableCell { Text = a.LastName ?? "-" });
                }
                row.Cells.Add(new TableCell { Text = a.Address1 ?? "-" });
                row.Cells.Add(new TableCell { Text = a.Address2 ?? "-" });
                row.Cells.Add(new TableCell { Text = a.City ?? "-" });
                row.Cells.Add(new TableCell { Text = a.State ?? "-" });
                row.Cells.Add(new TableCell { Text = a.Zip ?? "-" });
                table.Rows.Add(row);
            }
            #endregion
        }

        protected void btnSaveList_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(Request.UrlReferrer.ToString());
        }
    }
}