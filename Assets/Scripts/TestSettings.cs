using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

public static class TestSettings
{
    private static readonly string filePath = Path.Combine(Application.dataPath, "Editor/Data/SceneTestSettings.json");

    private static SceneSettings settings;

    static TestSettings()
    {
        LoadSettings();
    }

    private static void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            settings = JsonConvert.DeserializeObject<SceneSettings>(json);
        }
        else
        {
            settings = new SceneSettings();
        }
    }

    private static void SaveSettings()
    {
        string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public static int SceneIndex
    {
        get => settings.SceneIndex;
        set
        {
            settings.SceneIndex = value;
            SaveSettings();
        }
    }

    public static string TestLogFileName
    {
        get => settings.TestLogFileName;
        set
        {
            settings.TestLogFileName = value;
            SaveSettings();
        }
    }
}

public class SceneSettings
{
    public int SceneIndex { get; set; } = 0;
    public string TestLogFileName { get; set; } = "";
}
