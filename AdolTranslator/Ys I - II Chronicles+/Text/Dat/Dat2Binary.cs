using Yarhl.FileFormat;
using Yarhl.IO;

namespace AdolTranslator.Text.Dat
{
    public class Dat2Binary : IConverter<Dat, BinaryFormat>
    {
        private DataWriter writer;
        private Dat dat;

        public BinaryFormat Convert(AdolTranslator.Text.Dat.Dat source)
        {
            writer = new DataWriter(new DataStream());
            dat = source;

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
                var bytes = Binary2Dat.Sjis.GetBytes(text + "\0");
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
    }
}
