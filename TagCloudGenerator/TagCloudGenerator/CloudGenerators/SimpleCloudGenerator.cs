using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagCloudGenerator.GrammarInfo;

namespace TagCloudGenerator.CloudGenerators
{
    class SimpleCloudGenerator : ICloudGenerator
    {
        static readonly Graphics FakeGraphics = Graphics.FromImage(new Bitmap(1, 1));

        static Size MeasureString(string text, Font font)
        {
            return FakeGraphics.MeasureString(text, font).ToSize();
        }

        const int HorizontalMargin = 10;
        const int VerticalMargin = 3;

        const int Width = 500;

        int maxRate, minRate;
        List<WordView> wordsViews;
        int y;

        Font GetFont(int curRate)
        {
            var weightedRate = (double)(curRate - minRate) / (maxRate - minRate);
            var fontSize = 10 + (int)Math.Round(weightedRate * 30);
            return new Font("Times New Roman", fontSize, FontStyle.Bold);
        }

        int GenerateLine(KeyValuePair<string, int>[] wordsRating, int startIndex)
        {
            var maxTextHeight = 0;
            var x = HorizontalMargin;
            int index;
            for (index = startIndex; index < wordsRating.Length; index++)
            {
                var text = wordsRating[index].Key;
                var rate = wordsRating[index].Value;

                var font = GetFont(rate);
                var textSize = MeasureString(text, font);
                maxTextHeight = Math.Max(maxTextHeight, textSize.Height);

                if (x + textSize.Width + HorizontalMargin >= Width)
                {
                    if (index == startIndex)
                        throw new ArgumentException("Too long word to place it on an image of given width");
                    break;
                }
                wordsViews.Add(new WordView(text, font, Color.Blue, new Point(x, y)));
                x += textSize.Width + HorizontalMargin;
            }

            y += maxTextHeight + VerticalMargin;
            return index;
        }

        public CloudScheme Generate(KeyValuePair<string, int>[] wordsRating)
        {
            maxRate = wordsRating[0].Value;
            minRate = wordsRating[wordsRating.Length - 1].Value;
            wordsViews = new List<WordView>();
            y = VerticalMargin;

            var index = 0;
            while (index < wordsRating.Length)
                index = GenerateLine(wordsRating, index);

            return new CloudScheme(new Size(Width, y), wordsViews);
        }
    }
}
