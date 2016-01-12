using FluentAssertions;
using NUnit.Framework;

namespace TagCloudGenerator.WordFilters
{
    class LengthFilter_Tests
    {
        [Test]
        public void Filter_excludesWords_withLengthLesserThanMinimal()
        {
            var filter = LengthFilter.GetFilter(3);
            var words = new[] { "в", "на", "как", "активное", "хитрость" };

            var filteredWords = filter(words, null);

            filteredWords.Should().BeEquivalentTo("как", "активное", "хитрость");
        }
    }
}
