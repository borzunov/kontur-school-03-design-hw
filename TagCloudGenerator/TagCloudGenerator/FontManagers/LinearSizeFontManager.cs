using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.FontManagers
{
    class LinearSizeFontManager : IFontManager
    {
        public readonly FontFamily FontFamily;

        public LinearSizeFontManager(FontFamily fontFamily)
        {
            FontFamily = fontFamily;
        }

        const int MinFontEmSize = 10;
        const int MaxFontEmSize = 40;

        Font GetFont(int curRate, int minRate, int maxRate)
        {
            double weightedRate;
            if (minRate == maxRate)
                weightedRate = 1.0;
            else
                weightedRate = (double)(curRate - minRate) / (maxRate - minRate);
            var fontSize = MinFontEmSize + (int)Math.Round(weightedRate * (MaxFontEmSize - MinFontEmSize));
            return new Font(FontFamily, fontSize, FontStyle.Bold);
        }

        public IEnumerable<WordRectangle> GenerateFonts(IReadOnlyList<WordRating> orderedRatings)
        {
            if (orderedRatings.Count == 0)
                return Enumerable.Empty<WordRectangle>();

            var maxRate = orderedRatings[0].OccurencesCount;
            var minRate = orderedRatings[orderedRatings.Count - 1].OccurencesCount;

            return orderedRatings
                .Select(item =>
                {
                    var font = GetFont(item.OccurencesCount, minRate, maxRate);
                    var size = GraphicsFontMeasurer.MeasureString(item.Word, font);
                    return new WordRectangle(item.Word, font, size);
                });
        }
    }
}
