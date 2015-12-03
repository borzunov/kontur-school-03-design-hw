using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.CloudGenerators
{
    abstract class GraphicsCloudGeneratorBase : ICloudGenerator
    {
        public readonly Color BackgroundColor;
        public readonly Size Size;
        public readonly FontFamily FontFamily;

        protected GraphicsCloudGeneratorBase(Options options, FontFamily fontFamily)
        {
            BackgroundColor = options.BgColor;
            Size = new Size(options.Width, options.Height);
            FontFamily = fontFamily;
        }

        static readonly Graphics FakeGraphics = Graphics.FromImage(new Bitmap(1, 1));

        protected static Size MeasureString(string text, Font font)
        {
            return FakeGraphics.MeasureString(text, font).ToSize();
        }

        protected const int MinFontEmSize = 10;
        protected const int MaxFontEmSize = 40;

        protected Font GetFont(int curRate, int minRate, int maxRate)
        {
            double weightedRate;
            if (minRate == maxRate)
                weightedRate = 1.0;
            else
                weightedRate = (double)(curRate - minRate) / (maxRate - minRate);
            var fontSize = MinFontEmSize + (int)Math.Round(weightedRate * (MaxFontEmSize - MinFontEmSize));
            return new Font(FontFamily, fontSize, FontStyle.Bold);
        }

        public abstract CloudScheme Generate(WordRating rating);
    }
}
