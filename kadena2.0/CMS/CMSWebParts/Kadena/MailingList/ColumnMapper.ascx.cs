using CMS.IO;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            IEnumerable<string> headers = null;
            if (!IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["fileid"])
                    && !string.IsNullOrWhiteSpace(Request.QueryString["containerid"]))
                {
                    var fileId = new Guid(Request.QueryString["fileid"]);
                    var containerId = new Guid(Request.QueryString["containerid"]);

                    headers = ServiceHelper.GetHeaders(fileId);
                }
            }

            foreach (var cs in _columnSelectors)
            {
                phTitle.Controls.Add(new LiteralControl(GetColumnSelectorHtml(cs.Key, cs.Value, headers)));
            }
        }

        /// <summary>
        /// Creates column selector for specified column.
        /// </summary>
        /// <param name="columnName">Name of column to map.</param>
        /// <param name="optional">Flag to show if column is optional.</param>
        /// <param name="availableColumns">List of available columns to be mapped</param>
        /// <returns>String with html-code of column selector.</returns>
        private static string GetColumnSelectorHtml(string columnName, bool optional, IEnumerable<string> availableColumns)
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
                        foreach (var i in availableColumns)
                        {
                            if (i.Contains(columnName))
                                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Selected, string.Empty);
                            htmlWriter.AddAttribute(HtmlTextWriterAttribute.Value, i);
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Option);
                            htmlWriter.Write(i);
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