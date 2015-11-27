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

        public static Dictionary<string, int> MakeStatistics(IEnumerable<string> words)
        {
            return words
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        public static KeyValuePair<string, int>[] MakeWordsRating(Dictionary<string, int> statistics)
        {
            return statistics
                .OrderByDescending(item => item.Value)
                .ToArray();
        } 

        public void Process()
        {
            var words = wordsSource.GetWords();

            var statistics = MakeStatistics(words);
            var grammarInfo = grammarInfoParser.GetGrammarInfo(statistics.Keys);
            
            foreach (var filter in wordsFilters)
                statistics = filter.Filter(statistics, grammarInfo);

            var wordsRating = MakeWordsRating(statistics);

            var cloudScheme = cloudGenerator.Generate(wordsRating);
            cloudRenderer.Render(cloudScheme);
        }
    }
}
