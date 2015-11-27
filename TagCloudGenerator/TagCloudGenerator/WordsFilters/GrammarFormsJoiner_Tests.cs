using System.Collections.Generic;
using System.Linq;
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

            Assert.AreEqual(statistics.Count, 2);
            Assert.AreEqual(statistics.Count(pair => grammarInfo[pair.Key].InitialForm == "активный" &&
                                             pair.Value == 30), 1);
            Assert.AreEqual(statistics.Count(pair => grammarInfo[pair.Key].InitialForm == "команда" &&
                                             pair.Value == 40), 1);
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
            
            CollectionAssert.AreEquivalent(statistics.Keys, new[] { "активное", "команд" });
        }
    }
}
