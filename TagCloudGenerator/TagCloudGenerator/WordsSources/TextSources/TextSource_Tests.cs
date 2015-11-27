using FakeItEasy;
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
            
            CollectionAssert.AreEqual(new[] { "Well-written", "Martin's", "text", "with", "comment" }, words);
        }

        [Test]
        public void GetWords_supportsCyrillic()
        {
            var textSource = A.Fake<TextSource>();
            A.CallTo(() => textSource.GetText()).Returns(
                "Это кириллические буквы.");

            var words = textSource.GetWords();

            CollectionAssert.AreEqual(new[] { "Это", "кириллические", "буквы" }, words);
        }
    }
}
