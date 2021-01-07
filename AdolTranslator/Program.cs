using System;
using System.IO;
using System.Text;
using AdolTranslator.Text.Dat;
using Yarhl.FileSystem;
using Yarhl.Media.Text;

namespace AdolTranslator
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            Console.WriteLine("AdolTranslator - A simple Ys translator By Darkmet98.");

            if (args.Length != 1)
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
                    node.TransformWith(new Binary2Dat()).TransformWith(new Dat2Po()).TransformWith(new Po2Binary()).Stream.WriteTo($"{name}.po");
                    break;
            }
            
        }
    }
}
