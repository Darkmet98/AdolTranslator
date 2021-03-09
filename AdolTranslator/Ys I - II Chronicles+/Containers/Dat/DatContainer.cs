using System.Collections.Generic;
using Yarhl.FileFormat;

namespace AdolTranslator.Containers.Dat
{
    public class DatContainer : IFormat
    {
        public List<int> Positions { get; }
        public List<byte[]> Blocks { get; }
        public string Information { get; set; }

        public DatContainer()
        {
            Positions = new List<int>();
            Blocks = new List<byte[]>();
        }
    }
}
