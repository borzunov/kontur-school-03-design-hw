using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.CloudGenerators
{
    class GravityCloudGenerator : GraphicsCloudGeneratorBase
    {
        readonly Random random;

        delegate Point? PlaceMethod(Size wordSize, IReadOnlyList<WordView> alreadyPlacedWords);

        readonly PlaceMethod[] placeMethods;
        readonly PointF imageCenter;

        public GravityCloudGenerator(Options options, FontFamily fontFamily, Random random) :
            base(options, fontFamily)
        {
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

        WordView PlaceFirstWord(string word, Font font)
        {
            var wordSize = MeasureString(word, font);
            var position = new Point(
                (Size.Width - wordSize.Width) / 2,
                (Size.Height - wordSize.Height) / 2
            );
            return new WordView(word, font, GenerateColor(), position, wordSize);
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

        WordView SwapViewCoordinates(WordView view)
        {
            return new WordView(view.Word, view.Font, view.Color,
                new Point(view.Position.Y, view.Position.X), new Size(view.Size.Height, view.Size.Width));
        }
        
        Point? PlaceWordOnTheTop(Size wordSize, IReadOnlyList<WordView> alreadyPlacedWords)
        {
            var swappedSize = new Size(wordSize.Height, wordSize.Width);
            var swappedPoint = PlaceWordOnTheLeft(swappedSize,
                alreadyPlacedWords.Select(SwapViewCoordinates).ToList());
            return swappedPoint != null ? (Point?) new Point(swappedPoint.Value.Y, swappedPoint.Value.X) : null;
        }
        
        Point? PlaceWordOnTheBottom(Size wordSize, IReadOnlyList<WordView> alreadyPlacedWords)
        {
            var swappedSize = new Size(wordSize.Height, wordSize.Width);
            var swappedPoint = PlaceWordOnTheRight(swappedSize,
                alreadyPlacedWords.Select(SwapViewCoordinates).ToList());
            return swappedPoint != null ? (Point?)new Point(swappedPoint.Value.Y, swappedPoint.Value.X) : null;
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

        WordView PlaceNextWord(string word, Font font, IReadOnlyList<WordView> alreadyPlacedWords)
        {
            var wordSize = MeasureString(word, font);
            if (wordSize.Width > Size.Width || wordSize.Height > Size.Height)
                return null;

            var triesCount = Math.Max(50, alreadyPlacedWords.Count * 2);
            WordView bestView = null;
            double? bestRate = null;
            for (var i = 0; i < triesCount; i++)
            {
                var placeMethod = placeMethods[random.Next(placeMethods.Length)];
                var position = placeMethod(wordSize, alreadyPlacedWords);
                if (position == null)
                    continue;
                var view = new WordView(word, font, GenerateColor(), position.Value, wordSize);

                var rate = DistanceSquareBetween(GetViewCenter(view), imageCenter);
                if (bestRate == null || rate < bestRate)
                {
                    bestView = view;
                    bestRate = rate;
                }
            }
            return bestView;
        }

        public override CloudScheme Generate(WordRating rating)
        {
            var wordsByOccurenceCount = rating.WordsByOccurenceCount;
            if (wordsByOccurenceCount.Length == 0)
                return new CloudScheme(Size, BackgroundColor, new List<WordView>());

            var maxRate = wordsByOccurenceCount[0].Value;
            var minRate = wordsByOccurenceCount[wordsByOccurenceCount.Length - 1].Value;
            wordsByOccurenceCount = random.Shuffle(wordsByOccurenceCount);
            
            var firstWordFont = GetFont(wordsByOccurenceCount[0].Value, minRate, maxRate);
            var wordsViews = new List<WordView> { PlaceFirstWord(wordsByOccurenceCount[0].Key, firstWordFont) };

            foreach (var item in wordsByOccurenceCount.Skip(1))
            {
                var font = GetFont(item.Value, minRate, maxRate);
                var view = PlaceNextWord(item.Key, font, wordsViews);
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
