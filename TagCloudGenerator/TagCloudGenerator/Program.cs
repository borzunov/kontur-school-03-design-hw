using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.CloudGenerators;
using TagCloudGenerator.CloudRenderers;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.WordsFilters;
using TagCloudGenerator.WordsSources.TextSources;

namespace TagCloudGenerator
{
    static class Program
    {
        static void Main()
        {
            //var wordsStat = new WordsListReader("WordsList.txt").GetWords();
            var words = new TextDocumentReader("WarAndPeace_Part1.txt").GetWords();

            var stats = words
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());

            var grammarInfo = new MystemGrammarInfoParser().GetGrammarInfo(stats.Keys);

            var filters = new IWordsFilter[]
            {
                new PartsOfSpeechFilter(new HashSet<PartOfSpeech>
                {
                    PartOfSpeech.Adjective,
                    PartOfSpeech.Noun,
                }),
                new GrammarFormsFilter(),
                new MostCommonWordsFilter(20),
            };
            foreach (var filter in filters)
                stats = filter.Filter(stats, grammarInfo);

            var wordsRating = stats
                .OrderByDescending(item => item.Value)
                .ToArray();

            var cloudScheme = new SimpleCloudGenerator().Generate(wordsRating);
            new PngRenderer("output.png").Render(cloudScheme);
        }
    }
}
