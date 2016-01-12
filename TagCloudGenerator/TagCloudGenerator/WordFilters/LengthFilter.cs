using System.Linq;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.WordFilters
{
    static class LengthFilter
    {
        public static WordFilter GetFilter(int minLength)
        {
            return (words, grammarInfo) => words.Where(word => word.Length >= minLength);
        }
    }
}
