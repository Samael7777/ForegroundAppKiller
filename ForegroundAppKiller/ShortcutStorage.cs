using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace ForegroundAppKiller;

public class ShortcutStorage : IShortcutStorage
{
    private const string SettingsFilename = "Settings.json";

    public KeyboardShortcut GetShortcut()
    {
        if (!File.Exists(SettingsFilename))
            return new KeyboardShortcut();
        
        var settingsJson = File.ReadAllText(SettingsFilename);

        var shortcut = JsonConvert.DeserializeObject<KeyboardShortcut>(settingsJson);

        return shortcut ?? new KeyboardShortcut();
    }

    public void SaveShortcut(KeyboardShortcut shortcut)
    {

        var jsonData = JsonConvert.SerializeObject(shortcut, new StringEnumConverter());
        File.WriteAllText(SettingsFilename, jsonData);
    }
}