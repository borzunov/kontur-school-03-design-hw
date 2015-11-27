using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagCloudGenerator.WordsSources.TextSources
{
    public abstract class TextSource : IWordsSource
    {
        public abstract string GetText();

        public List<string> GetWords()
        {
            return (from Match match in new Regex(@"[\p{L}-']+").Matches(GetText())
                    select match.Value)
                   .ToList();
        }
    }
}
