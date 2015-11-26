using System.Collections.Generic;

namespace TagCloudGenerator.GrammarInfo
{
    internal interface IGrammarInfoParser
    {
        Dictionary<string, WordGrammarInfo> GetGrammarInfo(IEnumerable<string> words);
    }
}
