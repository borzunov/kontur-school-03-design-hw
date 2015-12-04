using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.WordsFilters
{
    class LengthFilter : IWordFilter
    {
        readonly int minLength;

        public LengthFilter(Options options)
        {
            minLength = options.MinLength;
        }

        public IEnumerable<string> Filter(IEnumerable<string> words,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return words.Where(word => word.Length >= minLength);
        }
    }
}
