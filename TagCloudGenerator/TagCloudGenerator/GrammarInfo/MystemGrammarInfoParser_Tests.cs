using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloudGenerator.GrammarInfo
{
    class MystemGrammarInfoParser_Tests
    {
        [Test]
        public void GetGrammarInfo_determinesPartsOfSpeech()
        {
            var words = new[] { "Маша", "сделала", "твои", "задачи" };
            
            var grammarInfo = MystemGrammarInfoParser.GetGrammarInfo(words);
            var partsOfSpeech = words
                .Select(word => grammarInfo[word].PartOfSpeech);

            CollectionAssert.AreEqual(new[]
            {
                PartOfSpeech.Noun, PartOfSpeech.Verb, PartOfSpeech.PronominalAdjective, PartOfSpeech.Noun
            }, partsOfSpeech);
        }

        [Test]
        public void GetGrammarInfo_determinesInitialForm()
        {
            var words = new[] { "Лев", "сделал", "нужные", "задачи" };

            var grammarInfo = MystemGrammarInfoParser.GetGrammarInfo(words);
            var partsOfSpeech = words
                .Select(word => grammarInfo[word].InitialForm);

            partsOfSpeech.Should().Equal("лев", "сделать", "нужный", "задача");
        }
    }
}
