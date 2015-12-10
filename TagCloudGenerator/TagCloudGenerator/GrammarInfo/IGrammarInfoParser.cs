using System.Collections.Generic;

namespace TagCloudGenerator.GrammarInfo
{
    interface IGrammarInfoParser
    {
        IReadOnlyDictionary<string, WordGrammarInfo> GetGrammarInfo(IEnumerable<string> words);
    }
}
