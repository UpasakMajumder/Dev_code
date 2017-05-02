using CMS.IO;
using CMS.PortalEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class MailingListUploader : CMSAbstractWebPart
    {
        private Dictionary<string, string> _mailTypes = new Dictionary<string, string>() {
            { "FirstClass", "First Class" },
            { "StandartUnsorted", "Standart - Unsorted" },
            { "StandartSorted", "Standart - Sorted" }
        };
        private Dictionary<string, string> _products = new Dictionary<string, string>() {
            { "Postcard", "Postcard" },
            { "Letter", "Letter" },
            { "SelfMailer", "Self-mailer" }
        };
        private Dictionary<string, string> _validity = new Dictionary<string, string>() {
            { "1week", "1 week" },
            { "90days", "90 days" },
            { "unlimited", "Unlimited" }
        };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
            }
            else
            {
                btnHelp.Attributes["data-tooltip-title"] = GetString("Kadena.MailingList.HelpUpload");
                textFileToUpload.InnerText = GetString("Kadena.MailingList.FileToUpload");
                textOr.InnerText = GetString("Kadena.MailingList.Or");
                textSkipField.InnerText = GetString("Kadena.MailingList.SkipField");
                btnSubmit.InnerText = GetString("Kadena.MailingList.Create");
                textFileName1.InnerText = GetString("Kadena.MailingList.FileName");
                textFileName2.InnerText = GetString("Kadena.MailingList.FileName");
                textFileNameDescr.InnerText = GetString("Kadena.MailingList.FileNameDescription");
                inpFileName.Attributes["placeholder"] = GetString("Kadena.MailingList.FileName");
            }

            phMailType.Controls.Add(new LiteralControl(
                    GetDictionaryHTML(GetString("Kadena.MailingList.MailType")
                                    , GetString("Kadena.MailingList.MailTypeDescription")
                                    , _mailTypes)));
            phProduct.Controls.Add(new LiteralControl(
                GetDictionaryHTML(GetString("Kadena.MailingList.Product")
                                    , GetString("Kadena.MailingList.ProductDesription")
                                    , _products)));
            phValidity.Controls.Add(new LiteralControl(
                GetDictionaryHTML(GetString("Kadena.MailingList.Validity")
                                    , GetString("Kadena.MailingList.ValidityDescription")
                                    , _validity)));
        }

        private string GetDictionaryHTML(string name, string description, IDictionary<string, string> options)
        {
            // We could use classes from System.Web.UI.HtmlControls namespace but Kentico encrypts some attributes of tags.

            var dictionaryName = name.Trim().Replace(' ', '-').ToLower();
            var stringWriter = new StringWriter();
            var html = new HtmlTextWriter(stringWriter);
            html.AddAttribute(HtmlTextWriterAttribute.Class, "upload-mail__row");
            html.RenderBeginTag(HtmlTextWriterTag.Div);
            if (!string.IsNullOrWhiteSpace(name))
            {
                html.RenderBeginTag(HtmlTextWriterTag.H2);
                html.Write(name);
                html.RenderEndTag();
            }
            if (!string.IsNullOrWhiteSpace(description))
            {
                html.RenderBeginTag(HtmlTextWriterTag.P);
                html.Write(description);
                html.RenderEndTag();
            }
            if (options.Count() > 0)
            {
                html.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                html.RenderBeginTag(HtmlTextWriterTag.Div);
                bool isChecked = false;
                foreach (var o in options)
                {
                    var id = $"{dictionaryName}{o.Key}Id";

                    html.AddAttribute(HtmlTextWriterAttribute.Class, "col-lg-4 col-xl-3");
                    html.RenderBeginTag(HtmlTextWriterTag.Div);

                    html.AddAttribute(HtmlTextWriterAttribute.Class, "input__wrapper");
                    html.RenderBeginTag(HtmlTextWriterTag.Div);

                    html.AddAttribute(HtmlTextWriterAttribute.Class, "input__radio");
                    html.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
                    html.AddAttribute(HtmlTextWriterAttribute.Name, dictionaryName);
                    html.AddAttribute(HtmlTextWriterAttribute.Id, id);
                    html.AddAttribute(HtmlTextWriterAttribute.Value, o.Key);
                    if (!isChecked)
                    {
                        html.AddAttribute(HtmlTextWriterAttribute.Checked, string.Empty);
                        isChecked = true;
                    }
                    html.RenderBeginTag(HtmlTextWriterTag.Input);
                    html.RenderEndTag();

                    html.AddAttribute(HtmlTextWriterAttribute.Class, "input__label input__label--radio");
                    html.AddAttribute(HtmlTextWriterAttribute.For, id);
                    html.RenderBeginTag(HtmlTextWriterTag.Label);
                    html.Write(o.Value);
                    html.RenderEndTag();

                    html.RenderEndTag();
                    html.RenderEndTag();
                }
                html.RenderEndTag();
            }
            html.RenderEndTag();
            return stringWriter.ToString();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsPostBack)
            {

                var nextStepUrl = DocumentContext.CurrentDocument.GetStringValue("UrlNextPage", string.Empty);
                Response.Redirect(nextStepUrl);
            }
        }
    }
}