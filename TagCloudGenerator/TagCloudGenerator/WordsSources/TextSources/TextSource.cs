using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagCloudGenerator.WordsSources.TextSources
{
    public abstract class TextSource : IWordsSource
    {
        public abstract string GetText();

        static readonly Regex WordOccurenceRegex = new Regex(@"[\p{L}-']+");

        public List<string> GetWords()
        {
            return (from Match match in WordOccurenceRegex.Matches(GetText())
                    select match.Value)
                   .ToList();
        }
    }
}
