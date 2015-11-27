using System.Collections.Generic;
using NUnit.Framework;

namespace TagCloudGenerator.WordsFilters
{
    class MostCommonWordsFilter_Tests
    {
        [Test]
        public void Filter_takesMostCommonWords()
        {
            var filter = new MostCommonWordsFilter(2);
            var statistics = new Dictionary<string, int>
            {
                {"работал", 80}, {"активное", 20}, {"существо", 50}, {"хитрость", 40}
            };

            statistics = filter.Filter(statistics, null);

            CollectionAssert.AreEquivalent(statistics, new Dictionary<string, int>
            {
                {"работал", 80}, {"существо", 50}
            });
        }
    }
}
