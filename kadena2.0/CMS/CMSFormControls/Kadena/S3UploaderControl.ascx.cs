using CMS.FormEngine.Web.UI;

namespace Kadena.CMSFormControls.Kadena
{
    // Solution taken from http://geekswithblogs.net/rashid/archive/2007/08/01/Create-An-Ajax-Style-File-Upload.aspx

    public partial class S3Uploader : FormEngineUserControl
    {
        public override object Value
        {
            get
            {
                return fldFileUrl.Value;
            }
            set
            {
                fldFileUrl.Value = value?.ToString();
            }
        }
    }
}