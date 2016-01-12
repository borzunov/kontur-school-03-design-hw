using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.WordFilters
{
    class PartOfSpeechFilter_Tests
    {
        [Test]
        public void Filter_filtersByPartOfSpeech()
        {
            var filter = PartOfSpeechFilter.GetFilter();
            var words = new[] { "работал", "активное", "существо" };
            var grammarInfo = new Dictionary<string, PartOfSpeech>
            {
                {"работал", PartOfSpeech.Verb}, {"существо", PartOfSpeech.Noun}, {"активное", PartOfSpeech.Adjective}
            }.ToDictionary(pair => pair.Key, pair => new WordGrammarInfo(null, pair.Value));

            var filteredWords = filter(words, grammarInfo);

            filteredWords.Should().BeEquivalentTo("активное", "существо");
        }
    }
}
