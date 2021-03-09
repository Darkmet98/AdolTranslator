using Texim;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace AdolTranslator.Image._256
{
    class iMAGE2562Binary : IConverter<PixelArray, BinaryFormat>
    {
        public DataReader OriginalFile { get; set; }
        public BinaryFormat Convert(PixelArray source)
        {

            var binary = new BinaryFormat();
            var writer = new DataWriter(binary.Stream);
            writer.Write(OriginalFile.ReadBytes(0x400));
            writer.Write(source.GetData());
            return binary;
        }


    }
}
