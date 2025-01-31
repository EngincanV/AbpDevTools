﻿using System.Text.Json;

namespace AbpDevTools.Configuration;
public static class ReplacementConfiguration
{
    public static string FolderPath => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "abpdev");
    public static string FilePath => Path.Combine(FolderPath, "replacements.json");
    public static Dictionary<string, ReplacementOption> GetOptions()
    {
        if (!Directory.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);

        var options = ReplacementOption.GetDefaults();
        if (File.Exists(FilePath))
        {
            options = JsonSerializer.Deserialize<Dictionary<string, ReplacementOption>>(File.ReadAllText(FilePath));
        }
        else
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(options, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }

        return options;
    }
}
