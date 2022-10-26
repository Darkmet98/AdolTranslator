using System.Collections.Generic;
using System.Linq;
using Yarhl.FileFormat;

namespace AdolTranslator.Text.Asn
{
    public class Asn : IFormat
    {
        public string Magic = "ASN\0";
        public List<AsnBlock> Blocks;

        public Asn()
        {
            Blocks = new List<AsnBlock>();
        }
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
        public List<string> Texts;

        public AsnBlock()
        {
            Positions = new List<int>();
            Texts = new List<string>();
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
