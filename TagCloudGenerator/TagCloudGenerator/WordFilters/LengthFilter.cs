using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.WordsFilters
{
    class LengthFilter
    {
        readonly int minLength;

        public LengthFilter(int minLength)
        {
            this.minLength = minLength;
        }

        public IEnumerable<string> Filter(IEnumerable<string> words,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            return words.Where(word => word.Length >= minLength);
        }
    }
}
