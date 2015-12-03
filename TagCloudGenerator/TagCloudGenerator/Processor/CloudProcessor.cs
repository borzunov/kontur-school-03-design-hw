using System.Linq;
using TagCloudGenerator.CloudGenerators;
using TagCloudGenerator.CloudRenderers;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.WordsFilters;
using TagCloudGenerator.WordsSources;

namespace TagCloudGenerator.Processor
{
    class CloudProcessor
    {
        readonly IWordSource wordSource;
        readonly IGrammarInfoParser grammarInfoParser;
        readonly IWordFilter[] wordFilters;
        readonly int wordCount;
        readonly ICloudGenerator cloudGenerator;
        readonly ICloudRenderer cloudRenderer;

        public CloudProcessor(Options options, IWordSource wordSource, IGrammarInfoParser grammarInfoParser,
            IWordFilter[] wordFilters, ICloudGenerator cloudGenerator, ICloudRenderer cloudRenderer)
        {
            this.wordSource = wordSource;
            this.grammarInfoParser = grammarInfoParser;
            this.wordFilters = wordFilters;
            wordCount = options.Count;
            this.cloudGenerator = cloudGenerator;
            this.cloudRenderer = cloudRenderer;
        }

        public void Process()
        {
            var words = wordSource.GetWords();

            var statistics = new WordStatistics(words);
            var grammarInfo = grammarInfoParser.GetGrammarInfo(statistics.OccurrenceCounts.Keys);
            
            foreach (var filter in wordFilters)
                statistics = filter.Filter(statistics, grammarInfo);

            var rating = new WordRating(statistics, wordCount);
            var cloudScheme = cloudGenerator.Generate(rating);
            cloudRenderer.Render(cloudScheme);
        }
    }
}
