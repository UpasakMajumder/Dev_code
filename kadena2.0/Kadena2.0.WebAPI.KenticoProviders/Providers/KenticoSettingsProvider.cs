using CMS.DataEngine;
using Kadena.Models.SiteSettings;
using System;
using System.Linq;

namespace Kadena2.WebAPI.KenticoProviders.Providers
{
    public class KenticoSettingsProvider : IKenticoSettingsProvider
    {
        private const int EmptyId = 0;

        public bool CategoryExists(string name)
        {
            var categoryId = BaseAbstractInfoProvider.GetId(SettingsCategoryInfo.OBJECT_TYPE, name);
            var categoryExists = categoryId != EmptyId;
            return categoryExists;
        }

        public void CreateCategory(Category category)
        {
            var parentCategory = SettingsCategoryInfoProvider.GetSettingsCategoryInfoByName(category.CategoryParentName);
            var newCategory = new SettingsCategoryInfo
            {
                CategoryDisplayName = category.DisplayName,
                CategoryName = category.Name,
                CategoryParentID = parentCategory.CategoryID,
                CategoryIsGroup = false,
                CategoryIsCustom = true,
                CategoryResourceID = parentCategory.CategoryResourceID
            };
            SettingsCategoryInfoProvider.SetSettingsCategoryInfo(newCategory);
        }

        public void CreateGroup(Group group)
        {
            var parentCategory = SettingsCategoryInfoProvider.GetSettingsCategoryInfoByName(group.Category.Name);
            var newGroup = new SettingsCategoryInfo
            {
                CategoryDisplayName = group.DisplayName,
                CategoryName = Guid.NewGuid().ToString(),
                CategoryParentID = parentCategory.CategoryID,
                CategoryIsGroup = true,
                CategoryIsCustom = true,
                CategoryResourceID = parentCategory.CategoryResourceID
            };
            SettingsCategoryInfoProvider.SetSettingsCategoryInfo(newGroup);
        }

        public void CreateKey(SettingsKey key)
        {
            var groupInfo = GetGroup(key.Group.DisplayName, key.Group.Category.Name);

            var newKey = new SettingsKeyInfo
            {
                KeyDisplayName = key.KeyDisplayName,
                KeyName = key.KeyName,
                KeyDescription = key.KeyDescription,
                KeyType = key.KeyType,
                KeyValue = key.KeyDefaultValue,
                KeyCategoryID = groupInfo.CategoryID,
                KeyDefaultValue = key.KeyDefaultValue,
                KeyEditingControlPath = key.KeyEditingControlPath,
                KeyExplanationText = key.KeyExplanationText,
                KeyFormControlSettings = key.KeyFormControlSettings,
                KeyValidation = key.KeyValidation
            };

            SettingsKeyInfoProvider.SetSettingsKeyInfo(newKey);
        }

        public SettingsKey GetKeyByName(string keyName)
        {
            var keyInfo = SettingsKeyInfoProvider.GetSettingsKeyInfo(keyName);
            if (keyInfo == null)
            {
                throw new ArgumentException($"'{keyName}' is not a valid key name");
            }

            var groupInfo = SettingsCategoryInfoProvider.GetSettingsCategoryInfo(keyInfo.KeyCategoryID);
            var categoryInfo = SettingsCategoryInfoProvider.GetSettingsCategoryInfo(groupInfo.CategoryParentID);
            var categoryParentInfo = SettingsCategoryInfoProvider.GetSettingsCategoryInfo(categoryInfo.CategoryParentID);

            var category = new Category(categoryInfo.CategoryDisplayName, categoryInfo.CategoryName, categoryParentInfo.CategoryName);
            var group = new Group(groupInfo.CategoryDisplayName, category);

            var key = new SettingsKey
            {
                KeyDefaultValue = keyInfo.KeyDefaultValue,
                KeyDescription = keyInfo.KeyDescription,
                KeyDisplayName = keyInfo.KeyDisplayName,
                KeyEditingControlPath = keyInfo.KeyEditingControlPath,
                KeyExplanationText = keyInfo.KeyExplanationText,
                KeyFormControlSettings = keyInfo.KeyFormControlSettings,
                KeyName = keyInfo.KeyName,
                KeyType = keyInfo.KeyType,
                KeyValidation = keyInfo.KeyValidation,
                Group = group
            };
            return key;
        }

        public bool GroupExists(string displayName, string categoryName)
        {
            var parentCategory = SettingsCategoryInfoProvider.GetSettingsCategoryInfoByName(categoryName);
            if (parentCategory == null)
            {
                throw new ArgumentException($"Parent category with name {categoryName} does not exist");
            }

            var groupExists = SettingsCategoryInfoProvider.GetSettingsCategories()
                .WhereEquals(nameof(SettingsCategoryInfo.CategoryParentID), parentCategory.CategoryID)
                .And()
                .WhereEquals(nameof(SettingsCategoryInfo.CategoryDisplayName), displayName)
                .Any();

            return groupExists;
        }

        private SettingsCategoryInfo GetGroup(string displayName, string categoryName)
        {
            var parentCategory = SettingsCategoryInfoProvider.GetSettingsCategoryInfoByName(categoryName);
            if (parentCategory == null)
            {
                throw new ArgumentException($"Parent category with name {categoryName} does not exist");
            }

            var group = SettingsCategoryInfoProvider.GetSettingsCategories()
                .WhereEquals(nameof(SettingsCategoryInfo.CategoryParentID), parentCategory.CategoryID)
                .And()
                .WhereEquals(nameof(SettingsCategoryInfo.CategoryDisplayName), displayName)
                .FirstOrDefault();
            if (group == null)
            {
                throw new ArgumentException($"Group with display name {displayName} does not exist");
            }

            return group;
        }

        public bool KeyExists(string name)
        {
            var keyId = BaseAbstractInfoProvider.GetId(SettingsKeyInfo.OBJECT_TYPE, name);
            var keyExists = keyId != EmptyId;
            return keyExists;
        }
    }
}
