using System;

namespace Kadena.Models.SiteSettings.Attributes
{
    public class CategoryAttribute : Attribute
    {
        public string DisplayName { get; }
        public string Name { get; }
        public string ParentName { get; }

        public CategoryAttribute(string displayName, string name, string parentName)
        {
            DisplayName = displayName;
            Name = name;
            ParentName = parentName;
        }
    }
}