using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordsFilters
{
    class PartOfSpeechFilter_Tests
    {
        [Test]
        public void Filter_filtersByPartOfSpeech()
        {
            var filter = new PartOfSpeechFilter();
            var words = new[] { "работал", "активное", "существо" };
            var grammarInfo = new Dictionary<string, PartOfSpeech>
            {
                {"работал", PartOfSpeech.Verb}, {"существо", PartOfSpeech.Noun}, {"активное", PartOfSpeech.Adjective}
            }.ToDictionary(pair => pair.Key, pair => new WordGrammarInfo(null, pair.Value));

            var filteredWords = filter.Filter(words, grammarInfo);

            filteredWords.Should().BeEquivalentTo("активное", "существо");
        }
    }
}
