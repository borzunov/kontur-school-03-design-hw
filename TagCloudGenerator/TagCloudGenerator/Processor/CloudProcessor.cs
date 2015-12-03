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
        readonly IWordsFilter[] wordsFilters;
        readonly ICloudGenerator cloudGenerator;
        readonly ICloudRenderer cloudRenderer;

        public CloudProcessor(IWordsSource wordsSource, IGrammarInfoParser grammarInfoParser,
            IWordsFilter[] wordsFilters, ICloudGenerator cloudGenerator, ICloudRenderer cloudRenderer)
        {
            this.wordsSource = wordsSource;
            this.grammarInfoParser = grammarInfoParser;
            this.wordsFilters = wordsFilters;
            this.cloudGenerator = cloudGenerator;
            this.cloudRenderer = cloudRenderer;
        }

        public void Process()
        {
            var words = wordsSource.GetWords();

            var statistics = new WordsStatistics(words);
            var grammarInfo = grammarInfoParser.GetGrammarInfo(statistics.OccurrencesCounts.Keys);
            
            foreach (var filter in wordsFilters)
                statistics = filter.Filter(statistics, grammarInfo);

            var rating = new WordsRating(statistics);
            var cloudScheme = cloudGenerator.Generate(rating);
            cloudRenderer.Render(cloudScheme);
        }
    }
}
