using System.Collections.Generic;
using System.Drawing;

namespace TagCloudGenerator.CloudGenerators
{
    class CloudScheme
    {
        public readonly Size Size;
        public readonly Color BackgroundColor;
        public readonly List<WordView> WordViews;

        public CloudScheme(Size size, Color backgroundColor, List<WordView> wordViews)
        {
            Size = size;
            BackgroundColor = backgroundColor;
            WordViews = wordViews;
        }
    }
}
