using Kadena.Models.SiteSettings;

public interface IKenticoSettingsProvider
{
    SettingsKey GetKeyByName(string name);
    void CreateKey(SettingsKey key);
    bool KeyExists(string name);

    void CreateCategory(Category category);
    bool CategoryExists(string name);

    void CreateGroup(Group group);
    bool GroupExists(string displayName, string categoryName);
}