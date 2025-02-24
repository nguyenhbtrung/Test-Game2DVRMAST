using UnityEngine;
using System.IO;

public static class TestLogger
{
    private static string logFilePath = Path.Combine(Application.dataPath, "TestLogs", "TrapTestResults.txt");
    private static string logDir = "TestLogs";
    public static string GetLogFilePath()
    {
        return logFilePath;
    }

    public static void Log(string message)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine(message);
        }
    }

    public static void Log(string message, string fileName)
    {
        string logFilePath = Path.Combine(Application.dataPath, logDir, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine(message);
        }
    }

    public static void Log(string message, string fileName, bool append = true)
    {
        string logFilePath = Path.Combine(Application.dataPath, logDir, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

        using (StreamWriter writer = new StreamWriter(logFilePath, append))
        {
            writer.WriteLine(message);
        }
    }

    public static void ClearLog()
    {
        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
        }
    }

    public static void ClearLog(string fileName)
    {
        string logFilePath = Path.Combine(Application.dataPath, logDir, fileName);
        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
        }
    }
}
