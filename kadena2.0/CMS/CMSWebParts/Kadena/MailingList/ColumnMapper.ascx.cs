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
        private static Dictionary<string, bool> _columnSelectors = new Dictionary<string, bool>
        {
            {"title", true },
            {"name", false },
            {"last name", true },
            {"first address line", false },
            {"second address line", false },
            {"city", false },
            {"state", false },
            {"zip code", false },
        };
        private string[] _headers;
        private Guid _fileId;
        private Guid _containerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["fileid"])
                && !string.IsNullOrWhiteSpace(Request.QueryString["containerid"]))
            {
                _fileId = new Guid(Request.QueryString["fileid"]);
                _containerId = new Guid(Request.QueryString["containerid"]);

                _headers = ServiceHelper.GetHeaders(_fileId).ToArray();
            }

            foreach (var cs in _columnSelectors)
            {
                phTitle.Controls.Add(new LiteralControl(GetColumnSelectorHtml(cs.Key, cs.Value, _headers)));
            }
        }

        /// <summary>
        /// Creates column selector for specified column.
        /// </summary>
        /// <param name="columnName">Name of column to map.</param>
        /// <param name="optional">Flag to show if column is optional.</param>
        /// <param name="availableColumns">List of available columns to be mapped</param>
        /// <returns>String with html-code of column selector.</returns>
        private static string GetColumnSelectorHtml(string columnName, bool optional, string[] availableColumns)
        {
            using (var writer = new StringWriter())
            {
                using (var htmlWriter = new HtmlTextWriter(writer))
                {

                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "input__wrapper");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "input__label");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Span);
                    htmlWriter.Write(columnName);
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

                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, $"col{columnName}");
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
                            if (availableColumns[i].Contains(columnName))
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

        }
    }
}