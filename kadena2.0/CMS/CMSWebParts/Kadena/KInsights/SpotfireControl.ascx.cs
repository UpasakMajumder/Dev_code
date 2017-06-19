using CMS.PortalEngine.Web.UI;
using System;

namespace Kadena.CMSWebParts.Kadena.KInsights
{
    public partial class SpotfireControl : CMSAbstractWebPart
    {
        public string FileUrl
        {
            get { return GetStringValue("FileUrl", string.Empty); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
    }
}