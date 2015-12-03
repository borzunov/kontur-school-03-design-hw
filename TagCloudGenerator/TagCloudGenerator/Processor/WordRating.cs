using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Processor
{
    class WordRating
    {
        public readonly KeyValuePair<string, int>[] WordsByOccurenceCount;

        public WordRating(KeyValuePair<string, int>[] wordsByOccurenceCount)
        {
            WordsByOccurenceCount = wordsByOccurenceCount;
        }

        public WordRating(WordStatistics statistics)
        {
            WordsByOccurenceCount = statistics.OccurrenceCounts
                .OrderByDescending(item => item.Value)
                .ToArray();
        }
    }
}
