using System.IO;
using System.Text;
using AdolTranslator.Text.Dat;
using Yarhl.Media.Text;

namespace AdolTranslator.Elf
{
    class CustomMapping : ElfManipulator.Functions.GenerateMapping
    {
        private bool useDic;
        public CustomMapping(byte[] elfOri, Po poPassed, Encoding encodingPassed, int memDiffPassed, bool containsFixedLength, string dictionaryPathPassed, bool customDictionary = false) : base(elfOri, poPassed, encodingPassed, memDiffPassed, containsFixedLength, dictionaryPathPassed, customDictionary)
        {
            if (File.Exists(dictionaryPathPassed))
            {
                useDic = true;
                Dat2Binary.GenerateDictionary(dictionaryPathPassed);
            }
                

        }

        public override string UseDictionary(string text)
        {
            return (useDic) ? Dat2Binary.ReplaceChars(text) : text;
        }
    }
}
