using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wacton.Desu.Kanji;

namespace SugarKanji
{
    public class KanjiDict
    {
        public List<IKanjiEntry> kanji;
        public async Task Out()
        {
            kanji = (await KanjiDictionary.ParseEntriesAsync()).ToList();
        }
    }
}
