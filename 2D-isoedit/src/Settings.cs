using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Program;

class Settings
{
    public string Directory = "./";
    public string DirectorySave = "./";
    public string DirectoryImport = "./";
    public string DirectoryExport = "./";

    public string DefaultTexture = "./";
    public string DefaultMap = "./";

    public int WindowWidth = 0;
    public int WindowHeight = 0;

    public bool Fullscreen = false;

    public bool Load(string path)
    {
        if (!File.Exists(path))
            return false;

        var lines = File.ReadAllLines(path);

        var settings = new SortedList<string, string>();
        foreach (var line in lines)
        {
            var split = line.Split(new char[] { '=' }, 2);
            if (split.Length == 2)
            {
                settings.Add(split[0].ToLower().Trim(), split[1].Trim());
            }
        }

        string value;

        if (settings.TryGetValue("fullscreen", out value))
            Fullscreen = Convert.ToBoolean(value);

        if (settings.TryGetValue("width", out value))
            WindowWidth = Convert.ToInt32(value);

        if (settings.TryGetValue("height", out value))
            WindowHeight = Convert.ToInt32(value);

        if (settings.TryGetValue("directory", out value))
            Directory = DirectorySave = DirectoryImport = DirectoryExport = value;

        if (settings.TryGetValue("directory_save", out value))
            DirectorySave = value;

        if (settings.TryGetValue("directory_export", out value))
            DirectoryExport = value;

        if (settings.TryGetValue("directory_import", out value))
            DirectoryImport = value;

        if (settings.TryGetValue("default_texture", out value))
            DefaultTexture = value;

        if (settings.TryGetValue("default_map", out value))
            DefaultMap = value;

        return true;
    }
}
