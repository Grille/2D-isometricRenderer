using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Program;

class SettingsFile : IniFile
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

    protected override void OnRead()
    {
        string value;

        bool TryGetValue(string key)
        {
            return Default.TryGetValue(key, out value);
        }

        if (TryGetValue("fullscreen"))
            Fullscreen = Convert.ToBoolean(value);

        if (TryGetValue("width"))
            WindowWidth = Convert.ToInt32(value);

        if (TryGetValue("height"))
            WindowHeight = Convert.ToInt32(value);

        if (TryGetValue("directory"))
            Directory = DirectorySave = DirectoryImport = DirectoryExport = value;

        if (TryGetValue("directory_save"))
            DirectorySave = value;

        if (TryGetValue("directory_export"))
            DirectoryExport = value;

        if (TryGetValue("directory_import"))
            DirectoryImport = value;

        if (TryGetValue("default_texture"))
            DefaultTexture = value;

        if (TryGetValue("default_map"))
            DefaultMap = value;
    }
}
