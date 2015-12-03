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
        readonly IWordsSource wordsSource;
        readonly IGrammarInfoParser grammarInfoParser;
        readonly IWordFilter[] wordFilters;
        readonly ICloudGenerator cloudGenerator;
        readonly ICloudRenderer cloudRenderer;

        public CloudProcessor(IWordsSource wordsSource, IGrammarInfoParser grammarInfoParser,
            IWordFilter[] wordFilters, ICloudGenerator cloudGenerator, ICloudRenderer cloudRenderer)
        {
            this.wordsSource = wordsSource;
            this.grammarInfoParser = grammarInfoParser;
            this.wordFilters = wordFilters;
            this.cloudGenerator = cloudGenerator;
            this.cloudRenderer = cloudRenderer;
        }

        public void Process()
        {
            var words = wordsSource.GetWords();

            var statistics = new WordStatistics(words);
            var grammarInfo = grammarInfoParser.GetGrammarInfo(statistics.OccurrenceCounts.Keys);
            
            foreach (var filter in wordFilters)
                statistics = filter.Filter(statistics, grammarInfo);

            var rating = new WordRating(statistics);
            var cloudScheme = cloudGenerator.Generate(rating);
            cloudRenderer.Render(cloudScheme);
        }
    }
}
