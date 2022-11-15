using System;
using System.IO;
using System.Linq;
using System.Text;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace AdolTranslator.Text.Asn
{
    public class Binary2Asn : IConverter<BinaryFormat, Asn>
    {
        private DataReader reader;
        private Asn asn;

        public Asn Convert(BinaryFormat source)
        {
            reader = new DataReader(source.Stream);
            asn = new Asn();

            var key = reader.ReadInt32();
            var count = reader.ReadInt32();
            var bytes = reader.ReadBytes(count);
            var decrypted = Decrypt(key, count, bytes);
            reader = new DataReader(DataStreamFactory.FromArray(decrypted, 0, decrypted.Length));

            ReadHeader();
            DumpBlocks();
            DumpText();

            return asn;
        }


        private void ReadHeader()
        {
            // Skip for now the initial header
            reader.Stream.Position = 0xC;

            // I think the header contains 6 blocks
            for (int i = 0; i < 6; i++)
            {
                var position = reader.ReadInt32();
                asn.Blocks.Add(new AsnBlock()
                {
                    PositionBlock = position
                });
            }
        }

        private void DumpBlocks()
        {
            for (int i = 0; i < asn.Blocks.Count; i++)
            {
                reader.Stream.Position = asn.Blocks[i].PositionBlock;
                asn.Blocks[i].Unknown1 = reader.ReadInt16();
                asn.Blocks[i].Count = reader.ReadInt16();
                asn.Blocks[i].Unknown2 = reader.ReadInt16();
                asn.Blocks[i].Unknown3 = reader.ReadInt16();
                asn.Blocks[i].PositionStart = (int)reader.Stream.Position + (asn.Blocks[i].Count * 4);
                for (int j = 0; j < asn.Blocks[i].Count; j++)
                {
                    asn.Blocks[i].Positions.Add(reader.ReadInt32() + asn.Blocks[i].PositionStart);
                }
            }

            for (int i = 0; i < asn.Blocks.Count; i++)
            {
                for (int j = 0; j < asn.Blocks[i].Count; j++)
                {
                    var position = asn.Blocks[i].Positions[j];
                    int nextPosition;
                    if (j == asn.Blocks[i].Count - 1)
                    {
                        if (i == asn.Blocks.Count - 1)
                            nextPosition = (int)reader.Stream.Length;
                        else
                        {
                            nextPosition = asn.Blocks[i + 1].Positions[0];
                        }
                    }
                    else
                    {
                        nextPosition = asn.Blocks[i].Positions[j+1];
                    }

                    reader.Stream.PushCurrentPosition();
                    reader.Stream.Position = position;
                    var size = nextPosition - position;
                    var bytes = reader.ReadBytes(size);
                    asn.Blocks[i].Data.Add(bytes);
                    asn.Blocks[i].Sizes.Add(size);

                    reader.Stream.PopPosition();
                }
            }
        }

        private void DumpText()
        {
            var block = asn.Blocks[5];
            var sort = 0;
            var meme = true;
            for (int i = 0; i < block.Count; i++)
            {
                /*reader.Stream.Position = block.Positions[i+3];
                asn.Texts.Add(new AsnTexts()
                {
                    byte1 = reader.ReadInt16(),
                    byte2 = reader.ReadInt16(),
                    byte3 = reader.ReadInt16(),
                    text = reader.ReadString(Encoding.GetEncoding("shift_jis")),
                    index = i+3,
                    sort = i+3
                });
                

                reader.Stream.Position = block.Positions[i+2];
                asn.Texts.Add(new AsnTexts()
                {
                    byte1 = reader.ReadInt16(),
                    byte2 = reader.ReadInt16(),
                    byte3 = reader.ReadInt16(),
                    text = reader.ReadString(Encoding.GetEncoding("shift_jis")),
                    index = i+2,
                    sort = i+2
                });
                i++;*/

                reader.Stream.Position = block.Positions[i];
                asn.Texts.Add(new AsnTexts()
                {
                    byte1 = reader.ReadInt16(),
                    byte2 = reader.ReadInt16(),
                    byte3 = reader.ReadInt16(),
                    text = reader.ReadString(Encoding.GetEncoding("shift_jis")),
                    index = i,
                    sort = i
                });
            }
        }

        private string GetDirtyText(byte[] array)
        {
            return System.Text.Encoding.UTF8.GetString(array);
        }

        private byte[] Decrypt(int key, int count, byte[] ori)
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
