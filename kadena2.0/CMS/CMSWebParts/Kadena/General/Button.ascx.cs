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

        protected void SetupControl()
        {
            if (!StopProcessing)
            {
                hlLink.NavigateUrl = Link;
                hlLink.Text = Text;
                hlLink.CssClass = "btn-action " + Type;

                if (Target.Equals("New window"))
                {
                    hlLink.Target = "_blank";
                }
            }
        }

        #endregion
    }
}