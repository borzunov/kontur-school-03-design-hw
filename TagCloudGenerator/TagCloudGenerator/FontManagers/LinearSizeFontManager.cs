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

        public IEnumerable<WordRectangle> GenerateFonts(WordRating rating)
        {
            var wordsByOccurenceCount = rating.WordsByOccurenceCount;
            if (wordsByOccurenceCount.Length == 0)
                return Enumerable.Empty<WordRectangle>();

            var maxRate = wordsByOccurenceCount[0].Value;
            var minRate = wordsByOccurenceCount[wordsByOccurenceCount.Length - 1].Value;

            return rating.WordsByOccurenceCount
                .Select(pair =>
                {
                    var word = pair.Key;
                    var font = GetFont(pair.Value, minRate, maxRate);
                    var size = GraphicsFontMeasurer.MeasureString(word, font);
                    return new WordRectangle(word, font, size);
                });
        }
    }
}
