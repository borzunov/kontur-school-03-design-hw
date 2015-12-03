using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloudGenerator.CloudGenerators
{
    class CenteredCloudGenerator : GraphicsCloudGeneratorBase
    {
        public readonly Color TextColor;
        readonly Random random;

        public CenteredCloudGenerator(Color backgroundColor, Color textColor, FontFamily fontFamily, Size size,
            Random random) : base(backgroundColor, size, fontFamily)
        {
            TextColor = textColor;
            this.random = random;
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

        WordView PlaceNextWord(string word, Font font, IReadOnlyList<WordView> alreadyPlacedWords)
        {
            var wordSize = MeasureString(word, font);
            if (wordSize.Width > Size.Width || wordSize.Height > Size.Height)
                return null;

            var triesCount = Math.Max(50, alreadyPlacedWords.Count * 2);
            for (var i = 0; i < triesCount; i++)
            {
                var curPosition = new Point(
                    random.Next(0, Size.Width - wordSize.Width),
                    random.Next(0, Size.Height - wordSize.Height)
                );
                var curRect = new Rectangle(curPosition, wordSize);

                var hasIntersection = alreadyPlacedWords
                    .Select(placedWord => new Rectangle(placedWord.Position, placedWord.Size))
                    .Any(curRect.IntersectsWith);
                if (!hasIntersection)
                    return new WordView(word, font, TextColor, curPosition, wordSize);
            }
            return null;
        }

        public override CloudScheme Generate(KeyValuePair<string, int>[] wordsRating)
        {
            if (wordsRating.Length == 0)
                return new CloudScheme(Size, BackgroundColor, new List<WordView>());

            var maxRate = wordsRating[0].Value;
            var minRate = wordsRating[wordsRating.Length - 1].Value;

            var firstWordFont = GetFont(maxRate, minRate, maxRate);
            var wordsViews = new List<WordView> { PlaceFirstWord(wordsRating[0].Key, firstWordFont) };

            foreach (var item in wordsRating.Skip(1))
            {
                var font = GetFont(item.Value, minRate, maxRate);
                var view = PlaceNextWord(item.Key, font, wordsViews);
                if (view == null)
                    break;
                wordsViews.Add(view);
            }

            return new CloudScheme(Size, BackgroundColor, wordsViews);
        }
    }
}
