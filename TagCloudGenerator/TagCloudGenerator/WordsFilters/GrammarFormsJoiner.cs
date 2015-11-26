using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.WordsFilters
{
    class GrammarFormsJoiner : IWordsFilter
    {
        public Dictionary<string, int> Filter(IReadOnlyDictionary<string, int> statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            var wordsGroupedByInitialForm = statistics.Keys
                .GroupBy(word => grammarInfo[word].InitialForm);
            return wordsGroupedByInitialForm
                .Select(wordForms => new
                {
                    MostCommonForm = wordForms
                        .OrderByDescending(form => statistics[form])
                        .First(),
                    InitialForm = wordForms.Key,
                    TotalCount = wordForms.Select(form => statistics[form]).Sum()
                })
                .ToDictionary(item => item.MostCommonForm, item => item.TotalCount);
        }
    }
}
