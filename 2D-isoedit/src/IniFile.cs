using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program;

class IniFile : Dictionary<string, Dictionary<string, string>>
{
    const string DefaultKey = "_default_";

    public Dictionary<string, string> Default => this[DefaultKey];

    public IniFile()
    {
        var section = new Dictionary<string, string>();
        Add(DefaultKey, section);
    }

    public void Load(string path)
    {
        using var reader = new StreamReader(path);
        Read(reader);
    }

    public void Save(string path)
    {
        using var writer = new StreamWriter(path);
        Write(writer);
    }

    public void Read(TextReader reader)
    {
        var section = Default;

        while (true)
        {
            var line = reader.ReadLine();
            if (line == null)
                break;

            var tline = line.Trim();
            if (tline[0] == '[')
            {
                if (!TryGetValue(tline, out section))
                {
                    section = new Dictionary<string, string>();
                    Add(tline, section);
                }
                continue;
            }

            var split = tline.Split(" ", 2, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length == 2)
            {
                var key = split[0];
                var value = split[1].Trim();
                section[key] = value;
                continue;
            }
        }

        OnRead();
    }

    public void Write(TextWriter writer)
    {
        OnWrite();

        foreach (var section in this)
        {
            writer.WriteLine($"[{section.Key}]");
            foreach (var pair in section.Value)
            {
                var key = pair.Key;
                var value = pair.Value;

                section.Value[key] = value;
            }
        }
    }

    protected virtual void OnRead()
    {

    }

    protected virtual void OnWrite()
    {

    }
}