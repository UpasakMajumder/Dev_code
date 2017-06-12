using CMS.CustomTables;
using CMS.EventLog;
using CMS.Helpers;
using CMS.IO;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Helpers;
using Kadena.Old_App_Code.Kadena.MailingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.UI;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class MailingListUploader : CMSAbstractWebPart
    {
        private readonly string _mailTypeTableName = "KDA.MailingType";
        private readonly string _productTableName = "KDA.MailingProductType";
        private readonly string _validityTableName = "KDA.MailingValidity";
        private MailingListData _container;

        protected void Page_Load(object sender, EventArgs e)
        {
            var containerId = Request.QueryString["containerid"];
            if (!string.IsNullOrWhiteSpace(containerId))
            {
                var id = new Guid(containerId);
                _container = ServiceHelper.GetMailingList(id);
            }
            if (!IsPostBack)
            {
                btnHelp.Attributes["title"] = GetString("Kadena.MailingList.HelpUpload");
                inpFileName.Attributes["placeholder"] = GetString("Kadena.MailingList.FileName");
            }

            var mailTypes = CustomTableItemProvider.GetItems(_mailTypeTableName)
                    .OrderBy("ItemOrder")
                    .ToDictionary(row => row["CodeName"].ToString(), row => row["DisplayName"].ToString());
            phMailType.Controls.Add(new LiteralControl(
                    GetDictionaryHTML(GetString("Kadena.MailingList.MailType")
                                    , GetString("Kadena.MailingList.MailTypeDescription")
                                    , mailTypes
                                    , _container?.mailType)));

            var products = CustomTableItemProvider.GetItems(_productTableName)
                    .OrderBy("ItemOrder")
                    .ToDictionary(row => row["CodeName"].ToString(), row => row["DisplayName"].ToString());
            phProduct.Controls.Add(new LiteralControl(
                GetDictionaryHTML(GetString("Kadena.MailingList.Product")
                                    , GetString("Kadena.MailingList.ProductDescription")
                                    , products
                                    , _container?.productType)));

            var validity = CustomTableItemProvider.GetItems(_validityTableName)
                    .OrderBy("ItemOrder")
                    .ToDictionary(row => row["DayNumber"].ToString(), row => row["DisplayName"].ToString());
            phValidity.Controls.Add(new LiteralControl(
                GetDictionaryHTML(GetString("Kadena.MailingList.Validity")
                                    , GetString("Kadena.MailingList.ValidityDescription")
                                    , validity
                                    , _container != null ? (_container.validTo - _container.createDate).TotalDays.ToString() : null
                                    )));

            if (_container != null)
            {
                divFileName.CssClass = "input__wrapper input__wrapper--disabled";
                inpFileName.Value = _container.name;
                inpFileName.Disabled = true;
            }
        }

        /// <summary>
        /// Creates radio button group for specified set with list of options.
        /// </summary>
        /// <param name="name">Name of set.</param>
        /// <param name="description">Description of set.</param>
        /// <param name="options">Set of options.</param>
        /// <returns>String with html-code of radio button group.</returns>
        private static string GetDictionaryHTML(string name, string description, IDictionary<string, string> options, string predefinedOption = null)
        {
            // We could use classes from System.Web.UI.HtmlControls namespace but Kentico encrypts some attributes of tags for them.

            using (var stringWriter = new StringWriter())
            {
                using (var html = new HtmlTextWriter(stringWriter))
                {
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
                    if ((options?.Count()).GetValueOrDefault() > 0)
                    {
                        html.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                        html.RenderBeginTag(HtmlTextWriterTag.Div);
                        bool isChecked = false;
                        foreach (var o in options)
                        {
                            var id = $"{name}{o.Key}Id";

                            html.AddAttribute(HtmlTextWriterAttribute.Class, "col-lg-4 col-xl-3");
                            html.RenderBeginTag(HtmlTextWriterTag.Div);

                            // <div class="input__wrapper">
                            if (string.IsNullOrWhiteSpace(predefinedOption))
                            {
                                html.AddAttribute(HtmlTextWriterAttribute.Class, "input__wrapper");
                            }
                            else
                            {
                                html.AddAttribute(HtmlTextWriterAttribute.Class, "input__wrapper input__wrapper--disabled");
                            }

                            html.RenderBeginTag(HtmlTextWriterTag.Div);

                            html.AddAttribute(HtmlTextWriterAttribute.Class, "input__radio");
                            html.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
                            html.AddAttribute(HtmlTextWriterAttribute.Name, name);
                            html.AddAttribute(HtmlTextWriterAttribute.Id, id);
                            html.AddAttribute(HtmlTextWriterAttribute.Value, o.Key);
                            if (string.IsNullOrWhiteSpace(predefinedOption))
                            {
                                if (!isChecked)
                                {
                                    html.AddAttribute(HtmlTextWriterAttribute.Checked, string.Empty);
                                    isChecked = true;
                                }
                            }
                            else
                            {
                                html.AddAttribute(HtmlTextWriterAttribute.Disabled, string.Empty);
                                if (predefinedOption.Equals(o.Key))
                                {
                                    html.AddAttribute(HtmlTextWriterAttribute.Checked, string.Empty);
                                    isChecked = true;
                                }
                            }
                            html.RenderBeginTag(HtmlTextWriterTag.Input);
                            html.RenderEndTag();

                            html.AddAttribute(HtmlTextWriterAttribute.Class, "input__label input__label--radio");
                            html.AddAttribute(HtmlTextWriterAttribute.For, id);
                            html.RenderBeginTag(HtmlTextWriterTag.Label);
                            html.Write(o.Value);
                            html.RenderEndTag();

                            html.RenderEndTag(); // </div class="input__wrapper">

                            html.RenderEndTag();
                        }
                        html.RenderEndTag();
                    }
                    html.RenderEndTag();
                    return stringWriter.ToString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (inpFile.PostedFile.ContentType == "application/vnd.ms-excel"
                || inpFile.PostedFile.ContentType == "text/csv")
            {
                try
                {
                    var fileStream = inpFile.PostedFile.InputStream;
                    var fileName = inpFileName.Value;

                    var fileId = ServiceHelper.UploadFile(fileStream, fileName);
                    var containerId = Guid.Empty;
                    if (_container == null)
                    {
                        var mailType = Request.Form[GetString("Kadena.MailingList.MailType")];
                        var product = Request.Form[GetString("Kadena.MailingList.Product")];
                        var validity = int.Parse(Request.Form[GetString("Kadena.MailingList.Validity")]);
                        containerId = ServiceHelper.CreateMailingContainer(fileName, mailType, product, validity);
                    }
                    else
                    {
                        containerId = new Guid(_container.id);
                        ServiceHelper.RemoveAddresses(containerId);
                    }

                    var nextStepUrl = GetStringValue("RedirectPage", string.Empty);
                    nextStepUrl = URLHelper.AddParameterToUrl(nextStepUrl, "containerid", containerId.ToString());
                    nextStepUrl = URLHelper.AddParameterToUrl(nextStepUrl, "fileid", fileId.ToString());
                    Response.Redirect(nextStepUrl);
                }
                catch (Exception exc)
                {
                    txtError.InnerText = exc.Message;
                    EventLogProvider.LogException("Mailing List Create", "EXCEPTION", exc, CurrentSite.SiteID);
                }
            }
        }
    }
}