using System.IO;

using BMBot.GUI.Avalonia.Models.IO.Directories;

namespace BMBot.GUI.Avalonia.Models.IO.Files;

public static class ApplicationFiles
{
    public static string LogsFilePath => Path.Combine(ApplicationDirectories.LogsDataPath, "activity.log");
}