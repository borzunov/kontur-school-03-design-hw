using System.Collections.Generic;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    interface IWordsFilter
    {
        WordsStatistics Filter(WordsStatistics statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo);
    }
}
