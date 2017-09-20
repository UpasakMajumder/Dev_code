using CMS.PortalEngine.Web.UI;

namespace Kadena.CMSWebParts.Kadena.General
{
    public partial class Button : CMSAbstractWebPart
    {
        #region Public properties

        public string Link
        {
            get
            {
                return GetStringValue("Link", string.Empty);
            }
        }

        public string LinkParameters
        {
            get
            {
                return GetStringValue("LinkParameters", string.Empty);
            }
        }

        public string Text
        {
            get
            {
                return GetStringValue("Text", string.Empty);
            }
        }

        public string Type
        {
            get
            {
                return GetStringValue("Type", string.Empty);
            }
        }

        public string Target
        {
            get
            {
                return GetStringValue("Target", string.Empty);
            }
        }

        #endregion

        #region Public methods

        public override void OnContentLoaded()
        {
            base.OnContentLoaded();
            SetupControl();
        }

        #endregion

        protected void SetupControl()
        {
            if (!StopProcessing)
            {
                hlLink.NavigateUrl = CombineLinkAndParameters(Link, LinkParameters);
                hlLink.Text = Text;
                hlLink.CssClass = "btn-action " + Type;

                if (Target.Equals("New window"))
                {
                    hlLink.Target = "_blank";
                }
            }
        }

        private string CombineLinkAndParameters(string link, string parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                var finalLink = link.TrimEnd('/').TrimEnd('?');

                var containsParameters = link.Contains("?");
                finalLink += containsParameters
                    ? "&"
                    : "?";

                finalLink += parameters;
                return finalLink;
            }

            return link;
        }
    }
}