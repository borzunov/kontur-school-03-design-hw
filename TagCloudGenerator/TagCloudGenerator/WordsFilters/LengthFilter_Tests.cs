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
            var words = new[] { "в", "на", "как", "активное", "хитрость" };

            var filteredWords = filter.Filter(words, null);

            filteredWords.Should().BeEquivalentTo("как", "активное", "хитрость");
        }
    }
}
