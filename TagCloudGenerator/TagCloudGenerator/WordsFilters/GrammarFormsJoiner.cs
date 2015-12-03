using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    class GrammarFormsJoiner : IWordsFilter
    {
        public WordsStatistics Filter(WordsStatistics statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            var wordsGroupedByInitialForm = statistics.OccurrencesCounts.Keys
                .Where(grammarInfo.ContainsKey)
                .GroupBy(word => grammarInfo[word].InitialForm);
            return new WordsStatistics(wordsGroupedByInitialForm
                .Select(wordForms => new
                {
                    MostCommonForm = wordForms
                        .OrderByDescending(form => statistics.OccurrencesCounts[form])
                        .First(),
                    InitialForm = wordForms.Key,
                    TotalCount = wordForms.Select(form => statistics.OccurrencesCounts[form]).Sum()
                })
                .ToDictionary(item => item.MostCommonForm, item => item.TotalCount));
        }
    }
}
