using CMS.Base;
using CMS.Core;
using CMS.EmailEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.MacroEngine;
using CMS.OnlineForms;
using CMS.SiteProvider;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace Kadena.Old_App_Code.Kadena.Forms
{
    public class FormsHelper
    {
        public string GetNewGuidName(string extension)
        {
            string fileName = Guid.NewGuid().ToString();

            if (!string.IsNullOrEmpty(extension))
            {
                fileName = string.Format("{0}.{1}", fileName, extension.TrimStart('.'));
            }
            return fileName;
        }

        public void SaveFileToDisk(HttpPostedFile postedFile, string fileName, string filesFolderPath)
        {
            string path = filesFolderPath + fileName;
            DirectoryHelper.EnsureDiskPath(path, SystemContext.WebApplicationPhysicalPath);
            StorageHelper.SaveFileToDisk(path, (BinaryData)postedFile.InputStream, false);

            if (!WebFarmHelper.WebFarmEnabled)
            {
                WebFarmHelper.CreateIOTask("UPDATEBIZFORMFILE", path, (BinaryData)postedFile.InputStream, "updatebizformfile", SiteContext.CurrentSiteName, fileName);
            }
        }

        public void SendFormEmail(BizFormItem item, int attachmentsCount)
        {
            if (item.BizFormInfo != null)
            {
                MacroResolver resolver = MacroContext.CurrentResolver.CreateChild();
                resolver.SetAnonymousSourceData(item);
                resolver.Settings.EncodeResolvedValues = true;
                resolver.Culture = CultureHelper.GetPreferredCulture();

                string body = DataHelper.GetNotEmpty(item.BizFormInfo.FormEmailTemplate, string.Empty);
                body = resolver.ResolveMacros(body);

                EmailMessage message = new EmailMessage();
                message.From = item.BizFormInfo.FormSendFromEmail;
                message.Recipients = resolver.ResolveMacros(item.BizFormInfo.FormSendToEmail);
                message.Subject = resolver.ResolveMacros(item.BizFormInfo.FormEmailSubject);
                message.Body = URLHelper.MakeLinksAbsolute(body);
                message.EmailFormat = EmailFormatEnum.Html;

                for (int i = 1; i <= attachmentsCount; i++)
                {
                    Attachment attachment = GetAttachment(item.GetStringValue("File" + i, string.Empty));
                    if (attachment != null)
                    {
                        message.Attachments.Add(attachment);
                    }
                }
                EmailSender.SendEmail(message);
            }
        }

        public Attachment GetAttachment(string attachmentGuid)
        {
            if (string.IsNullOrEmpty(attachmentGuid))
            {
                return null;
            }
            string formFilesFolderPath = FormHelper.GetBizFormFilesFolderPath(SiteContext.CurrentSiteName, (string)null);
            string path = formFilesFolderPath + FormHelper.GetGuidFileName(attachmentGuid);
            if (File.Exists(path))
            {
                Attachment attachment = new Attachment(
                    FileStream.New(path, FileMode.Open, FileAccess.Read),
                    FormHelper.GetGuidFileName(attachmentGuid));
                return attachment;
            }
            return null;
        }

        public bool IsEmailValid(string email)
        {
            var regexText = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                            + "@"
                            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            Regex regex = new Regex(regexText);
            Match match = regex.Match(email);
            return match.Success;
        }
    }
}