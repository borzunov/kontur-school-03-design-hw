using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.FontManagers;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.CloudGenerators
{
    class CenteredCloudGenerator_Tests
    {   
        const int RandomSeed = 42;

        static readonly FontManager FontManager =
            LinearSizeFontManager.GetFontManager(FontFamily.GenericSansSerif);

        static readonly Size CloudSize = new Size(200, 200);
        static readonly CloudGenerator CloudGenerator =
            CenteredCloudGenerator.GetCloudGenerator(new Random(RandomSeed), CloudSize);

        [Test]
        public void Generate_createsSchemeOfGivenSize()
        {
            var orderedRatings = WordRatingTestHelper.GenerateRatings(new Random(RandomSeed), 10);
            var wordRectangles = FontManager(orderedRatings);

            var scheme = CloudGenerator(wordRectangles);
            
            scheme.Size.Should().Be(CloudSize);
        }

        [Test]
        public void Generate_doesntCreateOverlappingWords()
        {
            var orderedRatings = WordRatingTestHelper.GenerateRatings(new Random(RandomSeed), 10);
            var wordRectangles = FontManager(orderedRatings);

            var scheme = CloudGenerator(wordRectangles);

            foreach (var view1 in scheme.Words)
            {
                var rect1 = new Rectangle(view1.Position, view1.Size);
                foreach (var view2 in scheme.Words)
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
            var orderedRatings = WordRatingTestHelper.GenerateRatings(new Random(RandomSeed), 50);
            var wordRectangles = FontManager(orderedRatings);

            var scheme = CloudGenerator(wordRectangles);

            var wordsFromRating = orderedRatings
                .Select(item => item.Word)
                .ToArray();
            var displayedWords = scheme.Words
                .Select(view => view.Word)
                .ToArray();
            displayedWords.Length.Should().BeLessThan(wordsFromRating.Length);
            displayedWords.Should().BeEquivalentTo(wordsFromRating.Take(displayedWords.Length));
        }

        [Test]
        public void Generate_displaysAllWords_ifTheresALotOfSpace()
        {
            var orderedRatings = WordRatingTestHelper.GenerateRatings(new Random(RandomSeed), 5);
            var wordRectangles = FontManager(orderedRatings);

            var scheme = CloudGenerator(wordRectangles);

            var wordsFromRating = orderedRatings
                .Select(item => item.Word)
                .ToArray();
            var displayedWords = scheme.Words
                .Select(view => view.Word)
                .ToArray();
            displayedWords.Should().BeEquivalentTo(wordsFromRating);
        }
    }
}
