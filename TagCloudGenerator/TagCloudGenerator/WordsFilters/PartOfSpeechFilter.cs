using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    class PartOfSpeechFilter : IWordFilter
    {
        readonly HashSet<PartOfSpeech> allowedPartsOfSpeech = new HashSet<PartOfSpeech>
        {
            PartOfSpeech.Adjective,
            PartOfSpeech.Noun,
        };

        public WordStatistics Filter(WordStatistics statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return new WordStatistics(statistics.OccurrenceCounts
                .Where(pair => grammarInfo.ContainsKey(pair.Key) &&
                               allowedPartsOfSpeech.Contains(grammarInfo[pair.Key].PartOfSpeech))
                .ToDictionary(pair => pair.Key, pair => pair.Value));
        }
    }
}
