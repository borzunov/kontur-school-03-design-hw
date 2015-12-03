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
            var statistics = new WordStatistics(new Dictionary<string, int>
            {
                {"работал", 10}, {"активное", 20}, {"существо", 30}
            });
            var grammarInfo = new Dictionary<string, PartOfSpeech>
            {
                {"работал", PartOfSpeech.Verb}, {"существо", PartOfSpeech.Noun}, {"активное", PartOfSpeech.Adjective}
            }.ToDictionary(pair => pair.Key, pair => new WordGrammarInfo(null, pair.Value));

            statistics = filter.Filter(statistics, grammarInfo);

            statistics.OccurrenceCounts.Should().Equal(new Dictionary<string, int>
            {
                {"активное", 20}, {"существо", 30}
            });
        }
    }
}
