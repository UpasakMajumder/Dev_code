using CMS.UIControls;
using System;

namespace Kadena.CMSFormControls
{
    public partial class S3Uploader : CMSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                UploadFile();
            }
        }

        private void UploadFile()
        {
            if ((inpFile.PostedFile?.ContentLength ?? 0) == 0)
            {
                lblMessage.Text = GetString("Kadena.Admin.NotValidFile");
                lnkFile.NavigateUrl = string.Empty;
                lnkFile.Text = string.Empty;
                return;
            }

            lblMessage.Text = string.Empty;
            lnkFile.Text = inpFile.PostedFile.FileName;
            lnkFile.NavigateUrl = inpFile.PostedFile.FileName;
        }
    }
}