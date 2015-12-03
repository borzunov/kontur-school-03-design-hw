using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    class MostCommonWordFilter_Tests
    {
        [Test]
        public void Filter_takesMostCommonWords()
        {
            var filter = new MostCommonWordFilter(new Options
            {
                Count = 2
            });
            var statistics = new WordStatistics(new Dictionary<string, int>
            {
                {"работал", 80}, {"активное", 20}, {"существо", 50}, {"хитрость", 40}
            });

            statistics = filter.Filter(statistics, null);

            statistics.OccurrenceCounts.Should().Equal(new Dictionary<string, int>
            {
                {"работал", 80}, {"существо", 50}
            });
        }
    }
}
