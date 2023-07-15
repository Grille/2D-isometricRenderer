using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Collections;

namespace Program;

public class TexturePack : IReadOnlyList<Texture>
{
    Texture[] textures;

    public TexturePack(string code) { 
        Parse(code);
    }

    public Texture this[int index] => textures[index];

    public int Count => textures.Length;

    public int GetId(Color color)
    {
        for (int i = 0; i < Count; i++)
        {
            if (this[i].Key == color)
                return i;
        }
        return 0;
    }

    public static TexturePack FromFile(string path)
    {
        var code = File.ReadAllText(path);
        return new TexturePack(code);
    }

    void Parse(string code)
    {
        var lines = code.Split(new[] { '\n' });

        var list = new List<Texture>();

        Texture texture = null;
        foreach (var line in lines)
        {
            var tline = line.Trim();

            if (tline.Length == 0)
                continue;

            if (tline[0] == '[')
            {
                string name = tline.Trim(new[] {
                    '[', ']'
                });
                texture = new Texture()
                {
                    Name = name,
                };
                list.Add(texture);
                continue;
            }

            var split = tline.Split(new[] { ' ' }, 2);
            string op = split[0].Trim().ToLower();
            string value = split[1]?.Trim();

            if (op == "c")
            {
                var seg = parseSegment(value);
                texture.AddSegment(seg);
            }

            if (op == "k")
            {
                var seg = parseSegment(value);
                texture.Key = seg.Color;
            }
        }

        Console.WriteLine("To array");
        textures = list.ToArray();

        foreach (var tex in textures)
        {
            tex.FillData();
        }
    }

    private TextureSegment parseSegment(string line)
    {
        var seg = new TextureSegment()
        {
            A = 255,
            R = 0,
            G = 0,
            B = 0,
            Repeat = 1,
        };

        var split = line.Split('*');
        if (split.Length == 2)
        {
            int repeat = int.Parse(split[1].Trim());
            seg.Repeat = repeat;
        }
        var csplit = split[0].Trim().Trim(new[] { '(', ')' }).Split(',');

        if (csplit.Length >= 3)
        {
            seg.R = int.Parse(csplit[0]);
            seg.G = int.Parse(csplit[1]);
            seg.B = int.Parse(csplit[2]);
        }

        if (csplit.Length >= 4)
        {
            seg.A = int.Parse(csplit[3]);
        }

        return seg;
    }

    public IEnumerator<Texture> GetEnumerator()
    {
        return ((IEnumerable<Texture>)textures).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return textures.GetEnumerator();
    }
}
