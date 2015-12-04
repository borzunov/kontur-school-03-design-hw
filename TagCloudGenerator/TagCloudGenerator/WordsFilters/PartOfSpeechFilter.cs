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

        public IEnumerable<string> Filter(IEnumerable<string> words,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return words
                .Where(word => grammarInfo.ContainsKey(word) &&
                               allowedPartsOfSpeech.Contains(grammarInfo[word].PartOfSpeech));
        }
    }
}
