using CMS.Helpers;
using CMS.IO;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class ColumnMapper : CMSAbstractWebPart
    {
        private static List<Tuple<string, string, bool>> _columnSelectors = new List<Tuple<string, string, bool>>
        {
            Tuple.Create("title", "Title", true ),
            Tuple.Create("name", "FirstName", false ),
            Tuple.Create("last name", "LastName", true ),
            Tuple.Create("first address line", "Address1", false ),
            Tuple.Create("second address line", "Address2", false ),
            Tuple.Create("city", "City", false ),
            Tuple.Create("state", "State", false ),
            Tuple.Create("zip code", "Zip", false )
        };
        private string _fileId;
        private Guid _containerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["fileid"])
                    && !string.IsNullOrWhiteSpace(Request.QueryString["containerid"]))
            {
                _fileId = Request.QueryString["fileid"];
                _containerId = new Guid(Request.QueryString["containerid"]);
            }

            if (!IsPostBack)
            {
                btnProcess.InnerText = GetString("Kadena.MailingList.ProcessList", string.Empty);
                btnReupload.InnerText = GetString("Kadena.MailingList.ReuploadList", string.Empty);

                if (!string.IsNullOrWhiteSpace(_fileId) && _containerId != Guid.Empty)
                {
                    var headers = ServiceHelper.GetHeaders(_fileId).ToArray();
                    foreach (var cs in _columnSelectors)
                    {
                        var sel = (FindControl($"sel{cs.Item2}") as HtmlSelect);
                        if (sel != null)
                        {
                            var emptyItem = new ListItem(GetString("Kadena.MailingList.Empty"), GetString("Kadena.MailingList.Empty"));
                            if (!cs.Item3)
                                emptyItem.Attributes["disabled"] = string.Empty;
                            sel.Items.Add(emptyItem);
                            sel.Value = GetString("Kadena.MailingList.Empty");

                            for (int i = 0; i < headers.Length; i++)
                            {
                                sel.Items.Add(new ListItem(headers[i], i.ToString()));
                                if (headers[i].ToLower().Contains(cs.Item1.ToLower()))
                                    sel.Value = i.ToString();
                            }
                        }
                    }
                }
            }
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(_fileId)
                    && _containerId != Guid.Empty)
                {
                    var mapping = new Dictionary<string, int>();
                    var isValid = true;
                    foreach (var c in _columnSelectors)
                    {
                        var columnName = c.Item2;
                        var optional = c.Item3;
                        var selectedValue = GetColumnValue(columnName);
                        if (Validate(columnName, selectedValue))
                        {
                            mapping.Add(columnName, selectedValue);
                        }
                        else
                        {
                            isValid = optional;
                        }
                    }
                    if (isValid)
                    {
                        ServiceHelper.UploadMapping(_fileId, _containerId, mapping);
                        ServiceHelper.ValidateAddresses(_containerId);
                        Response.Redirect(GetStringValue("ProcessListPageUrl", string.Empty));
                    }
                }
            }
        }

        protected void btnReupload_ServerClick(object sender, EventArgs e)
        {
            var url = URLHelper.AddParameterToUrl(GetStringValue("ReuploadListPageUrl", string.Empty)
                , "containerid", _containerId.ToString());
            Response.Redirect(url);
        }

        private bool Validate(string columnName, int value)
        {

            var wrap = (FindControl($"wrap{columnName}") as HtmlGenericControl);
            if (wrap != null)
            {
                wrap.Attributes["class"] = value > -1 ? "input__wrapper" : "input__wrapper mb-3";
                var div = (FindControl($"div{columnName}") as HtmlGenericControl);
                if (div != null)
                {
                    div.Attributes["class"] = value > -1 ? "input__select" : "input__select input--error";
                    var span = (FindControl($"span{columnName}") as HtmlGenericControl);
                    if (span != null)
                    {
                        span.InnerText = GetString("Kadena.MailingList.EnterValidValue");
                        span.Visible = (value < 0);
                    }
                }
            }
            return value > -1;
        }

        private int GetColumnValue(string columnName)
        {
            var sel = (FindControl($"sel{columnName}") as HtmlSelect);
            if (sel != null)
            {
                int result = -1;
                if (int.TryParse(sel.Value, out result))
                    return result;
            }
            return -1;
        }
    }
}