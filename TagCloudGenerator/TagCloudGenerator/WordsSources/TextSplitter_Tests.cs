using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.WordsSources.TextSources;

namespace TagCloudGenerator.WordsSources
{
    class TextSplitter_Tests
    {
        [Test]
        public void GetWords_splitsTextToWords()
        {
            var textSource = A.Fake<ITextSource>();
            A.CallTo(() => textSource.GetText()).Returns(
                "Well-written Martin's text   (with comment). ");
            var textSplitter = new TextSplitter(textSource);

            var words = textSplitter.GetWords();
            
            words.Should().Equal("Well-written", "Martin's", "text", "with", "comment");
        }

        [Test]
        public void GetWords_supportsCyrillic()
        {
            var textSource = A.Fake<ITextSource>();
            A.CallTo(() => textSource.GetText()).Returns(
                "Это кириллические буквы.");
            var textSplitter = new TextSplitter(textSource);

            var words = textSplitter.GetWords();

            words.Should().Equal("Это", "кириллические", "буквы");
        }
    }
}
