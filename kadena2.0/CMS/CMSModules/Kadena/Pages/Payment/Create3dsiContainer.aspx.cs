using CMS.SiteProvider;
using CMS.UIControls;
using System;

namespace Kadena.CMSModules.Kadena.Pages.Payment
{
    public partial class Create3dsiContainer : CMSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var site = SiteContext.CurrentSiteName;
            var code = tbCode.Text;
        }
    }
}