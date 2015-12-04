using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.FontManagers;

namespace TagCloudGenerator.CloudGenerators
{
    class CenteredCloudGenerator : ICloudGenerator
    {
        public readonly Color BackgroundColor;
        public readonly Size Size;
        readonly Random random;

        public readonly Color TextColor = Color.Green;

        public CenteredCloudGenerator(Options options, Random random)
        {
            BackgroundColor = options.BgColor;
            Size = new Size(options.Width, options.Height);
            this.random = random;
        }

        WordView PlaceFirstWord(WordRectangle rectangle)
        {
            var position = new Point(
                (Size.Width - rectangle.Size.Width) / 2,
                (Size.Height - rectangle.Size.Height) / 2
            );
            return new WordView(rectangle, position, TextColor);
        }

        WordView PlaceNextWord(WordRectangle rectangle, IReadOnlyList<WordView> alreadyPlacedWords)
        {
            if (rectangle.Size.Width > Size.Width || rectangle.Size.Height > Size.Height)
                return null;

            var triesCount = Math.Max(50, alreadyPlacedWords.Count * 2);
            for (var i = 0; i < triesCount; i++)
            {
                var curPosition = new Point(
                    random.Next(0, Size.Width - rectangle.Size.Width),
                    random.Next(0, Size.Height - rectangle.Size.Height)
                );
                var curRect = new Rectangle(curPosition, rectangle.Size);

                var hasIntersection = alreadyPlacedWords
                    .Select(placedWord => new Rectangle(placedWord.Position, placedWord.Size))
                    .Any(curRect.IntersectsWith);
                if (!hasIntersection)
                    return new WordView(rectangle, curPosition, TextColor);
            }
            return null;
        }

        public CloudScheme Generate(IEnumerable<WordRectangle> wordRectangles)
        {
            var rectanglesArray = wordRectangles.ToArray();
            if (rectanglesArray.Length == 0)
                return new CloudScheme(Size, BackgroundColor, new List<WordView>());

            var wordsViews = new List<WordView> { PlaceFirstWord(rectanglesArray[0]) };
            foreach (var rectangle in rectanglesArray.Skip(1))
            {
                var view = PlaceNextWord(rectangle, wordsViews);
                if (view == null)
                    break;
                wordsViews.Add(view);
            }

            return new CloudScheme(Size, BackgroundColor, wordsViews);
        }
    }
}
