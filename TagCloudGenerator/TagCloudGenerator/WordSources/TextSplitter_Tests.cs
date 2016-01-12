using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.WordSources;

namespace TagCloudGenerator.WordsSources
{
    class TextSplitter_Tests
    {
        [Test]
        public void GetWords_splitsTextToWords()
        {
            var text = "Well-written Martin's text   (with comment). ";

            var words = TextSplitter.GetWords(text);
            
            words.Should().Equal("Well-written", "Martin's", "text", "with", "comment");
        }

        [Test]
        public void GetWords_supportsCyrillic()
        {
            var text = "Это кириллические буквы.";

            var words = TextSplitter.GetWords(text);

            words.Should().Equal("Это", "кириллические", "буквы");
        }
    }
}
