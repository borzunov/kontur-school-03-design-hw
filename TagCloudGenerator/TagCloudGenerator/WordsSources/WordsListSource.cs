using System.Collections.Generic;
using System.Linq;

namespace TagCloudGenerator.WordsSources
{
    /*abstract class WordsListSource : IWordsSource
    {
        public abstract IEnumerable<string> GetWords();

        public Dictionary<string, int> GetWordStatistics()
        {
            return GetWords()
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());
        }
    }*/
}
