using System;
using System.Linq;

namespace TagCloudGenerator.Processor
{
    static class WordRatingTestHelper
    {
        static string GenerateWord(Random random, int length)
        {
            var chars = from i in Enumerable.Range(0, length)
                        select (char)random.Next('a', 'z' + 1);
            return new string(chars.ToArray());
        }

        public static WordRating[] GenerateRatings(Random random, int size)
        {
            var items = from i in Enumerable.Range(0, size)
                        select new WordRating(GenerateWord(random, 5), random.Next(0, 100000));
            return items
                .OrderByDescending(item => item.OccurencesCount)
                .ToArray();
        }
    }
}
