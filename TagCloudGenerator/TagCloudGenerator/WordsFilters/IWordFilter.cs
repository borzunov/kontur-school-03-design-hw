using System.Collections.Generic;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.WordsFilters
{
    interface IWordFilter
    {
        IEnumerable<string> Filter(IEnumerable<string> words,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo);
    }
}
