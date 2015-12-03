using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Processor
{
    class WordsStatistics
    {
        public readonly Dictionary<string, int> OccurrencesCounts;

        public WordsStatistics(IEnumerable<string> words)
        {
            OccurrencesCounts = words
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        public WordsStatistics(Dictionary<string, int> occurrencesCounts)
        {
            OccurrencesCounts = occurrencesCounts;
        }
    }
}
