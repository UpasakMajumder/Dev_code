using CMS.PortalEngine.Web.UI;
using System;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class StatisticLabel : CMSAbstractWebPart
    {
        public string TitleText
        {
            get
            {
                return GetStringValue("TitleText", string.Empty);
            }
        }

        public string TitleCssClass
        {
            get
            {
                return GetStringValue("TitleCssClass", string.Empty);
            }
        }

        public string ValueCssClass
        {
            get
            {
                return GetStringValue("ValueCssClass", string.Empty);
            }
        }

        public string ValueEndpoint
        {
            get
            {
                return GetStringValue("ValueEndpoint", string.Empty);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            pValue.Attributes["class"] = ValueCssClass;
            pTitle.Attributes["class"] = TitleCssClass;

            pValue.InnerText = "0";
        }
    }
}