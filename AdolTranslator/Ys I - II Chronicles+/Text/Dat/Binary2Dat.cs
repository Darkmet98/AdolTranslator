using System.IO;
using System.Text;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace AdolTranslator.Text.Dat
{
    public class Binary2Dat : IConverter<BinaryFormat, Dat>
    {
        private Dat dat;
        private DataReader reader;
        public static Encoding Sjis = Encoding.GetEncoding("shift_jis");

        public AdolTranslator.Text.Dat.Dat Convert(BinaryFormat source)
        {
            dat = new AdolTranslator.Text.Dat.Dat();
            reader = new DataReader(source.Stream) {Stream = {Position = 0}};

            ReadHeader();
            DumpText();

            return dat;
        }

        private void ReadHeader()
        {
            dat.Count = reader.ReadInt32();
            dat.DataSize = reader.ReadInt32();

            for (int i = 0; i < dat.Count; i++)
            {
                dat.SizesList.Add(reader.ReadInt32());
            }
        }

        private void DumpText()
        {
            //reader.Stream.Position = dat.DataSize;
            for (int i = 0; i < dat.Count; i++)
            {
                var size = dat.SizesList[i];
                if (size == -1)
                {
                    dat.TextList.Add("<!empty>");
                    continue;
                }
                
                var bytes = reader.ReadBytes(size);
                var decrypted = XorEncryption(bytes);
                //DebugBytes(decrypted, i);
                dat.TextList.Add(Sjis.GetString(decrypted).Replace("\0", ""));
            }
        }

        public static byte[] XorEncryption(byte[] array)
        {
            var key = 0xA5; //Xor Key — Thanks Megaflan

            var result = array;

            for (int i = 0; i < array.Length; i++)
            {
                result[i] = (byte)(result[i] ^ key);//Decrypt
            }

            return result;
        }

        private void DebugBytes(byte[] array, int index)
        {
            if (!Directory.Exists("debug"))
                Directory.CreateDirectory("debug");
            File.WriteAllBytes($"debug{Path.DirectorySeparatorChar}{index}.dec", array);
        }
    }
}
