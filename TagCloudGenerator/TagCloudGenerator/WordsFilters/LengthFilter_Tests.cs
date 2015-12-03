using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    class LengthFilter_Tests
    {
        [Test]
        public void Filter_excludesWords_withLengthLesserThanMinimal()
        {
            var filter = new LengthFilter(new Options
            {
                MinLength = 3
            });
            var statistics = new WordsStatistics(new Dictionary<string, int>
            {
                {"в", 80}, {"на", 230}, {"как", 50}, {"активное", 20}, {"хитрость", 40}
            });

            statistics = filter.Filter(statistics, null);

            statistics.OccurrencesCounts.Should().Equal(new Dictionary<string, int>
            {
                {"как", 50}, {"активное", 20}, {"хитрость", 40}
            });
        }
    }
}
