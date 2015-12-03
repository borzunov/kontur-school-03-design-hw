using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    class MostCommonWordFilter : IWordFilter
    {
        readonly int count;

        public MostCommonWordFilter(Options options)
        {
            count = options.Count;
        }

        public WordStatistics Filter(WordStatistics statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return new WordStatistics(statistics.OccurrenceCounts
                .OrderByDescending(pair => pair.Value)
                .Take(count)
                .ToDictionary(pair => pair.Key, pair => pair.Value));
        }
    }
}
