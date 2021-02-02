using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using AdolTranslator.Containers.Dat;
using AdolTranslator.Elf;
using AdolTranslator.Text.Dat;
using Yarhl.FileSystem;
using Yarhl.Media.Text;
using Binary2Dat = AdolTranslator.Text.Dat.Binary2Dat;

namespace AdolTranslator
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.WriteLine("AdolTranslator - A simple Ys translator By Darkmet98.");

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
            }
            
        }

        public static void Decompress(string path)
        {
            using (FileStream originalFileStream = new FileStream(path, FileMode.Append))
            {
                using (FileStream decompressedFileStream = File.Create(path + ".dev"))
                {
                    using (DeflateStream decompressionStream = new DeflateStream(originalFileStream, CompressionMode.Compress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        
                    }
                }
            }
        }
    }
}
