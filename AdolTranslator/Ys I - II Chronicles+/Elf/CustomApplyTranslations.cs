using System.Text;
using ElfManipulator.Data;
using ElfManipulator.Functions;
using Yarhl.Media.Text;

namespace AdolTranslator.Elf
{
    class CustomApplyTranslations : ApplyTranslation
    {
        public CustomApplyTranslations(Config configPassed) : base(configPassed)
        {
        }

        public override ElfData[] GenerateCustomMapping(byte[] elfArray, Po po, int encoding, int memDiff, bool containsFixedEntries,
            string dictionaryPath, bool customDictionary)
        {
            var map = new CustomMapping(elfArray, po, Encoding.GetEncoding(encoding), memDiff, containsFixedEntries, dictionaryPath, customDictionary);
            return map.Search().ToArray();
        }
    }
}
