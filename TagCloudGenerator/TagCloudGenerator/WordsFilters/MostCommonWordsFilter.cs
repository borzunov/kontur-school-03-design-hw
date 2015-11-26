using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.WordsFilters
{
    class MostCommonWordsFilter : IWordsFilter
    {
        readonly int count;

        public MostCommonWordsFilter(int count)
        {
            this.count = count;
        }

        public Dictionary<string, int> Filter(IReadOnlyDictionary<string, int> statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return statistics
                .OrderByDescending(pair => pair.Value)
                .Take(count)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
