using AdolTranslator.Text.Dat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.Media.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AdolTranslator.Ys_I___II_Chronicles_.Text.Asn
{
    public class Asn2Po : IConverter<AdolTranslator.Text.Asn.Asn, Po>
    {
        public Po Convert(AdolTranslator.Text.Asn.Asn source)
        {
            //Read the language used by the user' OS, this way the editor can spellcheck the translation. - Thanks Liquid_S por the code
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var po = new Po
            {
                Header = new PoHeader("Ys I & II Chronicles+", "anyemail@gmail.com", currentCulture.Name)
                {
                    LanguageTeam = "Any"
                }
            };


            // For now, get all text from block 5
            var meme = source.Texts.OrderBy(x => x.sort);
            foreach (var sourceText in meme)
            {
                var text = sourceText.text;
                if (string.IsNullOrWhiteSpace(text))
                    text = "<!empty>";
                po.Add(new PoEntry(text)
                {
                    Context = $"Block: 5 | Entry: {sourceText.index}",
                    ExtractedComments = $"Byte1: {sourceText.byte1:X} | Byte2: {sourceText.byte2:X} | Byte3: {sourceText.byte3:X} | Sort: {sourceText.sort}"
                });
            }

            return po;
        }
    }
}
