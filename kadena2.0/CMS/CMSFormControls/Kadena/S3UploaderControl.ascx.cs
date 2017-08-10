using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.Modules;
using CMS.SiteProvider;
using CMS.UIControls;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kadena.CMSFormControls.Kadena
{
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

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}