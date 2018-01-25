using System;

namespace Kadena.Models.SiteSettings.Attributes
{
    public class EncodedDefinitionAttribute : Attribute
    {
        public string Definition { get; }

        public EncodedDefinitionAttribute(string definition)
        {
            Definition = definition;
        }
    }
}