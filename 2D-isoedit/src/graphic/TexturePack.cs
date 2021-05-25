using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GGL.IO;

namespace Program
{
    public class TexturePack
    {
        public Texture[] Textures;
        public TexturePack()
        {
            Textures = new Texture[255];
            for (int i = 0; i < 255; i++)
                Textures[i] = new Texture("default",new byte[] { 2, 0, 1, 70, 100, 40, 255, 1, 100, 70, 40, 255 });
        }
        public void Load(string path)
        {
            try
            {
                //"../textures/default.tex"
                ByteStream bs = new ByteStream(path);
                bs.ResetIndex();
                int length = bs.ReadInt();
                Textures = new Texture[255];
                for (int i = 0; i < 255; i++)
                {
                    if (i < length)
                        Textures[i] = new Texture(bs.ReadString(), bs.ReadByteArray());
                }
            }
            catch (Exception e){
                Console.WriteLine(path + " " + e.Message);
            }
        }
        public void Save(string path)
        {
            ByteStream bs = new ByteStream();
            bs.ResetIndex();
            bs.WriteInt(Textures.Length);
            for (int i = 0; i < Textures.Length; i++)
            {
                bs.WriteString(Textures[i].Name);
                bs.WriteByteArray(Textures[i].Data);
            }
            bs.Save(path);
        }
    }
}
