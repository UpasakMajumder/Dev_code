using CMS.EventLog;
using CMS.UIControls;
using Kadena.BusinessLogic.Services.SettingsSynchronization;
using Kadena.Models.SiteSettings.Synchronization;
using Kadena2.WebAPI.KenticoProviders.Providers;
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
                var service = new SettingsSynchronizationService(new KenticoSettingsProvider());
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