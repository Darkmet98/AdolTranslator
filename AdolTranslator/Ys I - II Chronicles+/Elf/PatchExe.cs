using System.Collections.Generic;
using System.IO;
using ElfManipulator.Data;

namespace AdolTranslator.Elf
{
    class PatchExe
    {
        private Config config;
        public PatchExe(string exePath)
        {
            if (exePath.Contains("config"))
                InstanceSettingsConfig(exePath);
            else
                InstanceMainConfig(exePath);

            var apply = new CustomApplyTranslations(config);
            apply.GenerateElfPatched();
        }

        private void InstanceMainConfig(string exePath)
        {
            var dirPath = Path.GetDirectoryName(exePath);

            config = new Config()
            {
                ContainsFixedEntries = false,
                ElfPath = exePath,
                NewSize = 0x00100000,
                PoConfigs = new List<PoConfig>()
                {
                    new PoConfig()
                    {
                        EncodingId = 932,
                        SectionName = ".rdata",
                        PoPath = $"{dirPath}GUI 1.po",
                        CustomDictionary = true,
                        DictionaryPath = $"{dirPath}text.ini"
                    },
                    new PoConfig()
                    {
                        EncodingId = 1252,
                        SectionName = ".rdata",
                        PoPath = $"{dirPath}GUI 2.po",
                        CustomDictionary = false
                    },
                    new PoConfig()
                    {
                        EncodingId = 932,
                        SectionName = ".rdata",
                        PoPath = $"{dirPath}Diary.po",
                        CustomDictionary = true,
                        DictionaryPath = $"{dirPath}text2.ini"
                    },
                }
            };
        }

        private void InstanceSettingsConfig(string exePath)
        {
            var dirPath = Path.GetDirectoryName(exePath);

            config = new Config()
            {
                ContainsFixedEntries = false,
                ElfPath = exePath,
                NewSize = 0x00100000,
                PoConfigs = new List<PoConfig>()
                {
                    new PoConfig()
                    {
                        EncodingId = 1252,
                        SectionName = ".rdata",
                        PoPath = $"{dirPath}Config.po"
                    }
                }
            };
        }
    }
}
