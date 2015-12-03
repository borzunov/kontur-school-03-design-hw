using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    class GrammarFormJoiner : IWordFilter
    {
        public WordStatistics Filter(WordStatistics statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            var wordsGroupedByInitialForm = statistics.OccurrenceCounts.Keys
                .Where(grammarInfo.ContainsKey)
                .GroupBy(word => grammarInfo[word].InitialForm);
            return new WordStatistics(wordsGroupedByInitialForm
                .Select(wordForms => new
                {
                    MostCommonForm = wordForms
                        .OrderByDescending(form => statistics.OccurrenceCounts[form])
                        .First(),
                    InitialForm = wordForms.Key,
                    TotalCount = wordForms.Select(form => statistics.OccurrenceCounts[form]).Sum()
                })
                .ToDictionary(item => item.MostCommonForm, item => item.TotalCount));
        }
    }
}
