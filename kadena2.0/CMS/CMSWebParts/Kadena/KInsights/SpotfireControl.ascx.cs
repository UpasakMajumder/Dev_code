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
                ltSpotfire.Text = $@"<div class='col-lg-6'>
                                        <div id='spotfire-{Guid.NewGuid()}' data-url='{FileUrl}' class='spotfire__item js-spotfire'>
                                            <div class='spinner'>
                                                <svg class='icon '>
                                                    <use xlink:href='/gfx/svg/sprites/icons.svg#spinner' />
                                                </svg>
                                            </div>
                                        </div>
                                    </div>";
            }
        }
    }
}