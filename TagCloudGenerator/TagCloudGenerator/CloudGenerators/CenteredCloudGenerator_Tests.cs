using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;

namespace TagCloudGenerator.CloudGenerators
{
    class CenteredCloudGenerator_Tests
    {   
        string GenerateWord(Random random, int length)
        {
            var chars = from i in Enumerable.Range(0, length)
                        select (char)random.Next('a', 'z' + 1);
            return new string(chars.ToArray());
        }

        KeyValuePair<string, int>[] GenerateRating(Random random, int size)
        {
            var items = from i in Enumerable.Range(0, size)
                        select new KeyValuePair<string, int>(
                            GenerateWord(random, 5), random.Next(0, 100000));
            return items.OrderByDescending(item => item.Value).ToArray();
        }

        const int RandomSeed = 42;

        static readonly CenteredCloudGenerator GeneratorExample = new CenteredCloudGenerator(
            Color.WhiteSmoke, Color.Gray, "Arial", new Size(200, 200), new Random(RandomSeed));

        [Test]
        public void Generate_considersConstructorParameters()
        {
            var rating = GenerateRating(new Random(RandomSeed), 10);

            var scheme = GeneratorExample.Generate(rating);

            Assert.AreEqual(scheme.BackgroundColor, GeneratorExample.BackgroundColor);
            Assert.IsTrue(scheme.WordViews.All(view => view.Color == GeneratorExample.TextColor));
            Assert.IsTrue(scheme.WordViews.All(
                view => view.Font.FontFamily.Name == GeneratorExample.FontFamilyName));
            Assert.AreEqual(scheme.Size, GeneratorExample.Size);
        }

        [Test]
        public void Generate_doesntCreateOverlappingWords()
        {
            var rating = GenerateRating(new Random(RandomSeed), 50);

            var scheme = GeneratorExample.Generate(rating);

            foreach (var view1 in scheme.WordViews)
            {
                var rect1 = new Rectangle(view1.Position, view1.Size);
                foreach (var view2 in scheme.WordViews)
                {
                    if (view1 == view2)
                        continue;
                    var rect2 = new Rectangle(view2.Position, view2.Size);
                    Assert.IsFalse(rect1.IntersectsWith(rect2));
                }
            }
        }

        [Test]
        public void Generate_skipTheRarestWords_ifTheyDontFit()
        {
            var rating = GenerateRating(new Random(RandomSeed), 50);

            var scheme = GeneratorExample.Generate(rating);

            var wordsFromRating = rating
                .Select(item => item.Key)
                .ToArray();
            var displayedWords = scheme.WordViews
                .Select(view => view.Word)
                .ToArray();
            Assert.Less(displayedWords.Length, wordsFromRating.Length);
            CollectionAssert.AreEqual(wordsFromRating.Take(displayedWords.Length), displayedWords);
        }

        [Test]
        public void Generate_displaysAllWords_ifTheresALotOfSpace()
        {
            var rating = GenerateRating(new Random(RandomSeed), 5);

            var scheme = GeneratorExample.Generate(rating);

            var wordsFromRating = rating.Select(item => item.Key);
            var displayedWords = scheme.WordViews.Select(view => view.Word);
            CollectionAssert.AreEquivalent(wordsFromRating, displayedWords);
        }

        [Test]
        public void Generate_decreasesFontSizeWithWordPopularity()
        {
            var rating = GenerateRating(new Random(RandomSeed), 10);

            var scheme = GeneratorExample.Generate(rating);

            var allWords = rating
                .Select(item => item.Key)
                .ToArray();
            var views = scheme.WordViews
                .ToDictionary(view => view.Word, view => view);
            for (var i = 1; i < views.Count; i++)
                Assert.GreaterOrEqual(views[allWords[i - 1]].Font.Size, views[allWords[i]].Font.Size);
        }
    }
}
