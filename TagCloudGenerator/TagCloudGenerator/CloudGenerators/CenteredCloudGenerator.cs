using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.FontManagers;

namespace TagCloudGenerator.CloudGenerators
{
    class CenteredCloudGenerator
    {
        readonly Random random;

        public readonly Size Size;

        public CenteredCloudGenerator(Random random, Size size)
        {
            this.random = random;

            Size = size;
        }

        PlacedWordRectangle PlaceFirstWord(WordRectangle rectangle)
        {
            var position = new Point(
                (Size.Width - rectangle.Size.Width) / 2,
                (Size.Height - rectangle.Size.Height) / 2
            );
            return new PlacedWordRectangle(rectangle, position);
        }

        PlacedWordRectangle PlaceNextWord(WordRectangle rectangle,
            IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords)
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
                    return new PlacedWordRectangle(rectangle, curPosition);
            }
            return null;
        }

        public CloudScheme<PlacedWordRectangle> Generate(IEnumerable<WordRectangle> wordRectangles)
        {
            var rectanglesArray = wordRectangles.ToArray();
            if (rectanglesArray.Length == 0)
                return new CloudScheme<PlacedWordRectangle>(Size, new List<PlacedWordRectangle>());

            var wordsViews = new List<PlacedWordRectangle> { PlaceFirstWord(rectanglesArray[0]) };
            foreach (var rectangle in rectanglesArray.Skip(1))
            {
                var view = PlaceNextWord(rectangle, wordsViews);
                if (view == null)
                    break;
                wordsViews.Add(view);
            }

            return new CloudScheme<PlacedWordRectangle>(Size, wordsViews);
        }
    }
}
