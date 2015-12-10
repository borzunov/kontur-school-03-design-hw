using System.Collections.Generic;
using System.Drawing;

namespace TagCloudGenerator.CloudGenerators
{
    class CloudScheme<T> where T : PlacedWordRectangle
    {
        public readonly Size Size;
        public readonly IReadOnlyList<T> Words;

        public CloudScheme(Size size, List<T> words)
        {
            Size = size;
            Words = words;
        }
    }
}
