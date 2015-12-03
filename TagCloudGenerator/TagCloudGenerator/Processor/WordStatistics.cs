using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Processor
{
    class WordStatistics
    {
        public readonly Dictionary<string, int> OccurrenceCounts;

        public WordStatistics(IEnumerable<string> words)
        {
            OccurrenceCounts = words
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        public WordStatistics(Dictionary<string, int> occurrenceCounts)
        {
            OccurrenceCounts = occurrenceCounts;
        }
    }
}
