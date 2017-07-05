using CMS.PortalEngine.Web.UI;

namespace Kadena.CMSWebParts.Kadena.General
{
    public partial class RichMessage : CMSAbstractWebPart
    {
        #region Public properties

        public string HeaderText
        {
            get
            {
                return GetStringValue("HeaderText", string.Empty);
            }
        }

        public string Content
        {
            get
            {
                return GetStringValue("Content", string.Empty);
            }
        }

        public string PrimaryButtonText
        {
            get
            {
                return GetStringValue("PrimaryButtonText", string.Empty);
            }
        }

        public string SecondaryButtonText
        {
            get
            {
                return GetStringValue("SecondaryButtonText", string.Empty);
            }
        }

        public string SecondaryButtonUrl
        {
            get
            {
                return GetStringValue("SecondaryButtonUrl", string.Empty);
            }
        }

        public string SecondaryButtonTarget
        {
            get
            {
                return GetStringValue("SecondaryButtonTarget", string.Empty);
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
                ltlHeaderText.Text = HeaderText;
                ltlContent.Text = Content;
                ltlPrimaryButtonText.Text = PrimaryButtonText;

                if (!string.IsNullOrWhiteSpace(SecondaryButtonText) && !string.IsNullOrWhiteSpace(SecondaryButtonUrl))
                {
                    lnkSecondary.Text = SecondaryButtonText;
                    lnkSecondary.NavigateUrl = SecondaryButtonUrl;
                    lnkSecondary.Target = SecondaryButtonTarget;
                    lnkSecondary.Visible = true;
                }
            }
        }

        #endregion

    }
}