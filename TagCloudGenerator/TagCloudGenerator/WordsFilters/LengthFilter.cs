using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    class LengthFilter : IWordFilter
    {
        readonly int minLength;

        public LengthFilter(Options options)
        {
            minLength = options.MinLength;
        }

        public WordStatistics Filter(WordStatistics statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return new WordStatistics(statistics.OccurrenceCounts
                .Where(pair => pair.Key.Length >= minLength)
                .ToDictionary(pair => pair.Key, pair => pair.Value));
        }
    }
}
