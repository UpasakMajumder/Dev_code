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
                return GetStringValue("TitleText", "Sent in 90 days");
            }
        }

        public string TitleCssClass
        {
            get
            {
                return GetStringValue("TitleCssClass", "");
            }
        }

        public string ValueCssClass
        {
            get
            {
                return GetStringValue("ValueCssClass", "");
            }
        }

        public string ValueEndpoint
        {
            get
            {
                return GetStringValue("ValueEndpoint", "");
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