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
                Stream = { Position = 0 }
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
            foreach (var datPosition in datContainer.Positions)
            {
                reader.Stream.Position = datPosition;
                var size = reader.ReadInt32();
                reader.Stream.Position -= 4;
                datContainer.Blocks.Add(reader.ReadBytes(size));
            }
        }
    }
}
