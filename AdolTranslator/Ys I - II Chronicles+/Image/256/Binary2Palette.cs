using System.Collections.Generic;
using System.Drawing;
using Texim;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace AdolTranslator.Image._256
{
    class Binary2Palette : IConverter<BinaryFormat, Palette>
    {
        public Palette Convert(BinaryFormat source)
        {
            var reader = new DataReader(source.Stream)
            {
                Stream = {Position = 0x18}
            };
            var palette = new List<Color>();

            
            do
            {
                var red = reader.ReadByte();
                var green = reader.ReadByte();
                var blue = reader.ReadByte();
                var alpha = reader.ReadByte();
                palette.Add(Color.FromArgb(alpha, red, green, blue));
            } while (!reader.Stream.EndOfStream);

            /*
             * var reader = new DataReader(source.Stream);
            var palettes = new List<Color[]>();
            var count = new int[2];
            // Skip for now the PAL check
            reader.Stream.Position += 8;

            for (int i = 0; i < count.Length; i++)
            {
                count[i] = reader.ReadInt32();
            }

            foreach (var i in count)
            {
                var palette = new List<Color>();
                for (int e = 0; e < i; e++)
                {
                    var red = reader.ReadByte();
                    var green = reader.ReadByte();
                    var blue = reader.ReadByte();
                    var alpha = reader.ReadByte();
                    palette.Add(Color.FromArgb(alpha, red, green, blue));
                }
                palettes.Add(palette.ToArray());
            }
            

            return new Palette(palettes.ToArray());

             */
            return new Palette(palette.ToArray());

        }
    }
}
