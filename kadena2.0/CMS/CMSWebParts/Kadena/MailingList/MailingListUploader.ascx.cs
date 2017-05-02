using CMS.IO;
using CMS.PortalEngine.Web.UI;
using System;
using System.Linq;
using System.Web.UI;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class MailingListUploader : CMSAbstractWebPart
    {
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
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }
    }
}