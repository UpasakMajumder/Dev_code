using CMS.IO;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

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
        private Guid _fileId;
        private Guid _containerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnProcess.InnerText = GetString("Kadena.MailingList.ProcessList", string.Empty);
                btnReupload.InnerText = GetString("Kadena.MailingList.ReuploadList", string.Empty);
            }
            string[] headers = null;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["fileid"])
                && !string.IsNullOrWhiteSpace(Request.QueryString["containerid"]))
            {
                _fileId = new Guid(Request.QueryString["fileid"]);
                _containerId = new Guid(Request.QueryString["containerid"]);

                headers = ServiceHelper.GetHeaders(_fileId).ToArray();
            }

            foreach (var cs in _columnSelectors)
            {
                phTitle.Controls.Add(new LiteralControl(GetColumnSelectorHtml(cs.Item2, cs.Item1, cs.Item3, headers)));
            }
        }

        /// <summary>
        /// Creates column selector for specified column.
        /// </summary>
        /// <param name="columnName">Name of column to map.</param>
        /// <param name="optional">Flag to show if column is optional.</param>
        /// <param name="availableColumns">List of available columns to be mapped</param>
        /// <returns>String with html-code of column selector.</returns>
        private static string GetColumnSelectorHtml(string columnName, string displayName, bool optional, string[] availableColumns)
        {
            using (var writer = new StringWriter())
            {
                using (var htmlWriter = new HtmlTextWriter(writer))
                {
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "input__wrapper");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "input__label");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Span);
                    htmlWriter.Write(displayName);
                    htmlWriter.RenderEndTag();

                    if (optional)
                    {
                        htmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "input__right-label");
                        htmlWriter.RenderBeginTag(HtmlTextWriterTag.Span);
                        htmlWriter.Write("optional");
                        htmlWriter.RenderEndTag();
                    }
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "input__select");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, columnName);
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Value, string.Empty);
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Select);

                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Disabled, string.Empty);
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Selected, string.Empty);
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Option);
                    htmlWriter.Write("Empty");
                    htmlWriter.RenderEndTag();

                    if (availableColumns != null)
                    {
                        for (int i = 0; i < availableColumns.Length; i++)
                        {
                            if (availableColumns[i].Contains(displayName))
                                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Selected, string.Empty);
                            htmlWriter.AddAttribute(HtmlTextWriterAttribute.Value, i.ToString());
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Option);
                            htmlWriter.Write(availableColumns[i]);
                            htmlWriter.RenderEndTag();
                        }
                    }

                    htmlWriter.RenderEndTag();
                    htmlWriter.RenderEndTag();
                    htmlWriter.RenderEndTag();
                    return writer.ToString();
                }
            }
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (_fileId != Guid.Empty
                    && _containerId != Guid.Empty)
                {
                    var mapping = new Dictionary<string, int>();
                    foreach (var c in _columnSelectors)
                    {
                        var columnName = c.Item2;
                        var columnIndex = string.IsNullOrWhiteSpace(Request.Form[columnName]) ?
                            -1
                            : int.Parse(Request.Form[columnName]);
                        mapping.Add(columnName, columnIndex);
                    }
                    ServiceHelper.UploadMapping(_fileId, _containerId, mapping);
                    Response.Redirect(GetStringValue("ProcessListPageUrl", string.Empty));
                }
            }
        }

        protected void btnReupload_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(GetStringValue("ReuploadListPageUrl", string.Empty));
        }
    }
}