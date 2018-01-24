using System;

namespace Kadena.Models.SiteSettings
{
    public class Category
    {
        public string DisplayName { get; }
        public string Name { get; }
        public string CategoryParentName { get; }

        public Category(string displayName, string name, string categoryParentName)
        {
            DisplayName = !string.IsNullOrWhiteSpace(displayName)
                ? displayName
                : throw new ArgumentException(nameof(displayName) + " can't be empty");
            Name = !string.IsNullOrWhiteSpace(name)
                ? name
                : throw new ArgumentException(nameof(name) + " can't be empty");
            CategoryParentName = !string.IsNullOrWhiteSpace(categoryParentName)
                ? categoryParentName
                : throw new ArgumentException(nameof(categoryParentName) + " can't be empty");
        }
    }
}