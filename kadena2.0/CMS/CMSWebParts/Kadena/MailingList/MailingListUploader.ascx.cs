using CMS.PortalEngine.Web.UI;
using System;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class MailingListUploader : CMSAbstractWebPart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnHelp.Attributes["data-tooltip-title"] = GetString("Kadena.MailingList.HelpUpload");
            textFileToUpload.InnerText = GetString("Kadena.MailingList.FileToUpload");
            textOr.InnerText = GetString("Kadena.MailingList.Or");
            textSkipField.InnerText = GetString("Kadena.MailingList.SkipField");
        }
    }
}