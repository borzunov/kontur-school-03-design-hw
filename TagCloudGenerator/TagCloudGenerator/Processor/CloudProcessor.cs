using TagCloudGenerator.CloudGenerators;
using TagCloudGenerator.CloudRenderers;
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
        readonly ICloudRenderer cloudRenderer;

        public CloudProcessor(Options options, IWordSource wordSource, IGrammarInfoParser grammarInfoParser,
            GrammarFormJoiner grammarFormJoiner, IWordFilter[] wordFilters,
            IFontManager fontManager, ICloudGenerator cloudGenerator,
            ICloudRenderer cloudRenderer)
        {
            this.wordSource = wordSource;
            this.grammarInfoParser = grammarInfoParser;
            this.grammarFormJoiner = grammarFormJoiner;
            this.wordFilters = wordFilters;
            wordCount = options.Count;
            this.fontManager = fontManager;
            this.cloudGenerator = cloudGenerator;
            this.cloudRenderer = cloudRenderer;
        }

        public void Process()
        {
            var words = wordSource.GetWords();

            var statistics = new WordStatistics(words);
            var grammarInfo = grammarInfoParser.GetGrammarInfo(statistics.OccurrenceCounts.Keys);

            statistics = grammarFormJoiner.Join(statistics, grammarInfo);
            
            foreach (var filter in wordFilters)
                statistics = filter.Filter(statistics, grammarInfo);

            var rating = new WordRating(statistics, wordCount);
            var wordRectangles = fontManager.GenerateFonts(rating);
            var cloudScheme = cloudGenerator.Generate(wordRectangles);

            cloudRenderer.Render(cloudScheme);
        }
    }
}
