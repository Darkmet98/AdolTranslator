using System;
using System.IO;
using System.Text;
using AdolTranslator.Containers.Dat;
using AdolTranslator.Elf;
using AdolTranslator.Image._256;
using AdolTranslator.Image.Waku0;
using AdolTranslator.Text.Dat;
using Yarhl.FileSystem;
using Yarhl.IO;
using Yarhl.Media.Text;
using Binary2Dat = AdolTranslator.Text.Dat.Binary2Dat;

namespace AdolTranslator
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.WriteLine("AdolTranslator 1.0 - A simple program for Ys fan-translations by Darkmet98.\n" +
                              "Thanks to Twn for ASM Hacks and explanations.\n" +
                              "Thanks to Kaplas for waku0 swizzling and C to C# code port.\n" +
                              "Thanks to Pleonex for Yarhl libraries.");

            if (args.Length > 3 || args.Length == 0)
            {
                Console.WriteLine("Usage: AdolTranslator File.Extension");
                return;
            }

            var extension = Path.GetExtension(args[0]);
            var name = Path.GetFileNameWithoutExtension(args[0]);
            switch (extension.ToUpper())
            {
                case ".PO":
                    var node = NodeFactory.FromFile(args[0]);
                    node.TransformWith(new Binary2Po()).TransformWith(new Po2Dat()).TransformWith(new Dat2Binary()).Stream.WriteTo($"{name}_new.DAT");
                    break;

                case ".DAT":
                    node = NodeFactory.FromFile(args[0]);
                    if (args[0].Contains("SCENA"))
                        node.TransformWith(new Binary2Dat()).TransformWith(new Dat2Po()).TransformWith(new Po2Binary())
                            .Stream.WriteTo($"{name}.po");
                    else
                    {
                        node.TransformWith(new Binary2DatContainer()).TransformWith(new DatContainer2NodeContainer());
                        var folder = Path.GetFileNameWithoutExtension(args[0]);
                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);

                        foreach (var child in node.Children)
                        {
                            child.Stream.WriteTo(folder + Path.DirectorySeparatorChar + child.Name);
                        }
                    }
                    break;
                case ".EXE":
                    var exePatch = new PatchExe(args[0]);
                    break;
                /*case ".256":
                    var pal = NodeFactory.FromFile("COLOR.PAL").TransformWith(new Binary2Palette())
                        .GetFormatAs<Palette>();
                    var image = NodeFactory.FromFile(args[0]).TransformWith(new Binary2Image256()
                    {
                        PalettePassed = pal
                    }).GetFormatAs<Image256>();
                    //image.Palette.ToWinPaletteFormat("paleta.pal", 0, false);
                    image.Pixels.CreateBitmap(image.Palette, 0).Save(args[0] + ".png");
                    break;*/
                case ".BIN":
                    if (args[0].Contains("WAKU0"))
                    {
                        var imageWaku = NodeFactory.FromFile(args[0]).TransformWith(new Binary2Waku0()).GetFormatAs<Waku0>();
                        imageWaku.Pixels.Save(args[0] + ".png");
                    }
                    else
                        throw new NotSupportedException();

                    break;
                case ".PNG":
                    if (args[0].Contains("WAKU0"))
                    {
                        var waku02Binary = new Waku02Binary(args[0]);
                        File.WriteAllBytes(args[0] + "_new.bin", waku02Binary.Convert());
                    }
                    else
                        throw new NotSupportedException();

                    break;
            }
            
        }
    }
}
