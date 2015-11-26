using System.Collections.Generic;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.WordsFilters
{
    interface IWordsFilter
    {
        Dictionary<string, int> Filter(IReadOnlyDictionary<string, int> statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo);
    }
}
