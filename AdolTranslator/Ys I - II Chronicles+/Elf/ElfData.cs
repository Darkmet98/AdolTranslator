using System.Collections.Generic;

namespace AdolTranslator.Elf
{
    public class ElfData
    {
        public List<int> positions { get; set; }
        public string Text { get; set; }

        public ElfData()
        {
            positions = new List<int>();
        }
    }
}
