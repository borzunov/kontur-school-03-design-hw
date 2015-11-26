using System.Collections.Generic;
using System.Drawing;

namespace TagCloudGenerator.CloudGenerators
{
    class CloudScheme
    {
        public readonly Size Size;
        public readonly List<WordView> WordViews;

        public CloudScheme(Size size, List<WordView> wordViews)
        {
            Size = size;
            WordViews = wordViews;
        }
    }
}
