using CMS.UIControls;
using Kadena.BusinessLogic.Contracts;
using Kadena2.Container.Default;
using System;

namespace Kadena.CMSModules.Kadena.Pages.SettingsSynchronization
{
    public partial class Code : CMSPage
    {
        public string RootCategoryName => "Kadena";

        protected void btnGetCode_Click(object sender, EventArgs e)
        {
            HideErrorMessage();

            var keyName = settingsKeyName.Value;
            var service = DIContainer.Resolve<ISettingsSynchronizationService>();
            try
            {
                var generatedCode = service.GetSettingsKeySourceCode(keyName);
                if (!string.IsNullOrEmpty(generatedCode))
                {
                    code.InnerText = generatedCode;
                }
                else
                {
                    ShowErrorMessage("Settings key was not found");
                }
            }
            catch (Exception ex)
            {
                // since this is meant to be used by developer we are showing raw message instead of some friendlier one
                ShowErrorMessage(ex.Message);
            }
        }

        private void ShowErrorMessage(string message)
        {
            errorMessageContainer.Visible = true;
            errorMessage.Text = message;
        }

        private void HideErrorMessage()
        {
            errorMessageContainer.Visible = false;
        }
    }
}