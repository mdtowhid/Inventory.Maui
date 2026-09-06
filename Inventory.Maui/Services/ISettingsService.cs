using Microsoft.Maui.Storage;

namespace Inventory.Maui.Services;

public interface ISettingsService
{
    string GetString(string key, string defaultValue = "");
    void SetString(string key, string value);
    bool GetBool(string key, bool defaultValue = false);
    void SetBool(string key, bool value);
    void Clear();
}

public class SettingsService : ISettingsService
{
    public string GetString(string key, string defaultValue = "")
    {
        return Preferences.Get(key, defaultValue);
    }

    public void SetString(string key, string value)
    {
        Preferences.Set(key, value);
    }

    public bool GetBool(string key, bool defaultValue = false)
    {
        return Preferences.Get(key, defaultValue);
    }

    public void SetBool(string key, bool value)
    {
        Preferences.Set(key, value);
    }

    public void Clear()
    {
        Preferences.Clear();
    }
}