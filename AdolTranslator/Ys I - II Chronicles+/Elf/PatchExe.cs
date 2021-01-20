using System.Collections.Generic;
using System.IO;
using System.Text;
using AdolTranslator.Text.Dat;
using AsmResolver;
using TF.IO;
using Yarhl.FileSystem;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace AdolTranslator.Elf
{
    public class PatchExe
    {
        private DataWriter writer;
        /*
         * 0 = GUI 1|Config
         * 1 = GUI 2
         * 2 = Diary
         */
        private Po[] pos = new Po[3];
        private List<ElfData[]> data;

        private string elfPath;
        private long tradPointer;
        private WindowsAssembly peInfo;
        private bool configExe;

        public PatchExe(string exePath)
        {
            data = new List<ElfData[]>();
            Dat2Binary.GenerateDictionary();
            var path = Path.GetDirectoryName(exePath);
            if (!string.IsNullOrWhiteSpace(path))
                path += Path.DirectorySeparatorChar;
            elfPath = exePath;
            if (exePath.Contains("config"))
                configExe = true;

            InstanceData(path);
            ApplyTranslation();
            writer.Stream.WriteTo("meme2.exe");
        }

        private void InstanceData(string path)
        {
            var array = File.ReadAllBytes(elfPath);
            int count = 1;
            if (configExe)
                pos[0] = NodeFactory.FromFile($"{path}Config.po").TransformWith<Binary2Po>().GetFormatAs<Po>();
            else
            {
                pos[0] = NodeFactory.FromFile($"{path}GUI 1.po").TransformWith<Binary2Po>().GetFormatAs<Po>();
                pos[1] = NodeFactory.FromFile($"{path}GUI 2.po").TransformWith<Binary2Po>().GetFormatAs<Po>();
                pos[2] = NodeFactory.FromFile($"{path}Diary.po").TransformWith<Binary2Po>().GetFormatAs<Po>();
                count = 3;
            }
            
            
            for (int i = 0; i < count; i++)
            {
                var mapping = new GenerateMapping(array, pos[i]);
                var result = mapping.Search(configExe? 0x00401200 : 0);
                data.Add(result.ToArray());
            }

            tradPointer = array.Length + 100;
            
            var e = CreateExeFile();
            writer = new DataWriter(DataStreamFactory.FromStream(e))
            {
                DefaultEncoding = configExe? Encoding.GetEncoding(1252) : Binary2Dat.Sjis
            };
        }

        private void ApplyTranslation()
        {
            writer.Stream.Position = tradPointer;
            var translationSection = peInfo.GetSectionByName(".trad");
            var translationSectionBase = (int)(peInfo.NtHeaders.OptionalHeader.ImageBase +
                                                (translationSection.Header.VirtualAddress -
                                                 translationSection.Header.PointerToRawData));
            foreach (var elfData in data[0])
            {
                var newPosition = (int)writer.Stream.Position + translationSectionBase;
                if (!configExe)
                    writer.Write(Dat2Binary.ReplaceChars(elfData.Text));
                else
                    writer.Write(elfData.Text);

                writer.Stream.PushCurrentPosition();
                foreach (var dataPosition in elfData.positions)
                {
                    writer.Stream.Position = dataPosition;
                    writer.Write(newPosition);
                }
                writer.Stream.PopPosition();
            }

            if (configExe)
                return;

            foreach (var elfData in data[1])
            {
                var newPosition = (int)writer.Stream.Position + translationSectionBase;
                writer.Write(elfData.Text,true ,Encoding.GetEncoding(1252));
                writer.Stream.PushCurrentPosition();
                foreach (var dataPosition in elfData.positions)
                {
                    writer.Stream.Position = dataPosition;
                    writer.Write(newPosition);
                }
                writer.Stream.PopPosition();
            }
            Dat2Binary.GenerateDictionary("text2.ini");
            foreach (var elfData in data[2])
            {
                var newPosition = (int)writer.Stream.Position + translationSectionBase;
                writer.Write(Dat2Binary.ReplaceChars(elfData.Text));
                writer.Stream.PushCurrentPosition();
                foreach (var dataPosition in elfData.positions)
                {
                    writer.Stream.Position = dataPosition;
                    writer.Write(newPosition);
                }
                writer.Stream.PopPosition();
            }
        }

        // https://github.com/Kaplas80/TranslationFramework2/blob/059947615b1e671100f4471286dbc1f407866083/src/Plugins/YakuzaGame/Files/Exe/PEFile.cs
        private MemoryStream CreateExeFile()
        {
            peInfo = WindowsAssembly.FromFile(elfPath);
            var outputFs = new MemoryStream();
            using var inputFs = new FileStream(elfPath, FileMode.Open);
            using var input = new ExtendedBinaryReader(inputFs, Binary2Dat.Sjis);

            var output = new BinaryStreamWriter(outputFs);
            var writingContext = new WritingContext(peInfo, new BinaryStreamWriter(outputFs));

            var dosHeader = input.ReadBytes((int)peInfo.NtHeaders.StartOffset);
            output.WriteBytes(dosHeader);

            var ntHeader = peInfo.NtHeaders;
            ntHeader.FileHeader.NumberOfSections++;
            ntHeader.OptionalHeader.SizeOfImage += 0x00100000;

            ntHeader.Write(writingContext);

            var newSection = CreateTFSection(peInfo.SectionHeaders[peInfo.SectionHeaders.Count - 1], ntHeader.OptionalHeader.FileAlignment, ntHeader.OptionalHeader.SectionAlignment);
            peInfo.SectionHeaders.Add(newSection);

            foreach (var section in peInfo.SectionHeaders)
            {
                section.Write(writingContext);
            }

            foreach (var section in peInfo.SectionHeaders)
            {
                input.Seek(section.PointerToRawData, SeekOrigin.Begin);
                outputFs.Seek(section.PointerToRawData, SeekOrigin.Begin);

                var data = input.ReadBytes((int)section.SizeOfRawData);
                output.WriteBytes(data);
            }

            var bytes = new byte[0x00100000];
            output.WriteBytes(bytes);

            return outputFs;
        }

        private ImageSectionHeader CreateTFSection(ImageSectionHeader previous, uint fileAlignment, uint sectionAlignment)
        {
            var realAddress = previous.PointerToRawData + previous.SizeOfRawData;
            realAddress = Align(realAddress, fileAlignment);

            var virtualAddress = previous.VirtualAddress + previous.VirtualSize;
            virtualAddress = Align(virtualAddress, sectionAlignment);

            var sectionHeader = new ImageSectionHeader
            {
                Name = ".trad",
                Attributes = ImageSectionAttributes.MemoryRead |
                             ImageSectionAttributes.ContentInitializedData,
                PointerToRawData = realAddress,
                SizeOfRawData = 0x00100000,
                VirtualAddress = virtualAddress,
                VirtualSize = 0x00100000,
            };

            return sectionHeader;
        }

        private uint Align(uint value, uint align)
        {
            align--;
            return (value + align) & ~align;
        }
    }
}
