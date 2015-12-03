using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    class LengthFilter : IWordsFilter
    {
        readonly int minLength;

        public LengthFilter(Options options)
        {
            minLength = options.MinLength;
        }

        public WordsStatistics Filter(WordsStatistics statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return new WordsStatistics(statistics.OccurrencesCounts
                .Where(pair => pair.Key.Length >= minLength)
                .ToDictionary(pair => pair.Key, pair => pair.Value));
        }
    }
}
