using CMS.Helpers;
using CMS.UIControls;
using System;

namespace Kadena.CMSFormControls
{
    public partial class S3Uploader : CMSPage
    {
        private const string SCRIPT_TEMPLATE = "<script type=\"text/javascript\">window.parent.photoUploadComplete('{0}', {1});</script>";

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
                lblMessage.Text = string.Format(SCRIPT_TEMPLATE, "Please specify a valid file.", "true");
            }
            
            
            
            {
                //Uploaded file is valid, now we can do whatever we like to do, copying it file system,
                //saving it in db etc.
                lnkFile.Text = inpFile.PostedFile.FileName;
                lnkFile.NavigateUrl = inpFile.PostedFile.FileName;
                //Your Logic goes here

            }

            //Now inject the script which will fire when the page is refreshed.
            //ClientScript.RegisterStartupScript(this.GetType(), "uploadNotify", script);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            
        }
    }
}