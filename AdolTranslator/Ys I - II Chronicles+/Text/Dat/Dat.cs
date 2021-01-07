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
