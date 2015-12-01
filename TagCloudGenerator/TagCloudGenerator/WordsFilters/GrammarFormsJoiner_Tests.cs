using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.WordsFilters
{
    class GrammarFormsJoiner_Tests
    {
        [Test]
        public void Filter_joinsGrammarForms()
        {
            var filter = new GrammarFormsJoiner();
            var statistics = new Dictionary<string, int>
            {
                {"активный", 10}, {"активное", 20}, {"команд", 40},
            };
            var grammarInfo = new Dictionary<string, WordGrammarInfo>
            {
                {"активный", new WordGrammarInfo("активный", PartOfSpeech.Adjective)},
                {"активное", new WordGrammarInfo("активный", PartOfSpeech.Adjective)},
                {"команд", new WordGrammarInfo("команда", PartOfSpeech.Noun)},
            };

            statistics = filter.Filter(statistics, grammarInfo);

            statistics.Keys.Should().HaveCount(2)
                .And.Contain(key => grammarInfo[key].InitialForm == "активный" &&
                                    statistics[key] == 30)
                .And.Contain(key => grammarInfo[key].InitialForm == "команда" &&
                                    statistics[key] == 40);
        }

        [Test]
        public void Filter_selectsMostCommonForm()
        {
            var filter = new GrammarFormsJoiner();
            var statistics = new Dictionary<string, int>
            {
                {"активный", 10}, {"активное", 20}, {"команд", 40},
            };
            var grammarInfo = new Dictionary<string, WordGrammarInfo>
            {
                {"активный", new WordGrammarInfo("активный", PartOfSpeech.Adjective)},
                {"активное", new WordGrammarInfo("активный", PartOfSpeech.Adjective)},
                {"команд", new WordGrammarInfo("команда", PartOfSpeech.Noun)},
            };

            statistics = filter.Filter(statistics, grammarInfo);
            
            statistics.Keys.Should().BeEquivalentTo("активное", "команд");
        }
    }
}
