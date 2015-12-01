using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.WordsFilters
{
    class PartsOfSpeechFilter_Tests
    {
        [Test]
        public void Filter_filtersByPartOfSpeech()
        {
            var filter = new PartsOfSpeechFilter(new HashSet<PartOfSpeech>
            {
                PartOfSpeech.Noun, PartOfSpeech.Verb
            });
            var statistics = new Dictionary<string, int>
            {
                {"работал", 10}, {"активное", 20}, {"существо", 30}
            };
            var grammarInfo = new Dictionary<string, PartOfSpeech>
            {
                {"работал", PartOfSpeech.Verb}, {"существо", PartOfSpeech.Noun}, {"активное", PartOfSpeech.Adjective}
            }.ToDictionary(pair => pair.Key, pair => new WordGrammarInfo(null, pair.Value));

            statistics = filter.Filter(statistics, grammarInfo);

            statistics.Should().Equal(new Dictionary<string, int>
            {
                {"работал", 10}, {"существо", 30}
            });
        }
    }
}
