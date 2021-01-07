using Yarhl.FileFormat;
using Yarhl.Media.Text;

namespace AdolTranslator.Text.Dat
{
    public class Dat2Po : IConverter<Dat, Po>
    {
        public Po Convert(Dat source)
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

            var i = 0;
            foreach (var text in source.TextList)
            {
                po.Add(new PoEntry(text)
                {
                    Context = i++.ToString()
                });
            }

            return po;
        }
    }
}
