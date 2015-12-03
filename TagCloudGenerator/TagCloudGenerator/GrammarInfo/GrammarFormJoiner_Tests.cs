using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.GrammarInfo
{
    class GrammarFormJoiner_Tests
    {
        [Test]
        public void Join_mergesGrammarForms()
        {
            var filter = new GrammarFormJoiner();
            var statistics = new WordStatistics(new Dictionary<string, int>
            {
                {"активный", 10}, {"активное", 20}, {"команд", 40},
            });
            var grammarInfo = new Dictionary<string, WordGrammarInfo>
            {
                {"активный", new WordGrammarInfo("активный", PartOfSpeech.Adjective)},
                {"активное", new WordGrammarInfo("активный", PartOfSpeech.Adjective)},
                {"команд", new WordGrammarInfo("команда", PartOfSpeech.Noun)},
            };

            statistics = filter.Join(statistics, grammarInfo);

            statistics.OccurrenceCounts.Keys.Should().HaveCount(2)
                .And.Contain(key => grammarInfo[key].InitialForm == "активный" &&
                                    statistics.OccurrenceCounts[key] == 30)
                .And.Contain(key => grammarInfo[key].InitialForm == "команда" &&
                                    statistics.OccurrenceCounts[key] == 40);
        }

        [Test]
        public void Join_selectsMostCommonForm()
        {
            var filter = new GrammarFormJoiner();
            var statistics = new WordStatistics(new Dictionary<string, int>
            {
                {"активный", 10}, {"активное", 20}, {"команд", 40},
            });
            var grammarInfo = new Dictionary<string, WordGrammarInfo>
            {
                {"активный", new WordGrammarInfo("активный", PartOfSpeech.Adjective)},
                {"активное", new WordGrammarInfo("активный", PartOfSpeech.Adjective)},
                {"команд", new WordGrammarInfo("команда", PartOfSpeech.Noun)},
            };

            statistics = filter.Join(statistics, grammarInfo);
            
            statistics.OccurrenceCounts.Keys.Should().BeEquivalentTo("активное", "команд");
        }
    }
}
