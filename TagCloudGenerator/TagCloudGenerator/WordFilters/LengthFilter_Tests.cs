using FluentAssertions;
using NUnit.Framework;

namespace TagCloudGenerator.WordsFilters
{
    class LengthFilter_Tests
    {
        [Test]
        public void Filter_excludesWords_withLengthLesserThanMinimal()
        {
            var filter = new LengthFilter(3);
            var words = new[] { "в", "на", "как", "активное", "хитрость" };

            var filteredWords = filter.Filter(words, null);

            filteredWords.Should().BeEquivalentTo("как", "активное", "хитрость");
        }
    }
}
