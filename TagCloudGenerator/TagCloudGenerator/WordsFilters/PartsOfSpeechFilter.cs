﻿using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.WordsFilters
{
    class PartsOfSpeechFilter : IWordsFilter
    {
        readonly HashSet<PartOfSpeech> allowedPartsOfSpeech = new HashSet<PartOfSpeech>
        {
            PartOfSpeech.Adjective,
            PartOfSpeech.Noun,
        };

        public Dictionary<string, int> Filter(IReadOnlyDictionary<string, int> statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return statistics
                .Where(pair => grammarInfo.ContainsKey(pair.Key) &&
                               allowedPartsOfSpeech.Contains(grammarInfo[pair.Key].PartOfSpeech))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
