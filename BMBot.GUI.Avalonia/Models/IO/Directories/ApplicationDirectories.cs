using System;
using System.IO;

namespace BMBot.GUI.Avalonia.Models.IO.Directories;

public static class ApplicationDirectories
{
#if DEBUG
    public static string ClientDataPath => 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                     "BMBot", "GUI", "Debug");
    #else
    public static string ClientDataPath => 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                     "BMBot", "GUI");
    #endif
    
    public static string LogsDataPath => Path.Combine(ClientDataPath, "Logs");

    public static string TempDocumentDataPath => Path.Combine(Path.GetTempPath(), "BMBot");

    public static void CreateRequiredDirectories()
    {
        // Logs data path is automatically created by ILogger. - Comment by M9 on 07/09/2024 @ 15:51:21
        Directory.CreateDirectory(ClientDataPath);
        Directory.CreateDirectory(TempDocumentDataPath);
    }

    public static void CleanUpTempFolder()
    {
        if ( Directory.Exists(TempDocumentDataPath) )
        {
            Directory.Delete(TempDocumentDataPath, true);
        }
    }
}