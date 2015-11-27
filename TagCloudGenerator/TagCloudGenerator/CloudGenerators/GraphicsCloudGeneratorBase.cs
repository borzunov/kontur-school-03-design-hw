﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloudGenerator.CloudGenerators
{
    abstract class GraphicsCloudGeneratorBase : ICloudGenerator
    {
        public readonly Color BackgroundColor;
        public readonly Size Size;
        public readonly string FontFamilyName;

        protected GraphicsCloudGeneratorBase(Color backgroundColor, Size size, string fontFamilyName)
        {
            BackgroundColor = backgroundColor;
            Size = size;
            FontFamilyName = fontFamilyName;
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
            return new Font(FontFamilyName, fontSize, FontStyle.Bold);
        }

        public abstract CloudScheme Generate(KeyValuePair<string, int>[] wordsRating);
    }
}
