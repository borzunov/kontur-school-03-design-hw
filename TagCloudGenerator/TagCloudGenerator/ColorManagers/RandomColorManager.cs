using System;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.ColorManagers
{
    static class RandomColorManager
    {
        const int MaxColorComponent = 128;

        static Color GenerateColor(Random random)
        {
            return Color.FromArgb(
                random.Next(MaxColorComponent), random.Next(MaxColorComponent), random.Next(MaxColorComponent));
        }

        public static ColorManager GetColorManager(Random random, Color backgroundColor)
        {
            return scheme =>
            {
                var wordViews = scheme.Words
                    .Select(placedRectangle => new WordView(placedRectangle, GenerateColor(random)))
                    .ToList();
                return new ColoredCloudScheme<WordView>(scheme.Size, wordViews, backgroundColor);
            };
        }
    }
}
