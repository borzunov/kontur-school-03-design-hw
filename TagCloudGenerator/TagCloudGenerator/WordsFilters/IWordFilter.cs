using System.Collections.Generic;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    interface IWordFilter
    {
        WordStatistics Filter(WordStatistics statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo);
    }
}
