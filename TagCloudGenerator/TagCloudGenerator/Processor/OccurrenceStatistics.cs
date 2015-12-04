using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.Processor
{
    class OccurrenceStatistics
    {
        public readonly Dictionary<string, int> OccurrenceCount;

        public OccurrenceStatistics(IEnumerable<string> words)
        {
            OccurrenceCount = words
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        public OccurrenceStatistics(Dictionary<string, int> occurrenceCount)
        {
            OccurrenceCount = occurrenceCount;
        }
    }
}
