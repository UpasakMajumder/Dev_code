using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.Localization;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.SiteProvider;
using Kadena.Dto.General;
using Kadena.Models.Common;
using Kadena.Old_App_Code.Kadena.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace Kadena.CMSPages.Kadena
{
    public class SubmitContactUsForm : IHttpHandler
    {
        #region static members

        public const string _ContactUsFormCodeName = "KDA_ContactUsForm";

        #endregion

        #region Public members

        public void ProcessRequest(HttpContext context)
        {
            var result = new GeneralResultDTO();
            context.Response.ContentType = ContentTypes.Json;

            string fullName = context.Request.Form["fullName"];
            string companyName = context.Request.Form["companyName"];
            string email = context.Request.Form["email"];
            string phone = context.Request.Form["phone"];
            string message = context.Request.Form["message"];
            var files = new List<HttpPostedFile>();
            if (context.Request.Files.Count > 0)
            {
                for (int i = 0; context.Request.Files.Count > i; i++)
                {
                    files.Add(context.Request.Files[i]);
                }
            }

            #region Validation

            if (string.IsNullOrWhiteSpace(fullName))
            {
                result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ContactForm.EnterYourFullName", LocalizationContext.CurrentCulture.CultureCode) };
                context.Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            if (!string.IsNullOrWhiteSpace(email) && !new FormsHelper().IsEmailValid(email))
            {
                result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ContactForm.EmailAddressIsNotValid", LocalizationContext.CurrentCulture.CultureCode) };
                context.Response.Write(JsonConvert.SerializeObject(result));
                return;

            }
            if (string.IsNullOrWhiteSpace(message))
            {
                result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ContactForm.EnterYourRequest", LocalizationContext.CurrentCulture.CultureCode) };
                context.Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            if (files.Count > 0)
            {
                if (files.Count > 4)
                {
                    result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ContactForm.NumberOfAttachmentsIsTooBig", LocalizationContext.CurrentCulture.CultureCode) };
                    context.Response.Write(JsonConvert.SerializeObject(result));
                    return;
                }
                int filesTotalSize = 0;

                foreach (var requestFile in files)
                {
                    filesTotalSize += requestFile.ContentLength;
                }
                if (filesTotalSize > 10000000)
                {
                    result = new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ContactForm.TotalAttachmentsSizeIsTooBig", LocalizationContext.CurrentCulture.CultureCode) };
                    context.Response.Write(JsonConvert.SerializeObject(result));
                    return;
                }
            }

            #endregion

            result = SubmitContactUsFormInternal(fullName, companyName, email, phone, message, files);
            context.Response.Write(JsonConvert.SerializeObject(result));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Private methods

        private GeneralResultDTO SubmitContactUsFormInternal(string fullName, string companyName, string email, string phone, string message, List<HttpPostedFile> files)
        {
            var contactUsForm = BizFormInfoProvider.GetBizFormInfo(_ContactUsFormCodeName, SiteContext.CurrentSiteID);
            if (contactUsForm != null)
            {
                var contactUsFormClass = DataClassInfoProvider.GetDataClassInfo(contactUsForm.FormClassID);
                string contactUsFormClassName = contactUsFormClass.ClassName;

                BizFormItem newFormItem = BizFormItem.New(contactUsFormClassName);

                newFormItem.SetValue("FullName", fullName);
                newFormItem.SetValue("CompanyName", companyName);
                newFormItem.SetValue("Email", email);
                newFormItem.SetValue("Phone", phone);
                newFormItem.SetValue("Message", message);
                newFormItem.SetValue("Site", SiteContext.CurrentSite.DisplayName);
                newFormItem.SetValue("User", MembershipContext.AuthenticatedUser.UserName);
                newFormItem.SetValue("FormInserted", DateTime.Now);
                newFormItem.SetValue("FormUpdated", DateTime.Now);

                if (files.Count > 0)
                {
                    for (int i = 0; files.Count > i; i++)
                    {
                        string extension = System.IO.Path.GetExtension(files[i].FileName);
                        string fileName = new FormsHelper().GetNewGuidName(extension);
                        string formFilesFolderPath = FormHelper.GetBizFormFilesFolderPath(SiteContext.CurrentSiteName);
                        string fileNameString = fileName + "/" + Path.GetFileName(files[i].FileName);
                        new FormsHelper().SaveFileToDisk(files[i], fileName, formFilesFolderPath);
                        newFormItem.SetValue(string.Format("File{0}", i + 1), (object)fileNameString);
                    }
                }
                newFormItem.Insert();

                new FormsHelper().SendFormEmail(newFormItem, files.Count);

                return new GeneralResultDTO { success = true };
            }
            else
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ContactForm.ContactFormRepositoryNotFound", LocalizationContext.CurrentCulture.CultureCode) };
            }
        }

        #endregion

    }
}