using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using System;
using System.Collections.Generic;

namespace Kadena.Old_App_Code.Kadena.Email
{
    public class EmailService
    {
        private readonly string _setUpPasswordUrlSettingKey = "KDA_SetUpPasswordURL";
        private readonly string _setUpPasswordUrlMacro = "SetUpPasswordUrl";

        private Dictionary<string, EmailTemplateInfo> templateCache = new Dictionary<string, EmailTemplateInfo>();

        private string GetSetUpPasswordUrl(Guid userCode, string siteName = null)
        {
            string setUpUrl;
            if (!string.IsNullOrEmpty(siteName))
            {
                setUpUrl = SettingsKeyInfoProvider.GetURLValue($"{siteName}.{_setUpPasswordUrlSettingKey}", string.Empty);
            }
            else
            {
                setUpUrl = SettingsKeyInfoProvider.GetURLValue(_setUpPasswordUrlSettingKey, string.Empty);
            }
            return URLHelper.AddParameterToUrl(setUpUrl, "h", userCode.ToString());
        }

        public void SendResetPasswordEmail(UserInfo user, string templateName, string siteName, MacroResolver resolver = null)
        {
            EmailTemplateInfo template;
            var templateKey = siteName + templateName;
            if (!templateCache.TryGetValue(templateKey, out template))
            {
                template = EmailTemplateProvider.GetEmailTemplate(templateName, siteName);
                templateCache[templateKey] = template;
            }

            SendResetPasswordEmail(user, template, siteName, resolver);
        }

        public void SendResetPasswordEmail(UserInfo user, EmailTemplateInfo template, string siteName, MacroResolver resolver = null)
        {
            var macroResolver = resolver ?? MacroResolver.GetInstance();
            macroResolver.SetNamedSourceData(_setUpPasswordUrlMacro, GetSetUpPasswordUrl(user.UserGUID, siteName));

            var fromAddress = !string.IsNullOrWhiteSpace(template.TemplateFrom) 
                ? template.TemplateFrom 
                : SettingsKeyInfoProvider.GetValue("CMSNoreplyEmailAddress", siteName);

            var message = new EmailMessage();
            message.EmailFormat = EmailFormatEnum.Both;
            message.From = fromAddress;
            message.Recipients = user.Email;
            message.Subject = macroResolver.ResolveMacros(template.TemplateSubject);
            message.Body = macroResolver.ResolveMacros(template.TemplateText);
            message.PlainTextBody = macroResolver.ResolveMacros(template.TemplatePlainText);

            EmailSender.SendEmail(siteName, message, sendImmediately: false);
        }
    }
}