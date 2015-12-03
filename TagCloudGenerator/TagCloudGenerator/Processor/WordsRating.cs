using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Processor
{
    class WordsRating
    {
        public readonly KeyValuePair<string, int>[] WordsByOccurencesCount;

        public WordsRating(KeyValuePair<string, int>[] wordsByOccurencesCount)
        {
            WordsByOccurencesCount = wordsByOccurencesCount;
        }

        public WordsRating(WordsStatistics statistics)
        {
            WordsByOccurencesCount = statistics.OccurrencesCounts
                .OrderByDescending(item => item.Value)
                .ToArray();
        }
    }
}
