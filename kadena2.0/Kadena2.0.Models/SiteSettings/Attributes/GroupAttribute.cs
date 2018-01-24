using System;

namespace Kadena.Models.SiteSettings.Attributes
{
    public class GroupAttribute : Attribute
    {
        public string DisplayName { get; }

        public GroupAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}