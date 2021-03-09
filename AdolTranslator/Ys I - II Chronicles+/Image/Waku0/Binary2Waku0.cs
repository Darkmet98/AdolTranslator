using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace AdolTranslator.Image.Waku0
{
    class Binary2Waku0 : IConverter<BinaryFormat, Waku0>
    {
        public Waku0 Convert(BinaryFormat source)
        {
            var result = new Waku0();
            var reader = new DataReader(source.Stream);

            var width = 256;
            var height = 256;

            var bytes = reader.ReadBytes((int) reader.Stream.Length);
            var swizzle = Unswizzling.Unswizzle(bytes, false);
            //File.WriteAllBytes("test.bin",Unswizzling.Unswizzle(uns, true));
            //File.WriteAllBytes("test1.bin", swizzle);

            result.Pixels = ConvertArray(swizzle, width, height);

            return result;
        }

        // Kaplas code
        private Bitmap ConvertArray(byte[] data, int imageWidth, int imageHeight)
        {
            unsafe
            {
                fixed (byte* ptr = data)
                {
                    var image = new Bitmap(imageWidth, imageHeight, imageWidth * 4, PixelFormat.Format32bppArgb, new IntPtr(ptr));
                    return image;
                }
            }
        }


    }
}
