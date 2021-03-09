using Yarhl.FileFormat;
using Yarhl.IO;

namespace AdolTranslator.Containers.Dat
{
    public class Binary2DatContainer : IConverter<BinaryFormat, DatContainer>
    {
        private DatContainer datContainer;
        private DataReader reader;

        public DatContainer Convert(BinaryFormat source)
        {
            reader = new DataReader(source.Stream)
            {
                Stream = {Position = 0}
            };

            datContainer = new DatContainer();

            DumpHeader();
            DumpData();

            return datContainer;
        }

        private void DumpHeader()
        {
            // Read the first entry for knowing what is the end of header
            var end = reader.ReadInt32();
            var count = end / 4;
            datContainer.Positions.Add(end);

            // Start dumping the entire header
            for (int i = 1; i < count; i++)
            {
                datContainer.Positions.Add(reader.ReadInt32());
            }
        }

        private void DumpData()
        {
            var i = 0;
            foreach (var datPosition in datContainer.Positions)
            {
                var datDec = new Compression.DatDecompression();
                reader.Stream.Position = datPosition;
                var size = reader.ReadInt32();
                reader.Stream.Position -= 4;
                var dec = datDec.Decompression(size, reader.ReadBytes(size), (int) reader.Stream.Length);
                datContainer.Blocks.Add(dec);
                GetBlockInfo(dec.Length, i++);
            }
        }

        private void GetBlockInfo(int arrayLength, int i)
        {
            datContainer.Information = "DUMMY";
            return;
            int delW = 16;
            int delH = 8;
            int width = 0;
            int height = 0;
            int dresult;

            do
            {
                width += delW;
                height += delH;
                dresult = width * height;
                if (dresult > arrayLength)
                {
                    delW = 16;
                    delH = 16;
                    width = 0;
                    height = 0;
                }
            }
            while (dresult != (arrayLength));

            datContainer.Information += $"{i}.bin\nWIDTH:{width}\nHEIGHT:{height}\n\n";

        }
    }
}
