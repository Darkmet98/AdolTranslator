using System.Collections.Generic;
using System.Linq;
using Yarhl.FileFormat;

namespace AdolTranslator.Text.Asn
{
    public class Asn : IFormat
    {
        public string Magic = "ASN\0";
        public List<AsnBlock> Blocks;
        public List<AsnTexts> Texts;

        public Asn()
        {
            Blocks = new List<AsnBlock>();
            Texts = new List<AsnTexts>();
        }
    }

    public class AsnTexts
    {
        public int index;
        public short byte1;
        public short byte2;
        public short byte3;
        public string text;

        // Custom sort
        // Explaining, what the fuck is that, the sort from this game is very strange, is like reading a book from the right to the left, for example:
        // The normal sort is 1 2 3 4 5 6
        // But this game sort is 2 1 4 3 6 5
        public int sort;
    }

    public class AsnBlock
    {
        public int PositionBlock;
        public short Unknown1;
        public short Count;
        public short Unknown2;
        public short Unknown3;
        public int PositionStart;
        public List<int> Positions;
        public List<int> Sizes;
        public List<byte[]> Data;

        public AsnBlock()
        {
            Positions = new List<int>();
            Sizes = new List<int>();
            Data = new List<byte[]>();
        }
    }
}


/*
 *  Investigations
 *  Block 0 = Junk Data
 *  Block 1 = Item descriptions, menu entries and junk
 *  Block 2 = Junk Data
 *  Block 3 = Junk Data
 *  Block 4 = Junk Data
 *  Block 5 = All text from game
 */
