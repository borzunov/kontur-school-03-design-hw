using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.Processor;

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

        WordsRating GenerateRating(Random random, int size)
        {
            var items = from i in Enumerable.Range(0, size)
                        select new KeyValuePair<string, int>(
                            GenerateWord(random, 5), random.Next(0, 100000));
            return new WordsRating(items
                .OrderByDescending(item => item.Value)
                .ToArray());
        }

        const int RandomSeed = 42;

        static readonly CenteredCloudGenerator GeneratorExample = new CenteredCloudGenerator(new Options
        {
            BgColor = Color.WhiteSmoke,
            Width = 200,
            Height = 200
        }, FontFamily.GenericSansSerif, new Random(RandomSeed));

        [Test]
        public void Generate_considersConstructorParameters()
        {
            var rating = GenerateRating(new Random(RandomSeed), 10);

            var scheme = GeneratorExample.Generate(rating);

            scheme.BackgroundColor.Should().Be(GeneratorExample.BackgroundColor);
            scheme.WordViews.Should().OnlyContain(view => view.Color == GeneratorExample.TextColor);
            scheme.WordViews.Should().OnlyContain(
                view => view.Font.FontFamily.Name == GeneratorExample.FontFamily.Name);
            scheme.Size.Should().Be(GeneratorExample.Size);
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
                    rect1.IntersectsWith(rect2).Should().BeFalse();
                }
            }
        }

        [Test]
        public void Generate_skipTheRarestWords_ifTheyDontFit()
        {
            var rating = GenerateRating(new Random(RandomSeed), 50);

            var scheme = GeneratorExample.Generate(rating);

            var wordsFromRating = rating.WordsByOccurencesCount
                .Select(item => item.Key)
                .ToArray();
            var displayedWords = scheme.WordViews
                .Select(view => view.Word)
                .ToArray();
            displayedWords.Length.Should().BeLessThan(wordsFromRating.Length);
            displayedWords.Should().BeEquivalentTo(wordsFromRating.Take(displayedWords.Length));
        }

        [Test]
        public void Generate_displaysAllWords_ifTheresALotOfSpace()
        {
            var rating = GenerateRating(new Random(RandomSeed), 5);

            var scheme = GeneratorExample.Generate(rating);

            var wordsFromRating = rating.WordsByOccurencesCount
                .Select(item => item.Key);
            var displayedWords = scheme.WordViews.Select(view => view.Word);
            displayedWords.Should().BeEquivalentTo(wordsFromRating);
        }

        [Test]
        public void Generate_decreasesFontSizeWithWordPopularity()
        {
            var rating = GenerateRating(new Random(RandomSeed), 10);

            var scheme = GeneratorExample.Generate(rating);

            var orderedWords = rating.WordsByOccurencesCount
                .Select(item => item.Key)
                .ToArray();
            var views = scheme.WordViews
                .ToDictionary(view => view.Word, view => view);
            for (var i = 1; i < views.Count; i++)
                views[orderedWords[i - 1]].Font.Size.Should().BeGreaterOrEqualTo(
                    views[orderedWords[i]].Font.Size);
        }
    }
}
