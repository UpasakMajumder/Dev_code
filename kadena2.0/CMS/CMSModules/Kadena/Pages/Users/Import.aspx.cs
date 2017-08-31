using CMS.UIControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kadena.CMSModules.Kadena.Pages.Users
{
    public partial class Import : CMSPage
    {
        private readonly string importTemplateFile = "~/Resources/UserImportTemplate.xlsx";

        protected void Page_Load(object sender, EventArgs e)
        {
            Setup();
        }

        private void Setup()
        {
            btnDownloadTemplate.NavigateUrl = importTemplateFile;
        }

        protected void btnUploadUserList_Click(object sender, EventArgs e)
        {
            var file = importFile.PostedFile;

            // TODO: invoke processing of import file
        }
    }
}