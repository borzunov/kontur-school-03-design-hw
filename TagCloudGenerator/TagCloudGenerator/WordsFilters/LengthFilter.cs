﻿using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.WordsFilters
{
    class LengthFilter : IWordsFilter
    {
        readonly int minLength;

        public LengthFilter(Options options)
        {
            minLength = options.MinLength;
        }

        public Dictionary<string, int> Filter(IReadOnlyDictionary<string, int> statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return statistics
                .Where(pair => pair.Key.Length >= minLength)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
