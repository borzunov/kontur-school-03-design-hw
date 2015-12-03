using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    class PartsOfSpeechFilter : IWordsFilter
    {
        readonly HashSet<PartOfSpeech> allowedPartsOfSpeech = new HashSet<PartOfSpeech>
        {
            PartOfSpeech.Adjective,
            PartOfSpeech.Noun,
        };

        public WordsStatistics Filter(WordsStatistics statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return new WordsStatistics(statistics.OccurrencesCounts
                .Where(pair => grammarInfo.ContainsKey(pair.Key) &&
                               allowedPartsOfSpeech.Contains(grammarInfo[pair.Key].PartOfSpeech))
                .ToDictionary(pair => pair.Key, pair => pair.Value));
        }
    }
}
