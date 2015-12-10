using System.Collections.Generic;
using System.Linq;

namespace TagCloudGenerator.Processor
{
    class OccurrenceStatistics
    {
        public readonly IReadOnlyDictionary<string, int> OccurrenceCount;

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
