using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    class MostCommonWordsFilter : IWordsFilter
    {
        readonly int count;

        public MostCommonWordsFilter(Options options)
        {
            count = options.Count;
        }

        public WordsStatistics Filter(WordsStatistics statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return new WordsStatistics(statistics.OccurrencesCounts
                .OrderByDescending(pair => pair.Value)
                .Take(count)
                .ToDictionary(pair => pair.Key, pair => pair.Value));
        }
    }
}
