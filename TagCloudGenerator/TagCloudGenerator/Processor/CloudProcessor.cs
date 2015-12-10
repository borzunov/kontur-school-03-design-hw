using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.CloudGenerators;
using TagCloudGenerator.CloudRenderers;
using TagCloudGenerator.ColorManagers;
using TagCloudGenerator.FontManagers;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.WordsFilters;
using TagCloudGenerator.WordsSources;

namespace TagCloudGenerator.Processor
{
    class CloudProcessor
    {
        readonly IWordSource wordSource;
        readonly IGrammarInfoParser grammarInfoParser;
        readonly GrammarFormJoiner grammarFormJoiner;
        readonly IWordFilter[] wordFilters;
        readonly int wordCount;
        readonly IFontManager fontManager;
        readonly ICloudGenerator cloudGenerator;
        readonly IColorManager colorManager;
        readonly ICloudRenderer cloudRenderer;

        public CloudProcessor(Options options, IWordSource wordSource, IGrammarInfoParser grammarInfoParser,
            GrammarFormJoiner grammarFormJoiner, IWordFilter[] wordFilters,
            IFontManager fontManager, ICloudGenerator cloudGenerator, IColorManager colorManager,
            ICloudRenderer cloudRenderer)
        {
            this.wordSource = wordSource;
            this.grammarInfoParser = grammarInfoParser;
            this.grammarFormJoiner = grammarFormJoiner;
            this.wordFilters = wordFilters;
            wordCount = options.Count;
            this.fontManager = fontManager;
            this.cloudGenerator = cloudGenerator;
            this.colorManager = colorManager;
            this.cloudRenderer = cloudRenderer;
        }

        public void Process()
        {
            var words = wordSource.GetWords();

            var statistics = new OccurrenceStatistics(words);

            var grammarInfo = grammarInfoParser.GetGrammarInfo(statistics.OccurrenceCount.Keys);
            statistics = grammarFormJoiner.Join(statistics, grammarInfo);

            IEnumerable<string> filteredWords = statistics.OccurrenceCount.Keys;
            foreach (var filter in wordFilters)
                filteredWords = filter.Filter(filteredWords, grammarInfo);
            var filteredWordsSet = new HashSet<string>(filteredWords);

            var orderedRatings = statistics.OccurrenceCount
                .Select(pair => new WordRating(pair.Key, pair.Value))
                .Where(item => filteredWordsSet.Contains(item.Word))
                .OrderByDescending(item => item.OccurencesCount)
                .Take(wordCount)
                .ToList();

            var wordRectangles = fontManager.GenerateFonts(orderedRatings);
            var cloudScheme = cloudGenerator.Generate(wordRectangles);
            var coloredCloudScheme = colorManager.GenerateColors(cloudScheme);

            cloudRenderer.Render(coloredCloudScheme);
        }
    }
}
