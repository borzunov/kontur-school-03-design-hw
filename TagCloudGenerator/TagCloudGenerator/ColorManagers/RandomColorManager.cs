using System;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.CloudGenerators;

namespace TagCloudGenerator.ColorManagers
{
    class RandomColorManager
    {
        readonly Random random;

        public readonly Color BackgroundColor;

        public RandomColorManager(Random random, Color backgroundColor)
        {
            this.random = random;
            BackgroundColor = backgroundColor;
        }
        
        const int MaxColorComponent = 128;

        Color GenerateColor()
        {
            return Color.FromArgb(
                random.Next(MaxColorComponent), random.Next(MaxColorComponent), random.Next(MaxColorComponent));
        }

        public ColoredCloudScheme<WordView> GenerateColors(CloudScheme<PlacedWordRectangle> scheme)
        {
            var wordViews = scheme.Words
                .Select(placedRectangle => new WordView(placedRectangle, GenerateColor()))
                .ToList();
            return new ColoredCloudScheme<WordView>(scheme.Size, wordViews, BackgroundColor);
        }
    }
}
