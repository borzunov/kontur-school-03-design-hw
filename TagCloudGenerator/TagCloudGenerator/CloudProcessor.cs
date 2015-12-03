using System.Collections.Generic;
using System.Linq;
using TagCloudGenerator.CloudGenerators;
using TagCloudGenerator.CloudRenderers;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.WordsFilters;
using TagCloudGenerator.WordsSources;

namespace TagCloudGenerator
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

            var statistics = words
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());
            var grammarInfo = grammarInfoParser.GetGrammarInfo(statistics.Keys);
            
            foreach (var filter in wordsFilters)
                statistics = filter.Filter(statistics, grammarInfo);

            var wordsRating = statistics
                .OrderByDescending(item => item.Value)
                .ToArray();

            var cloudScheme = cloudGenerator.Generate(wordsRating);
            cloudRenderer.Render(cloudScheme);
        }
    }
}
