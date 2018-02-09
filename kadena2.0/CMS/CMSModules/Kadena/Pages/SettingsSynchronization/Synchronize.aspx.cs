using CMS.EventLog;
using CMS.UIControls;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.SiteSettings.Synchronization;
using Kadena2.Container.Default;
using System;

namespace Kadena.CMSModules.Kadena.Pages.SettingsSynchronization
{
    public partial class Synchronize : CMSPage
    {
        public string RootCategoryName => "Kadena";

        protected void btnSynchronizeSettings_Click(object sender, EventArgs e)
        {
            try
            {
                var service = DIContainer.Resolve<ISettingsSynchronizationService>();
                var result = service.Synchronize();

                ShowSuccessMessage($"All done! Added {result.AddedCount} new key(s)");
            }
            catch (SettingsSynchronizationException ex)
            {
                ShowErrorMessage(ex.Message);
                EventLogProvider.LogException("Synchronize settings", "EXCEPTION", ex);
            }
        }

        private void ShowErrorMessage(string message)
        {
            successMessageContainer.Visible = false;
            errorMessageContainer.Visible = true;
            errorMessage.Text = message;
        }

        private void ShowSuccessMessage(string message)
        {
            errorMessageContainer.Visible = false;
            successMessageContainer.Visible = true;
            successMessage.Text = message;
        }
    }
}