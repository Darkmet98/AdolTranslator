using System.Collections.Generic;
using Yarhl.FileFormat;

namespace AdolTranslator.Containers.SE.DAT
{
    public class SeDatContainer : IFormat
    {
        public int Count { get; set; }
        public List<int> Sizes { get; }
        public List<byte[]> Blocks { get; }

        public SeDatContainer()
        {
            Sizes = new List<int>();
            Blocks = new List<byte[]>();
        }
    }
}
