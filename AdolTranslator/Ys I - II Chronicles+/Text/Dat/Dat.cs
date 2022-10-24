using System.Collections.Generic;
using Yarhl.FileFormat;

namespace AdolTranslator.Text.Dat
{
    public class Dat : IFormat
    {
        public int Count { get; set; }
        public int DataSize { get; set; }
        public List<int> SizesList { get; set; }
        public List<string> TextList { get; set; }

        public Dat()
        {
            SizesList = new List<int>();
            TextList = new List<string>();
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
