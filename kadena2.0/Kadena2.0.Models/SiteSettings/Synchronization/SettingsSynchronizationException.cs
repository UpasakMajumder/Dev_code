using System;

namespace Kadena.Models.SiteSettings.Synchronization
{
    public class SettingsSynchronizationException : Exception
    {
        public SettingsSynchronizationException(string message) : base(message)
        {
        }
    }
}