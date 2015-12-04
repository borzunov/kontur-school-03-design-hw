using System.Collections.Generic;
using System.Drawing;
using TagCloudGenerator.CloudGenerators;

namespace TagCloudGenerator.ColorManagers
{
    class ColoredCloudScheme<T> : CloudScheme<T> where T : WordView
    {
        public readonly Color BackgroundColor;

        public ColoredCloudScheme(Size size, List<T> wordViews, Color backgroundColor) : base(size, wordViews)
        {
            BackgroundColor = backgroundColor;
        }
    }
}
