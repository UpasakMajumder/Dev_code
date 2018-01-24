using System;

namespace Kadena.Models.SiteSettings
{
    public class Group
    {
        public Category Category { get; set; }

        public string DisplayName { get; }

        public Group(string displayName, Category category)
        {
            DisplayName = !string.IsNullOrWhiteSpace(displayName)
                ? displayName
                : throw new ArgumentException(nameof(displayName) + " can't be empty");
            Category = category ?? throw new ArgumentNullException(nameof(category));
        }
    }
}