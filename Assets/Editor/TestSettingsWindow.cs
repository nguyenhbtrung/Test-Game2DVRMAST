using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;

public class TestSettingsWindow : EditorWindow
{
    private int sceneIndex = 0;
    private string[] sceneNames;

    [MenuItem("Window/Scene Test Settings")]
    public static void ShowWindow()
    {
        GetWindow<TestSettingsWindow>("Test Settings");
    }

    private void OnEnable()
    {
        sceneNames = EditorBuildSettings.scenes
            .Select(scene => Path.GetFileNameWithoutExtension(scene.path))
            .ToArray();
    }

    private void OnGUI()
    {
        GUILayout.Label("Chọn Scene để kiểm tra:", EditorStyles.boldLabel);

        if (sceneNames == null || sceneNames.Length == 0)
        {
            EditorGUILayout.HelpBox("Không có scene nào trong Build Settings.", MessageType.Warning);
            return;
        }

        sceneIndex = EditorGUILayout.Popup("Scene", sceneIndex, sceneNames);

        if (GUILayout.Button("Lưu"))
        {
            TestSettings.SceneIndex = sceneIndex;
            Debug.Log("Scene Index đã được lưu: " + sceneIndex);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Tạo TestLog mới"))
        {
            CreateNewTestLog();
        }
    }

    private void CreateNewTestLog()
    {
        string logsDirectory = Path.Combine(Application.dataPath, "TestLogs");
        if (!Directory.Exists(logsDirectory))
        {
            Directory.CreateDirectory(logsDirectory);
        }

        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"TestLog_{timestamp}.txt";
        string filePath = Path.Combine(logsDirectory, fileName);

        File.WriteAllText(filePath, $"Test Hitbox {timestamp}");

        TestSettings.TestLogFileName = fileName;
        Debug.Log("Đã tạo TestLog mới: " + fileName);
    }
}
