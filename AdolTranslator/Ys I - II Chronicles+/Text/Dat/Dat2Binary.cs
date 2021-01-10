using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace AdolTranslator.Text.Dat
{
    public class Dat2Binary : IConverter<Dat, BinaryFormat>
    {
        private DataWriter writer;
        private Dat dat;
        private Dictionary<string, string> map;

        private string dictionaryDir =
            $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}{Path.DirectorySeparatorChar}text.ini";

        public BinaryFormat Convert(Dat source)
        {
            writer = new DataWriter(new DataStream());
            dat = source;
            map = new Dictionary<string, string>();

            if (File.Exists(dictionaryDir))
                GenerateDictionary();

            FillHeader();
            WriteData();
            UpdateHeader();

            return new BinaryFormat(writer.Stream);
        }

        private void FillHeader()
        {
            writer.Write(dat.Count);
            writer.WriteTimes(0, (dat.Count + 1) * 4);
        }

        private void WriteData()
        {
            var currentPosition = (int) writer.Stream.Position;

            foreach (var text in dat.TextList)
            {
                if (text == "<!empty>")
                {
                    dat.SizesList.Add(-1);
                    continue;
                }

                var bytes = Binary2Dat.Sjis.GetBytes(ReplaceChars(text) + "\0");
                var encrypted = Binary2Dat.XorEncryption(bytes);
                writer.Write(encrypted);
                dat.SizesList.Add(encrypted.Length);
            }

            dat.DataSize = (int)writer.Stream.Position - currentPosition;
        }

        private void UpdateHeader()
        {
            writer.Stream.Position = 4;
            writer.Write(dat.DataSize);
            foreach (var size in dat.SizesList)
            {
                writer.Write(size);
            }
        }

        private void GenerateDictionary()
        {
            var textFile = File.ReadAllLines(dictionaryDir);
            foreach (var s in textFile)
            {
                var splitted = s.Split(' ');
                var utf = Encoding.GetEncoding(1252).GetString(GetBytesFromString(splitted[0]));
                var sjis = Binary2Dat.Sjis.GetString(GetBytesFromString(splitted[1]));
                map.Add(utf, sjis);
            }
        }

        private string ReplaceChars(string ori)
        {
            return map.Aggregate(ori, (current, key) => current.Replace(key.Key, key.Value));
        }

        private byte[] GetBytesFromString(string intText)
        {
            var list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(System.Convert.ToInt32(intText)));
            do
            {
                list.Remove(0);
            } while (list.Contains(0));
            list.Reverse();
            return list.ToArray();
        }
    }
}
