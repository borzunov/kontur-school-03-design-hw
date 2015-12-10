using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.CloudGenerators;
using TagCloudGenerator.ColorManagers;
using TagCloudGenerator.FontManagers;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.Processor
{
    delegate IReadOnlyDictionary<string, WordGrammarInfo> GrammarInfoParser(IEnumerable<string> words);
    delegate IEnumerable<string> WordFilter(IEnumerable<string> words,
            IReadOnlyDictionary<string, WordGrammarInfo> grammarInfo);
    delegate IEnumerable<WordRectangle> FontManager(IReadOnlyList<WordRating> orderedRatings);
    delegate CloudScheme<PlacedWordRectangle> CloudGenerator(IEnumerable<WordRectangle> wordRectangles);
    delegate ColoredCloudScheme<WordView> ColorManager(CloudScheme<PlacedWordRectangle> scheme);
    delegate void CloudRenderer(ColoredCloudScheme<WordView> scheme);

    static class CloudProcessor
    {
        public static void Process(IEnumerable<string> words,
            GrammarInfoParser grammarInfoParser,
            WordFilter[] wordFilters,
            int wordCount,
            FontManager fontManager,
            CloudGenerator cloudGenerator,
            ColorManager colorManager,
            CloudRenderer cloudRenderer)
        {
            var statistics = new OccurrenceStatistics(words);

            var grammarInfo = grammarInfoParser(statistics.OccurrenceCount.Keys);
            statistics = GrammarFormJoiner.Join(statistics, grammarInfo);

            IEnumerable<string> filteredWords = statistics.OccurrenceCount.Keys;
            foreach (var filter in wordFilters)
                filteredWords = filter(filteredWords, grammarInfo);
            var filteredWordsSet = new HashSet<string>(filteredWords);

            var orderedRatings = statistics.OccurrenceCount
                .Select(pair => new WordRating(pair.Key, pair.Value))
                .Where(item => filteredWordsSet.Contains(item.Word))
                .OrderByDescending(item => item.OccurencesCount)
                .Take(wordCount)
                .ToList();

            var wordRectangles = fontManager(orderedRatings);
            var cloudScheme = cloudGenerator(wordRectangles);
            var coloredCloudScheme = colorManager(cloudScheme);

            cloudRenderer(coloredCloudScheme);
        }
    }
}
