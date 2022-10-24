using Yarhl.FileFormat;
using Yarhl.IO;

namespace AdolTranslator.Containers.SE.DAT
{
    public class Binary2SeDatContainer : IConverter<BinaryFormat, SeDatContainer>
    {
        private SeDatContainer datContainer;
        private DataReader reader;

        public SeDatContainer Convert(BinaryFormat source)
        {
            reader = new DataReader(source.Stream)
            {
                Stream = {Position = 0}
            };

            datContainer = new SeDatContainer();

            DumpData();

            return datContainer;
        }

        private void DumpData()
        {
            datContainer.Count = reader.ReadInt32();

            for (int i = 0; i < datContainer.Count; i++)
            {
                datContainer.Sizes.Add(reader.ReadInt32());
            }

            for (int i = 0; i < datContainer.Count; i++)
            {
                datContainer.Blocks.Add(reader.ReadBytes(datContainer.Sizes[i]));
            }
        }
    }
}
