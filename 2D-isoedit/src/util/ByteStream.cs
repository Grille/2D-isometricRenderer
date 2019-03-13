using System;
using System.Collections.Generic;
using System.IO;


namespace GGL.IO
{
    public enum CompressMode
    {
        Auto = -1,
        None = 0,
        RLE = 1
    }
    public class ByteStream
    {
        private byte[] data;
        private int index;
        private int endIndex;
        public int Index
        {
            get { return index; }
            set
            {
                if (index > endIndex) endIndex = index;
                if (value > endIndex) endIndex = value;
                index = value;
            }
        }
        public int EndIndex
        {
            get
            {
                if (index > endIndex) endIndex = index;
                return endIndex;
            }
        }

        public ByteStream()
        {
            //datal[7] = 8;

            data = new byte[10000000];
            index = 0;
        }
        public ByteStream(string path)
        {
            data = File.ReadAllBytes(path);
            index = 0;// data.Length - 1;
            endIndex = data.Length;
        }
        public ByteStream(byte[] bytes)
        {
            data = bytes;
            index = 0;// data.Length - 1;
            endIndex = data.Length;
        }

        #region write
        public void WriteByte(byte input)
        {
            data[index++] = input;
        }
        public void WriteInt(int input)
        {
            byte[] value = BitConverter.GetBytes(input);
            data[index++] = value[0];
            data[index++] = value[1];
            data[index++] = value[2];
            data[index++] = value[3];
        }
        public void WriteFloat(float input)
        {
            byte[] value = BitConverter.GetBytes(input);
            data[index++] = value[0];
            data[index++] = value[1];
            data[index++] = value[2];
            data[index++] = value[3];
        }

        public void WriteByteArray(int[] input)
        {
            byte[] data = new byte[input.Length];
            for (int i = 0; i < input.Length; i++) data[i] = (byte)input[i];
            WriteByteArray(data, 0);
        }
        public void WriteByteArray(byte[] input)
        {
            WriteByteArray(input, 0);
        }
        public void WriteByteArray(byte[] input, CompressMode compressionMode)
        {
            if (compressionMode == CompressMode.Auto)
            {
                byte curValue = input[0];
                int changes = 1;
                for (int i = 1; i < input.Length; i++)
                {
                    if (input[i] != curValue)
                    {
                        changes++;
                        curValue = input[i];
                    }
                }
                float clutter = changes / (float)input.Length;
                if (clutter >= 0.5) compressionMode = CompressMode.None;
                else compressionMode = CompressMode.RLE;

            }
            if (input.Length < 256)
            {
                WriteByte((byte)(compressionMode + 4));
                WriteByte((byte)input.Length);
            }
            else
            {
                WriteByte((byte)compressionMode);
                WriteInt((int)input.Length);
            }

            switch (compressionMode)
            {
                case CompressMode.None:
                    for (int i = 0; i < input.Length; i++)
                    {
                        data[index++] = input[i];
                    }
                    break;
                case CompressMode.RLE:
                    byte curValue = input[0], curLength = 0;
                    for (int i = 1; i < input.Length; i++)
                    {
                        if (input[i] != curValue || curLength >= 255)
                        {
                            data[index++] = curLength;
                            data[index++] = curValue;
                            curValue = input[i]; curLength = 0;
                        }
                        else curLength++;
                    }
                    data[index++] = curLength;
                    data[index++] = curValue;
                    break;
            }
        }
        public void WriteIntArray(int[] input)
        {
            WriteInt((int)input.Length);
            for (int i = 0; i < input.Length; i++) WriteInt(input[i]);
        }
        public void WriteIntArray2D(int[,] input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            WriteInt(width);
            WriteInt(height);
            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = 0; iy < height; iy++)
                {
                    WriteInt(input[ix, iy]);
                }
            }
        }
        public void WriteFloatArray(float[] input)
        {
            WriteInt((int)input.Length);
            for (int i = 0; i < input.Length; i++) WriteFloat(input[i]);
        }
        public void WriteString(string input)
        {
            char[] stringData = input.ToCharArray();

            if (input.Length < 256)
            {
                WriteByte(0);
                WriteByte((byte)input.Length);
            }
            else
            {
                WriteByte(1);
                WriteInt((int)input.Length);
            }
            for (int i = 0; i < stringData.Length; i++)
            {
                data[index++] = (byte)stringData[i];
            }
        }
        public void WriteStringArray(string[] input)
        {
            WriteInt((int)input.Length);
            for (int i = 0; i < input.Length; i++) WriteString(input[i]);
        }
        #endregion

        #region read
        public byte ReadByte()
        {
            return data[index++];
        }
        public int ReadInt()
        {
            return (data[index++] << 0 | data[index++] << 8 | data[index++] << 16 | data[index++] << 24);
        }
        public float ReadFloat()
        {
            index += 4;
            return BitConverter.ToSingle(data, index - 4);
        }

        public byte[] ReadByteArray()
        {
            byte mode = ReadByte();
            int length;
            if (mode > 3)
            {
                length = ReadByte();
                mode -= 4;
            }
            else length = ReadInt();

            byte[] retData = new byte[length];

            if (mode == 0)
            {
                for (int i = 0; i < retData.Length; i++)
                {
                    retData[i] = data[index++];
                }
            }
            else if (mode == 1)
            {
                int curLength = 0;
                while (curLength < length)
                {
                    byte len = data[index++];
                    byte value = data[index++];
                    for (int i = 0; i < len + 1; i++)
                        retData[curLength + i] = value;
                    curLength += len + 1;
                }
            }
            else if (mode == 2)
            {
                for (int i = 0; i < retData.Length; i += 2)
                {
                    retData[i] = (byte)(data[index] >> 4);
                    retData[i + 1] = (byte)(data[index++] & 15);
                }
            }

            return retData;
        }
        public int[] ReadIntArray()
        {
            int length = ReadInt();
            int[] retData = new int[length];
            for (int i = 0; i < retData.Length; i++) retData[i] = ReadInt();
            return retData;
        }
        public int[,] ReadIntArray2D()
        {
            int width = ReadInt();
            int height = ReadInt();
            int[,] retData = new int[width, height];

            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = 0; iy < height; iy++)
                {
                    retData[ix, iy] = ReadInt();
                }
            }
            return retData;
        }
        public float[] ReadFloatArray()
        {
            int length = ReadInt();
            float[] retData = new float[length];
            for (int i = 0; i < retData.Length; i++) retData[i] = ReadFloat();
            return retData;
        }

        public string ReadString()
        {
            byte mode = ReadByte();
            int length;
            if (mode == 0) length = ReadByte();
            else length = ReadInt();

            char[] retData = new char[length];

            for (int i = 0; i < retData.Length; i++)
            {
                retData[i] = (char)data[index++];
            }

            return new string(retData);
        }
        public string[] ReadStringArray()
        {
            int length = ReadInt();
            string[] retData = new string[length];
            for (int i = 0; i < retData.Length; i++) retData[i] = ReadString();
            return retData;
        }
        #endregion

        private int testArraySize(Array addValue)
        {
            return 0;
            //if (index + addValue >= data.Length) resize(data.Length + addValue);
        }
        private void testSize(int addValue)
        {
            //if (index + addValue >= data.Length) resize(data.Length + addValue);
        }
        private void resize(int size)
        {
            byte[] newData = new byte[size];
            for (int i = 0; i < Math.Min(data.Length, size); i++)
            {
                newData[i] = data[i];
            }
            data = newData;
        }
        public void ResetIndex()
        {
            index = 0;
        }
        public byte[] GetBytes()
        {
            byte[] result = new byte[EndIndex];
            Array.Copy(data, result, EndIndex);
            return result;
        }
        public string GetString()
        {
            char[] saveData = new char[data.Length];
            for (int i = 0; i < saveData.Length; i++)
            {
                saveData[i] = (char)data[i];
            }
            return new string(saveData);
        }
        public void Save(string path)
        {
            resize(index);
            File.WriteAllBytes(path, data);
        }


    }
}
