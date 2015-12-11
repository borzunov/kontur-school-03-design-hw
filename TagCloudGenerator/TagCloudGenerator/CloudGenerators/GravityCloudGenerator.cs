using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.FontManagers;

namespace TagCloudGenerator.CloudGenerators
{
    class GravityCloudGenerator
    {
        readonly Random random;

        public readonly Size Size;

        delegate Point? PlaceMethod(Size wordSize, IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords);

        readonly PlaceMethod[] placeMethods;
        readonly PointF imageCenter;

        public GravityCloudGenerator(Random random, Size size)
        {
            this.random = random;
            Size = size;

            placeMethods = new PlaceMethod[]
            {
                PlaceWordOnTheLeft, PlaceWordOnTheRight, PlaceWordOnTheTop, PlaceWordOnTheBottom
            };

            imageCenter = new PointF(Size.Width / 2.0f, Size.Height / 2.0f);
        }

        PlacedWordRectangle PlaceFirstWord(WordRectangle rectangle)
        {
            var position = new Point(
                (Size.Width - rectangle.Size.Width) / 2,
                (Size.Height - rectangle.Size.Height) / 2
            );
            return new PlacedWordRectangle(rectangle, position);
        }

        static bool AreSegmentsIntersect(int l1, int r1, int l2, int r2)
        {
            return Math.Min(r1, r2) >= Math.Max(l1, l2);
        }

        Tuple<int, IEnumerable<PlacedWordRectangle>> GetHorizontalStripe(Size wordSize,
            IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords)
        {
            var minUsedY = alreadyPlacedWords.Min(view => view.Position.Y);
            var maxUsedY = alreadyPlacedWords.Max(view => view.Position.Y + view.Size.Height);

            var minPositionY = Math.Max(0, minUsedY - wordSize.Height + 1);
            var maxPositionY = Math.Min(Size.Height, maxUsedY + wordSize.Height - 1) - wordSize.Height;
            var positionY = minPositionY + (int)Math.Round(random.NextDouble() * (maxPositionY - minPositionY));

            var wordsOnStripe = alreadyPlacedWords
                .Where(view => AreSegmentsIntersect(positionY, positionY + wordSize.Height,
                    view.Position.Y, view.Position.Y + view.Size.Height));
            return Tuple.Create(positionY, wordsOnStripe);
        }

        const int PlacingMargin = 3;

        Point? PlaceWordOnTheLeft(Size wordSize, IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords)
        {
            var stripe = GetHorizontalStripe(wordSize, alreadyPlacedWords);

            var minUsedXInStripe = stripe.Item2.Min(view => view.Position.X);
            var positionX = minUsedXInStripe - wordSize.Width - PlacingMargin;
            if (positionX < 0)
                return null;

            return new Point(positionX, stripe.Item1);
        }

        Point? PlaceWordOnTheRight(Size wordSize, IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords)
        {
            var stripe = GetHorizontalStripe(wordSize, alreadyPlacedWords);

            var maxUsedXInStripe = stripe.Item2.Max(view => view.Position.X + view.Size.Width);
            var positionX = maxUsedXInStripe + PlacingMargin;
            if (positionX + wordSize.Width >= Size.Height)
                return null;

            return new Point(positionX, stripe.Item1);
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
        
        Point? PlaceWordOnTheTop(Size wordSize, IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords)
        {
            var swappedPoint = PlaceWordOnTheLeft(SwapSizeElements(wordSize),
                alreadyPlacedWords.Select(SwapViewCoordinates).ToList());
            return swappedPoint != null ? (Point?) SwapPointCoordinates(swappedPoint.Value) : null;
        }
        
        Point? PlaceWordOnTheBottom(Size wordSize, IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords)
        {
            var swappedPoint = PlaceWordOnTheRight(SwapSizeElements(wordSize),
                alreadyPlacedWords.Select(SwapViewCoordinates).ToList());
            return swappedPoint != null ? (Point?)SwapPointCoordinates(swappedPoint.Value) : null;
        }

        static PointF GetViewCenter(PlacedWordRectangle rectangle)
        {
            return new PointF(rectangle.Position.X + rectangle.Size.Width / 2.0f,
                              rectangle.Position.Y + rectangle.Size.Height / 2.0f);
        }

        static double DistanceSquareBetween(PointF a, PointF b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }

        PlacedWordRectangle PlaceNextWord(WordRectangle rectangle,
            IReadOnlyList<PlacedWordRectangle> alreadyPlacedWords)
        {
            if (rectangle.Size.Width > Size.Width || rectangle.Size.Height > Size.Height)
                return null;

            var triesCount = Math.Max(50, alreadyPlacedWords.Count * 2);
            PlacedWordRectangle bestRectangle = null;
            double? bestRate = null;
            for (var i = 0; i < triesCount; i++)
            {
                var placeMethod = placeMethods[random.Next(placeMethods.Length)];
                var position = placeMethod(rectangle.Size, alreadyPlacedWords);
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

        public CloudScheme<PlacedWordRectangle> Generate(IEnumerable<WordRectangle> wordRectangles)
        {
            var shuffledRectangles = random.Shuffle(wordRectangles);
            if (shuffledRectangles.Length == 0)
                return new CloudScheme<PlacedWordRectangle>(Size, new List<PlacedWordRectangle>());
            
            var wordsViews = new List<PlacedWordRectangle> { PlaceFirstWord(shuffledRectangles[0]) };
            foreach (var rectangle in shuffledRectangles.Skip(1))
            {
                var view = PlaceNextWord(rectangle, wordsViews);
                if (view == null)
                    throw new ArgumentException(
                        "Failed to place all words in an image of given size " +
                        "(you can increase --width and --height parameters or decrease --max-count)");
                wordsViews.Add(view);
            }

            return new CloudScheme<PlacedWordRectangle>(Size, wordsViews);
        }
    }
}
