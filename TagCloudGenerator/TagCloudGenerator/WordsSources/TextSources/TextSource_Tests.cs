using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloudGenerator.WordsSources.TextSources
{
    class TextSource_Tests
    {
        [Test]
        public void GetWords_splitsTextToWords()
        {
            var textSource = A.Fake<TextSource>();
            A.CallTo(() => textSource.GetText()).Returns(
                "Well-written Martin's text   (with comment). ");

            var words = textSource.GetWords();
            
            words.Should().Equal("Well-written", "Martin's", "text", "with", "comment");
        }

        [Test]
        public void GetWords_supportsCyrillic()
        {
            var textSource = A.Fake<TextSource>();
            A.CallTo(() => textSource.GetText()).Returns(
                "Это кириллические буквы.");

            var words = textSource.GetWords();

            words.Should().Equal("Это", "кириллические", "буквы");
        }
    }
}
