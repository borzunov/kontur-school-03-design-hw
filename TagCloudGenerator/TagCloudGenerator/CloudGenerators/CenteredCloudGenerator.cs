using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.FontManagers;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.CloudGenerators
{
    static class CenteredCloudGenerator
    {
        static PlacedWordRectangle PlaceFirstWord(WordRectangle rectangle, Size cloudSize)
        {
            var position = new Point(
                (cloudSize.Width - rectangle.Size.Width) / 2,
                (cloudSize.Height - rectangle.Size.Height) / 2
            );
            return new PlacedWordRectangle(rectangle, position);
        }

        static PlacedWordRectangle PlaceNextWord(WordRectangle rectangle,
            IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords, Random random, Size cloudSize)
        {
            if (rectangle.Size.Width > cloudSize.Width || rectangle.Size.Height > cloudSize.Height)
                return null;

            var triesCount = Math.Max(50, alreadyPlacedWords.Count * 2);
            for (var i = 0; i < triesCount; i++)
            {
                var curPosition = new Point(
                    random.Next(0, cloudSize.Width - rectangle.Size.Width),
                    random.Next(0, cloudSize.Height - rectangle.Size.Height)
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

        public static CloudGenerator GetCloudGenerator(Random random, Size cloudSize)
        {
            return wordRectangles =>
            {
                var rectanglesArray = wordRectangles.ToArray();
                if (rectanglesArray.Length == 0)
                    return new CloudScheme<PlacedWordRectangle>(cloudSize, new List<PlacedWordRectangle>());

                var wordsViews = new List<PlacedWordRectangle> {PlaceFirstWord(rectanglesArray[0], cloudSize)};
                foreach (var rectangle in rectanglesArray.Skip(1))
                {
                    var view = PlaceNextWord(rectangle, wordsViews, random, cloudSize);
                    if (view == null)
                        break;
                    wordsViews.Add(view);
                }

                return new CloudScheme<PlacedWordRectangle>(cloudSize, wordsViews);
            };
        }
    }
}
