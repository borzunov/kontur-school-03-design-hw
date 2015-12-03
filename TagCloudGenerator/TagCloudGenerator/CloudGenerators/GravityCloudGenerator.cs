using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloudGenerator.CloudGenerators
{
    class GravityCloudGenerator : GraphicsCloudGeneratorBase
    {
        public readonly Color TextColor;
        readonly Random random;

        delegate WordView PlaceMethod(string word, Size wordSize, Font font,
            IReadOnlyList<WordView> alreadyPlacedWords);

        readonly PlaceMethod[] placeMethods;
        readonly PointF imageCenter;

        public GravityCloudGenerator(Color backgroundColor, Color textColor, FontFamily fontFamily, Size size,
            Random random) : base(backgroundColor, size, fontFamily)
        {
            TextColor = textColor;
            this.random = random;

            placeMethods = new PlaceMethod[]
            {
                PlaceWordOnTheLeft, PlaceWordOnTheRight, PlaceWordOnTheTop, PlaceWordOnTheBottom
            };

            imageCenter = new PointF(size.Width / 2.0f, size.Height / 2.0f);
        }

        WordView PlaceFirstWord(string word, Font font)
        {
            var wordSize = MeasureString(word, font);
            var position = new Point(
                (Size.Width - wordSize.Width) / 2,
                (Size.Height - wordSize.Height) / 2
            );
            return new WordView(word, font, TextColor, position, wordSize);
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

        WordView PlaceWordOnTheLeft(string word, Size wordSize, Font font,
            IReadOnlyList<WordView> alreadyPlacedWords)
        {
            var stripe = GetHorizontalStripe(wordSize, alreadyPlacedWords);

            var minUsedXInStripe = stripe.Item2.Min(view => view.Position.X);
            var positionX = minUsedXInStripe - wordSize.Width;
            if (positionX < 0)
                return null;

            return new WordView(word, font, TextColor, new Point(positionX, stripe.Item1), wordSize);
        }

        WordView PlaceWordOnTheRight(string word, Size wordSize, Font font,
            IReadOnlyList<WordView> alreadyPlacedWords)
        {
            var stripe = GetHorizontalStripe(wordSize, alreadyPlacedWords);

            var maxUsedXInStripe = stripe.Item2.Max(view => view.Position.X + view.Size.Width);
            var positionX = maxUsedXInStripe;
            if (positionX + wordSize.Width >= Size.Height)
                return null;

            return new WordView(word, font, TextColor, new Point(positionX, stripe.Item1), wordSize);
        }

        WordView SwapViewCoordinates(WordView view)
        {
            return new WordView(view.Word, view.Font, view.Color,
                new Point(view.Position.Y, view.Position.X), new Size(view.Size.Height, view.Size.Width));
        }
        
        WordView PlaceWordOnTheTop(string word, Size wordSize, Font font,
            IReadOnlyList<WordView> alreadyPlacedWords)
        {
            var swappedSize = new Size(wordSize.Height, wordSize.Width);
            var swappedView = PlaceWordOnTheLeft(word, swappedSize, font,
                alreadyPlacedWords.Select(SwapViewCoordinates).ToList());
            return swappedView != null ? SwapViewCoordinates(swappedView) : null;
        }
        
        WordView PlaceWordOnTheBottom(string word, Size wordSize, Font font,
            IReadOnlyList<WordView> alreadyPlacedWords)
        {
            var swappedSize = new Size(wordSize.Height, wordSize.Width);
            var swappedView = PlaceWordOnTheRight(word, swappedSize, font,
                alreadyPlacedWords.Select(SwapViewCoordinates).ToList());
            return swappedView != null ? SwapViewCoordinates(swappedView) : null;
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
                var view = placeMethod(word, wordSize, font, alreadyPlacedWords);
                if (view == null)
                    continue;

                var rate = DistanceSquareBetween(GetViewCenter(view), imageCenter);
                if (bestRate == null || rate < bestRate)
                {
                    bestView = view;
                    bestRate = rate;
                }
            }
            return bestView;
        }

        public override CloudScheme Generate(KeyValuePair<string, int>[] wordsRating)
        {
            if (wordsRating.Length == 0)
                return new CloudScheme(Size, BackgroundColor, new List<WordView>());

            var maxRate = wordsRating[0].Value;
            var minRate = wordsRating[wordsRating.Length - 1].Value;
            wordsRating = random.Shuffle(wordsRating);
            
            var firstWordFont = GetFont(wordsRating[0].Value, minRate, maxRate);
            var wordsViews = new List<WordView> { PlaceFirstWord(wordsRating[0].Key, firstWordFont) };

            foreach (var item in wordsRating.Skip(1))
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
