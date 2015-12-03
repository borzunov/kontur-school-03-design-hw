using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloudGenerator.WordsFilters
{
    class MostCommonWordsFilter_Tests
    {
        [Test]
        public void Filter_takesMostCommonWords()
        {
            var filter = new MostCommonWordsFilter(new Options
            {
                Count = 2
            });
            var statistics = new Dictionary<string, int>
            {
                {"работал", 80}, {"активное", 20}, {"существо", 50}, {"хитрость", 40}
            };

            statistics = filter.Filter(statistics, null);

            statistics.Should().Equal(new Dictionary<string, int>
            {
                {"работал", 80}, {"существо", 50}
            });
        }
    }
}
