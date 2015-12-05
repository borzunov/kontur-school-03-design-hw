using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.FontManagers
{
    class LinearSizeFontManager_Tests
    {
        const int RandomSeed = 42;

        [Test]
        public void GenerateFonts_decreasesFontSizeWithWordPopularity()
        {
            var orderedRatings = WordRatingTestHelper.GenerateRatings(new Random(RandomSeed), 10);
            var fontManager = new LinearSizeFontManager(FontFamily.GenericSansSerif);

            var wordRectangles = fontManager.GenerateFonts(orderedRatings).ToArray();

            var rectanglesDictionary = wordRectangles
                .ToDictionary(rectangle => rectangle.Word, rectangle => rectangle);
            rectanglesDictionary.Count.Should().Be(orderedRatings.Length);
            for (var i = 1; i < orderedRatings.Length; i++)
            {
                var biggerWord = orderedRatings[i - 1].Word;
                var smallerWord = orderedRatings[i].Word;
                rectanglesDictionary[biggerWord].Font.Size.Should().BeGreaterOrEqualTo(
                    rectanglesDictionary[smallerWord].Font.Size);
            }
        }
    }
}
