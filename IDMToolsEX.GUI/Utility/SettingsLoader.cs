using System;
using System.IO;
using System.Text.Json;
using IDMToolsEX.Models;

namespace IDMToolsEX.Utility;

public class SettingsLoader
{
    private const string SettingsFileName = "settings.json";

    public void SaveSettings(Settings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        var filePath = Path.Combine(AppContext.BaseDirectory, SettingsFileName);
        File.WriteAllText(filePath, json);
    }

    public Settings LoadSettings()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, SettingsFileName);
        if (!File.Exists(filePath)) return new Settings(); // Return default settings if file doesn't exist

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
    }
}
