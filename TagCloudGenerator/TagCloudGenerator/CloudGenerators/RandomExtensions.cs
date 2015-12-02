using System;
using System.Collections.Generic;
using System.Linq;

namespace TagCloudGenerator.CloudGenerators
{
    static class RandomExtensions
    {
        public static T[] Shuffle<T>(this Random random, IEnumerable<T> enumerable)
        {
            var elements = enumerable.ToArray();
            for (var i = 0; i < elements.Length; i++)
            {
                var j = random.Next(0, i + 1);

                var tmp = elements[j];
                elements[j] = elements[i];
                elements[i] = tmp;
            }
            return elements;
        }
    }
}
