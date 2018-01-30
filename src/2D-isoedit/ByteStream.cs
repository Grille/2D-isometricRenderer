using System;
using System.IO;


namespace GGL.IO
{
    public class ByteStream
    {
        private byte[] data;
        private int index;

        public ByteStream()
        {
            data = new byte[10000000];
            index = 0;
        }
        public ByteStream(string path)
        {
            data = File.ReadAllBytes(path);
            index = 0;// data.Length - 1;
        }

        #region write
        public void WriteInt(int input)
        {
            testSize(4);
            byte[] value = BitConverter.GetBytes(input);
            data[index++] = value[0];
            data[index++] = value[1];
            data[index++] = value[2];
            data[index++] = value[3];
        }
        public void WriteByte(byte input)
        {
            testSize(1);
            data[index++] = input;
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
        public void WriteByteArray(byte[] input, int compressionMode)
        {
            testSize(input.Length);
            WriteByte((byte)compressionMode);
            WriteInt((int)input.Length);
            if (compressionMode == 0)//direct 8bit
            {
                for (int i = 0; i < input.Length; i++)
                {
                    data[index++] = input[i];
                }
            }
            if (compressionMode == 1)//compres 8bit
            {
                WriteString("begin [");
                int lastValue = input[0];
                int length = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    if (lastValue == input[i] && i < input.Length)
                    {
                        WriteString("-");
                        length++;
                    }
                    else
                    {
                        WriteString(" |" + length + "x" + lastValue);
                        //data[index++] = length;
                        //data[index++] = lastValue;
                        lastValue = input[i];
                        length = 0;
                    }
                }
                WriteString("] end");
            }
            else if (compressionMode == 2)//direct 4bit
            {
                for (int i = 0; i < input.Length; i += 2)
                {
                    data[index++] = (byte)(input[i] << 4 | input[i + 1]);
                }
            }
            else if (compressionMode == 3)//compres 4bit
            {
            }
        }

        public void WriteIntArray(byte[] input)
        {
            WriteInt((int)input.Length);
            for (int i = 0; i < input.Length; i++) WriteInt(input[i]);

        }
        public void WriteString(string input)
        {
            char[] stringData = input.ToCharArray();
            WriteInt(stringData.Length);
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
        public int ReadInt()
        {
            return (data[index++] << 0 | data[index++] << 8 | data[index++] << 16 | data[index++] << 24);
        }
        public byte ReadByte()
        {
            return data[index++];
        }
        public byte[] ReadByteArray()
        {
            byte mode = ReadByte();
            int length = ReadInt();
            byte[] retData = new byte[length];

            if (mode == 0)
            {
                for (int i = 0; i < retData.Length; i++)
                {
                    retData[i] = data[index++];
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
        public string ReadString()
        {
            int length = ReadInt();
            char[] retData = new char[length];

            for (int i = 0; i < retData.Length; i++)
            {
                retData[i] = (char)data[index++];
            }

            return new string(retData);
        }
        public int[] ReadIntArray()
        {
            int length = ReadInt();
            int[] retData = new int[length];
            for (int i = 0; i < retData.Length; i++) retData[i] = ReadInt();
            return retData;
        }
        public string[] ReadStringArray()
        {
            int length = ReadInt();
            string[] retData = new string[length];
            for (int i = 0; i < retData.Length; i++) retData[i] = ReadString();
            return retData;
        }
        #endregion

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
        public byte[] GetData()
        {
            return data;
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
