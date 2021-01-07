using Yarhl.FileFormat;
using Yarhl.Media.Text;

namespace AdolTranslator.Text.Dat
{
    public class Po2Dat : IConverter<Po, Dat>
    {
        public Dat Convert(Po source)
        {
            var dat = new Dat()
            {
                Count = source.Entries.Count
            };

            foreach (var entry in source.Entries)
            {
                dat.TextList.Add(entry.Text);
            }

            return dat;
        }
    }
}
