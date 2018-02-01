using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class ProductOptions : CMSAbstractWebPart
    {
        public int SKUID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("SKUID"), 0);
            }
        }

        public string PriceElementName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PriceElementName"), string.Empty);
            }
        }

        public string PriceUrl
        {
            get
            {
                return URLHelper.ResolveUrl(ValidationHelper.GetString(GetValue("PriceUrl"), string.Empty));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}