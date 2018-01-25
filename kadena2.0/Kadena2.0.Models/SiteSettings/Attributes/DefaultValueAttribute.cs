using System;

namespace Kadena.Models.SiteSettings.Attributes
{
    public class DefaultValueAttribute : Attribute
    {
        public string DefaultValue { get; }

        public DefaultValueAttribute(string defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}