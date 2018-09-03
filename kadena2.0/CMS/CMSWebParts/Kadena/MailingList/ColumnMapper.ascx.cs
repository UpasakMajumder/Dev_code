using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using Kadena.Container.Default;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Tuple.Create("second address line", "Address2", true ),
            Tuple.Create("city", "City", false ),
            Tuple.Create("state", "State", false ),
            Tuple.Create("zip code", "Zip", false )
        };        
        private string _fileId;
        private Guid _containerId;

        public string ReuploadListPageUrl
        {
            get
            {
                return GetStringValue("ReuploadListPageUrl", string.Empty);
            }
        }

        public string ProcessListPageUrl
        {
            get
            {
                return GetStringValue("ProcessListPageUrl", string.Empty);
            }
        }

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
            }

            if (!string.IsNullOrWhiteSpace(_fileId) && _containerId != Guid.Empty)
            {
                var parsingClient = DIContainer.Resolve<IParsingClient>();
                var parseResult = parsingClient.GetHeaders(_fileId.ToString()).Result;
                if (parseResult.Success)
                {
                    var headers = parseResult.Payload.ToArray();

                    foreach (var cs in _columnSelectors)
                    {
                        var sel = (FindControl($"sel{cs.Item2}") as HtmlSelect);
                        if (sel != null)
                        {
                            var selectedValue = GetColumnValue(cs.Item2);
                            var valueToSelect = -1;
                            sel.Items.Clear();

                            var emptyItem = new ListItem(GetString("Kadena.MailingList.Empty"), GetString("Kadena.MailingList.Empty"));
                            if (!cs.Item3)
                                emptyItem.Attributes["disabled"] = string.Empty;
                            sel.Items.Add(emptyItem);

                            for (int i = 0; i < headers.Length; i++)
                            {
                                sel.Items.Add(new ListItem(headers[i], i.ToString()));
                                if (headers[i].ToLower().Contains(cs.Item1.ToLower()))
                                    valueToSelect = i;
                            }
                            if (selectedValue > -1)
                            {
                                valueToSelect = selectedValue;
                            }
                            sel.Value = valueToSelect < 0 ? GetString("Kadena.MailingList.Empty") : valueToSelect.ToString();
                        }
                    }
                }
                else
                {
                    EventLogProvider.LogEvent(EventType.ERROR, GetType().Name, "ParsingHeaders", parseResult.ErrorMessages, siteId: CurrentSite.SiteID);
                }
            }
        }

        protected void btnProcess_Click(object sender, EventArgs e)
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
                    try
                    {
                        var mailingClient = DIContainer.Resolve<IMailingListClient>();
                        var uploadResult = mailingClient.UploadMapping(_fileId, _containerId, mapping).Result;
                        if (!uploadResult.Success)
                        {
                            throw new InvalidOperationException(uploadResult.ErrorMessages);
                        }
                        var validationClient = DIContainer.Resolve<IAddressValidationClient>();
                        var validationResult = validationClient.Validate(_containerId).Result;
                        if (!validationResult.Success)
                        {
                            throw new InvalidOperationException(validationResult.ErrorMessages);
                        }
                        Response.Redirect(ProcessListPageUrl);
                    }
                    catch (Exception ex)
                    {
                        EventLogProvider.LogException("Mailing List - Column mapping", "PROCESS", ex);
                        inpErrorTitle.Value = ResHelper.GetString("Kadena.MailingList.ColumnMapping.GeneralErrorTitle");
                        inpErrorText.Value = ResHelper.GetString("Kadena.MailingList.ColumnMapping.GeneralErrorText");
                    }
                }
            }
            else
            {
                inpErrorTitle.Value = ResHelper.GetString("Kadena.MailingList.ColumnMapping.GeneralErrorTitle");
                inpErrorText.Value = ResHelper.GetString("Kadena.MailingList.ColumnMapping.GeneralErrorText");
            }
        }

        protected void btnReupload_ServerClick(object sender, EventArgs e)
        {
            var url = URLHelper.AddParameterToUrl(ReuploadListPageUrl
                , "containerid", _containerId.ToString());
            Response.Redirect(url);
        }

        private bool Validate(string columnName, int value)
        {
            var wrap = (FindControl($"wrap{columnName}") as HtmlGenericControl);
            if (wrap != null)
            {
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