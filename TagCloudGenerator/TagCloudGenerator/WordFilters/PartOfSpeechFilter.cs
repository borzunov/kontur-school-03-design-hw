using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordFilters
{
    static class PartOfSpeechFilter
    {
        static readonly HashSet<PartOfSpeech> AllowedPartsOfSpeech = new HashSet<PartOfSpeech>
        {
            PartOfSpeech.Adjective,
            PartOfSpeech.Noun,
        };

        public static WordFilter GetFilter()
        {
            return (words, grammarInfo) => words
                .Where(word => grammarInfo.ContainsKey(word) &&
                               AllowedPartsOfSpeech.Contains(grammarInfo[word].PartOfSpeech));
        }
    }
}
