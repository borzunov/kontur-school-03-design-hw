using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.GrammarInfo
{
    class GrammarFormJoiner
    {
        public OccurrenceStatistics Join(OccurrenceStatistics statistics,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo)
        {
            var wordsGroupedByInitialForm = statistics.OccurrenceCount.Keys
                .Where(grammarInfo.ContainsKey)
                .GroupBy(word => grammarInfo[word].InitialForm);
            return new OccurrenceStatistics(wordsGroupedByInitialForm
                .Select(wordForms => new
                {
                    MostCommonForm = wordForms
                        .OrderByDescending(form => statistics.OccurrenceCount[form])
                        .First(),
                    InitialForm = wordForms.Key,
                    TotalCount = wordForms.Select(form => statistics.OccurrenceCount[form]).Sum()
                })
                .ToDictionary(item => item.MostCommonForm, item => item.TotalCount));
        }
    }
}
