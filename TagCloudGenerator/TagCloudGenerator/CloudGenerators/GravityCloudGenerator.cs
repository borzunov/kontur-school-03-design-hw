using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.FontManagers;

namespace TagCloudGenerator.CloudGenerators
{
    class GravityCloudGenerator : ICloudGenerator
    {
        public readonly Color BackgroundColor;
        public readonly Size Size;
        readonly Random random;

        delegate Point? PlaceMethod(Size wordSize, IReadOnlyList<WordView> alreadyPlacedWords);

        readonly PlaceMethod[] placeMethods;
        readonly PointF imageCenter;

        public GravityCloudGenerator(Options options, Random random)
        {
            BackgroundColor = options.BgColor;
            Size = new Size(options.Width, options.Height);
            this.random = random;

            placeMethods = new PlaceMethod[]
            {
                PlaceWordOnTheLeft, PlaceWordOnTheRight, PlaceWordOnTheTop, PlaceWordOnTheBottom
            };

            imageCenter = new PointF(Size.Width / 2.0f, Size.Height / 2.0f);
        }

        const int MaxColorValue = 128;

        Color GenerateColor()
        {
            return Color.FromArgb(random.Next(MaxColorValue), random.Next(MaxColorValue), random.Next(MaxColorValue));
        }

        WordView PlaceFirstWord(WordRectangle rectangle)
        {
            var position = new Point(
                (Size.Width - rectangle.Size.Width) / 2,
                (Size.Height - rectangle.Size.Height) / 2
            );
            return new WordView(rectangle, position, GenerateColor());
        }

        static bool AreSegmentsIntersect(int l1, int r1, int l2, int r2)
        {
            return Math.Min(r1, r2) >= Math.Max(l1, l2);
        }

        Tuple<int, IEnumerable<WordView>> GetHorizontalStripe(Size wordSize,
            IReadOnlyList<WordView> alreadyPlacedWords)
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

        Point? PlaceWordOnTheLeft(Size wordSize, IReadOnlyList<WordView> alreadyPlacedWords)
        {
            var stripe = GetHorizontalStripe(wordSize, alreadyPlacedWords);

            var minUsedXInStripe = stripe.Item2.Min(view => view.Position.X);
            var positionX = minUsedXInStripe - wordSize.Width - PlacingMargin;
            if (positionX < 0)
                return null;

            return new Point(positionX, stripe.Item1);
        }

        Point? PlaceWordOnTheRight(Size wordSize, IReadOnlyList<WordView> alreadyPlacedWords)
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

        static WordView SwapViewCoordinates(WordView view)
        {
            var newRectangle = new WordRectangle(view.Word, view.Font, SwapSizeElements(view.Size));
            return new WordView(newRectangle, SwapPointCoordinates(view.Position), view.Color);
        }
        
        Point? PlaceWordOnTheTop(Size wordSize, IReadOnlyList<WordView> alreadyPlacedWords)
        {
            var swappedPoint = PlaceWordOnTheLeft(SwapSizeElements(wordSize),
                alreadyPlacedWords.Select(SwapViewCoordinates).ToList());
            return swappedPoint != null ? (Point?) SwapPointCoordinates(swappedPoint.Value) : null;
        }
        
        Point? PlaceWordOnTheBottom(Size wordSize, IReadOnlyList<WordView> alreadyPlacedWords)
        {
            var swappedPoint = PlaceWordOnTheRight(SwapSizeElements(wordSize),
                alreadyPlacedWords.Select(SwapViewCoordinates).ToList());
            return swappedPoint != null ? (Point?)SwapPointCoordinates(swappedPoint.Value) : null;
        }

        static PointF GetViewCenter(WordView view)
        {
            return new PointF(view.Position.X + view.Size.Width / 2.0f,
                              view.Position.Y + view.Size.Height / 2.0f);
        }

        static double DistanceSquareBetween(PointF a, PointF b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }

        WordView PlaceNextWord(WordRectangle rectangle, IReadOnlyList<WordView> alreadyPlacedWords)
        {
            if (rectangle.Size.Width > Size.Width || rectangle.Size.Height > Size.Height)
                return null;

            var triesCount = Math.Max(50, alreadyPlacedWords.Count * 2);
            WordView bestView = null;
            double? bestRate = null;
            for (var i = 0; i < triesCount; i++)
            {
                var placeMethod = placeMethods[random.Next(placeMethods.Length)];
                var position = placeMethod(rectangle.Size, alreadyPlacedWords);
                if (position == null)
                    continue;
                var view = new WordView(rectangle, position.Value, GenerateColor());

                var rate = DistanceSquareBetween(GetViewCenter(view), imageCenter);
                if (bestRate == null || rate < bestRate)
                {
                    bestView = view;
                    bestRate = rate;
                }
            }
            return bestView;
        }

        public CloudScheme Generate(IEnumerable<WordRectangle> wordRectangles)
        {
            var shuffledRectangles = random.Shuffle(wordRectangles);
            if (shuffledRectangles.Length == 0)
                return new CloudScheme(Size, BackgroundColor, new List<WordView>());
            
            var wordsViews = new List<WordView> { PlaceFirstWord(shuffledRectangles[0]) };
            foreach (var rectangle in shuffledRectangles.Skip(1))
            {
                var view = PlaceNextWord(rectangle, wordsViews);
                if (view == null)
                    throw new ArgumentException(
                        "Failed to place all words in an image of given size " +
                        "(you can increase --width and --height parameters or decrease --max-count)");
                wordsViews.Add(view);
            }

            return new CloudScheme(Size, BackgroundColor, wordsViews);
        }
    }
}
