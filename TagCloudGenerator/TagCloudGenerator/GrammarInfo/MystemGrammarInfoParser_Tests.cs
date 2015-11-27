using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TagCloudGenerator.GrammarInfo
{
    class MystemGrammarInfoParser_Tests
    {
        [Test]
        public void GetGrammarInfo_determinesPartsOfSpeech()
        {
            var words = new[] { "Маша", "сделала", "твои", "задачи" };
            
            var grammarInfo = new MystemGrammarInfoParser().GetGrammarInfo(words);
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
            
            var grammarInfo = new MystemGrammarInfoParser().GetGrammarInfo(words);
            var partsOfSpeech = words
                .Select(word => grammarInfo[word].InitialForm);

            CollectionAssert.AreEqual(new[]
            {
                "лев", "сделать", "нужный", "задача"
            }, partsOfSpeech);
        }
    }
}
