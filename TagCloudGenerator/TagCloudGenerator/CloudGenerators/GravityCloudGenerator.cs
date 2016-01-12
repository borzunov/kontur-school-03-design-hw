using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.FontManagers;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.CloudGenerators
{
    static class GravityCloudGenerator
    {
        static PlacedWordRectangle PlaceFirstWord(WordRectangle rectangle, Size cloudSize)
        {
            var position = new Point(
                (cloudSize.Width - rectangle.Size.Width) / 2,
                (cloudSize.Height - rectangle.Size.Height) / 2
            );
            return new PlacedWordRectangle(rectangle, position);
        }

        static bool AreSegmentsIntersect(int l1, int r1, int l2, int r2)
        {
            return Math.Min(r1, r2) >= Math.Max(l1, l2);
        }

        class Stripe
        {
            public readonly int Coordinate;
            public readonly IReadOnlyList<PlacedWordRectangle> WordsOnStripe;

            public Stripe(int coordinate, IReadOnlyList<PlacedWordRectangle> wordsOnStripe)
            {
                Coordinate = coordinate;
                WordsOnStripe = wordsOnStripe;
            }
        }

        static Stripe GetHorizontalStripe(Size wordSize,
            IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords, Random random, Size cloudSize)
        {
            var minUsedY = alreadyPlacedWords.Min(view => view.Position.Y);
            var maxUsedY = alreadyPlacedWords.Max(view => view.Position.Y + view.Size.Height);

            var minPositionY = Math.Max(0, minUsedY - wordSize.Height + 1);
            var maxPositionY = Math.Min(cloudSize.Height, maxUsedY + wordSize.Height - 1) - wordSize.Height;
            var positionY = minPositionY + (int)Math.Round(random.NextDouble() * (maxPositionY - minPositionY));

            var wordsOnStripe = alreadyPlacedWords
                .Where(view => AreSegmentsIntersect(positionY, positionY + wordSize.Height,
                    view.Position.Y, view.Position.Y + view.Size.Height))
                .ToList();
            return new Stripe(positionY, wordsOnStripe);
        }

        delegate Point? PlaceMethod(Size wordSize, IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords,
            Random random, Size cloudSize);

        const int PlacingMargin = 3;

        static Point? PlaceWordOnTheLeft(Size wordSize, IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords,
            Random random, Size cloudSize)
        {
            var stripe = GetHorizontalStripe(wordSize, alreadyPlacedWords, random, cloudSize);
            var minUsedXInStripe = stripe.WordsOnStripe.Min(view => view.Position.X);
            var positionX = minUsedXInStripe - wordSize.Width - PlacingMargin;
            if (positionX < 0)
                return null;

            return new Point(positionX, stripe.Coordinate);
        }

        static Point? PlaceWordOnTheRight(Size wordSize, IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords,
            Random random, Size cloudSize)
        {
            var stripe = GetHorizontalStripe(wordSize, alreadyPlacedWords, random, cloudSize);
            var maxUsedXInStripe = stripe.WordsOnStripe.Max(view => view.Position.X + view.Size.Width);
            var positionX = maxUsedXInStripe + PlacingMargin;
            if (positionX + wordSize.Width >= cloudSize.Width)
                return null;

            return new Point(positionX, stripe.Coordinate);
        }

        static Size SwapSizeElements(Size size)
        {
            return new Size(size.Height, size.Width);
        }

        static Point SwapPointCoordinates(Point point)
        {
            return new Point(point.Y, point.X);
        }

        static PlacedWordRectangle SwapViewCoordinates(PlacedWordRectangle rectangle)
        {
            var newRectangle = new WordRectangle(rectangle.Word, rectangle.Font, SwapSizeElements(rectangle.Size));
            return new PlacedWordRectangle(newRectangle, SwapPointCoordinates(rectangle.Position));
        }

        static PlaceMethod RotatePlaceMethod(PlaceMethod method)
        {
            return (wordSize, alreadyPlacedWords, random, cloudSize) =>
            {
                var swappedPoint = method(SwapSizeElements(wordSize),
                    alreadyPlacedWords.Select(SwapViewCoordinates).ToList(), random, SwapSizeElements(cloudSize));
                // FIXME: Need we SwapSizeElementsHere?
                return swappedPoint != null ? (Point?)SwapPointCoordinates(swappedPoint.Value) : null;
            };
        }

        static readonly PlaceMethod[] PlaceMethods =
        {
            PlaceWordOnTheLeft, PlaceWordOnTheRight,
            RotatePlaceMethod(PlaceWordOnTheLeft), RotatePlaceMethod(PlaceWordOnTheRight)
        };

        static PointF GetViewCenter(PlacedWordRectangle rectangle)
        {
            return new PointF(rectangle.Position.X + rectangle.Size.Width / 2.0f,
                              rectangle.Position.Y + rectangle.Size.Height / 2.0f);
        }

        static double DistanceSquareBetween(PointF a, PointF b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }

        static PlacedWordRectangle PlaceNextWord(WordRectangle rectangle,
            IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords, Random random, Size cloudSize)
        {
            if (rectangle.Size.Width > cloudSize.Width || rectangle.Size.Height > cloudSize.Height)
                return null;

            var triesCount = Math.Max(50, alreadyPlacedWords.Count * 2);
            var imageCenter = new PointF(cloudSize.Width / 2.0f, cloudSize.Height / 2.0f);
            PlacedWordRectangle bestRectangle = null;
            double? bestRate = null;
            for (var i = 0; i < triesCount; i++)
            {
                var placeMethod = PlaceMethods[random.Next(PlaceMethods.Length)];
                var position = placeMethod(rectangle.Size, alreadyPlacedWords, random, cloudSize);
                if (position == null)
                    continue;
                var view = new PlacedWordRectangle(rectangle, position.Value);

                var rate = DistanceSquareBetween(GetViewCenter(view), imageCenter);
                if (bestRate == null || rate < bestRate)
                {
                    bestRectangle = view;
                    bestRate = rate;
                }
            }
            return bestRectangle;
        }

        public static CloudGenerator GetCloudGenerator(Random random, Size cloudSize)
        {
            return wordRectangles =>
            {
                var shuffledRectangles = random.Shuffle(wordRectangles);
                if (shuffledRectangles.Length == 0)
                    return new CloudScheme<PlacedWordRectangle>(cloudSize, new List<PlacedWordRectangle>());

                var wordsViews = new List<PlacedWordRectangle>
                {
                    PlaceFirstWord(shuffledRectangles[0], cloudSize)
                };
                foreach (var rectangle in shuffledRectangles.Skip(1))
                {
                    var view = PlaceNextWord(rectangle, wordsViews, random, cloudSize);
                    if (view == null)
                        throw new ArgumentException(
                            "Failed to place all words in an image of given size " +
                            "(you can increase --width and --height parameters or decrease --max-count)");
                    wordsViews.Add(view);
                }

                return new CloudScheme<PlacedWordRectangle>(cloudSize, wordsViews);
            };
        }
    }
}
