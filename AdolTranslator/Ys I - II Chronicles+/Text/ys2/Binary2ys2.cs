using System;
using System.IO;
using System.Linq;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace AdolTranslator.Text.ys2
{
    public class Binary2ys2 : IConverter<BinaryFormat, Ys2>
    {
        private DataReader reader;
        public Ys2 Convert(BinaryFormat source)
        {
            reader = new DataReader(source.Stream);

            var key = reader.ReadInt32();
            var count = reader.ReadInt32();
            var bytes = reader.ReadBytes(count);
            File.WriteAllBytes("result.test", DecryptYs2(key, count, bytes));


            throw new NotImplementedException();
        }


        private byte[] DecryptYs2(int key, int count, byte[] ori)
        {
            
            var result = ori.ToArray();
            var keylong = (ulong)key;

            for (var i = 0; i < count; i++)
            {
                keylong = keylong * 0x3d09;
                var op = (byte)((uint)keylong >> 0x10);
                result[i] = (byte)(result[i] - op);
            }

            return result;
        }
    }
}
