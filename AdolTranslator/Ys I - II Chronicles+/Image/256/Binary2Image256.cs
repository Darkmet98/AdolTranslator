using System.Collections.Generic;
using System.Drawing;
using Texim;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace AdolTranslator.Image._256
{
    class Binary2Image256 : IConverter<BinaryFormat, Image256>
    {
        public Palette PalettePassed { get; set; }
        public Image256 Convert(BinaryFormat source)
        {
            var result = new Image256();
            var reader = new DataReader(source.Stream);

            var width = reader.ReadInt32();
            var height = reader.ReadInt32();


            result.Pixels = new PixelArray
            {
                Width = width,
                Height = height,
            };

            result.Palette = PalettePassed;
            result.Pixels.SetData(
                reader.ReadBytes((int)reader.Stream.Length-0x8),
                PixelEncoding.HorizontalTiles,
                ColorFormat.Indexed_8bpp,
                new Size(width, height));

            return result;
        }
    }
}
