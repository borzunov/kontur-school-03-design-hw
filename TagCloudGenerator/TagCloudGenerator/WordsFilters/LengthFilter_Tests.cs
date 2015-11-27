using System.Collections.Generic;
using NUnit.Framework;

namespace TagCloudGenerator.WordsFilters
{
    class LengthFilter_Tests
    {
        [Test]
        public void Filter_excludesWords_withLengthLesserThanMinimal()
        {
            var filter = new LengthFilter(3);
            var statistics = new Dictionary<string, int>
            {
                {"в", 80}, {"на", 230}, {"как", 50}, {"активное", 20}, {"хитрость", 40}
            };

            statistics = filter.Filter(statistics, null);

            CollectionAssert.AreEquivalent(statistics, new Dictionary<string, int>
            {
                {"как", 50}, {"активное", 20}, {"хитрость", 40}
            });
        }
    }
}
