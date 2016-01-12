using System;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.FontManagers
{
    static class LinearSizeFontManager
    {
        const int MinFontEmSize = 10;
        const int MaxFontEmSize = 40;

        static Font GetFont(FontFamily fontFamily, int curRate, int minRate, int maxRate)
        {
            double weightedRate;
            if (minRate == maxRate)
                weightedRate = 1.0;
            else
                weightedRate = (double)(curRate - minRate) / (maxRate - minRate);
            var fontSize = MinFontEmSize + (int)Math.Round(weightedRate * (MaxFontEmSize - MinFontEmSize));
            return new Font(fontFamily, fontSize, FontStyle.Bold);
        }

        public static FontManager GetFontManager(FontFamily fontFamily)
        {
            return orderedRatings =>
            {
                if (orderedRatings.Count == 0)
                    return Enumerable.Empty<WordRectangle>();

                var maxRate = orderedRatings[0].OccurencesCount;
                var minRate = orderedRatings[orderedRatings.Count - 1].OccurencesCount;

                return orderedRatings
                    .Select(item =>
                    {
                        var font = GetFont(fontFamily, item.OccurencesCount, minRate, maxRate);
                        var size = GraphicsFontMeasurer.MeasureString(item.Word, font);
                        return new WordRectangle(item.Word, font, size);
                    });
            };
        }
    }
}
