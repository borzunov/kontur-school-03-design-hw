using System;
using System.Diagnostics;
using System.Drawing;
using NUnit.Framework;

namespace TagCloudGenerator
{
    [Explicit]
    [TestFixture]
    internal class Program_Tests
    {
        [Test]
        public void PerformanceTest()
        {
            var options = new Program.Options
            {
                BgColor = Color.White,
                TextColor = Color.Black,
                Count = 100,
                FontFamily = "Monospace",
                Width = 400,
                Height = 400,
                MinLength = 3,
                OutputImage = "output.png",
                Text = "WarAndPeace_Part1.txt"
            };

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < 5; i++)
                Program.RunWithOptions(options);
            
            Console.WriteLine(watch.Elapsed);
        }
    }
}