using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using NUnit.Framework;

namespace TagCloudGenerator
{
    [TestFixture, Explicit]
    class Program_PerformanceTests
    {
        const int WarmupRunsCount = 5;
        const int MeasuredRunsCount = 15;

        [Test]
        public void processCloud_onWordsFromWarAndPeace()
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
                Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WarAndPeace_Part1.txt")
            };

            for (var i = 0; i < WarmupRunsCount; i++)
                Program.RunWithOptions(options);

            var watch = Stopwatch.StartNew();

            for (int i = 0; i < MeasuredRunsCount; i++)
                Program.RunWithOptions(options);

            var averageTimeMsecs = (int)Math.Round(watch.Elapsed.TotalMilliseconds / MeasuredRunsCount);
            Console.WriteLine($"Average time: {averageTimeMsecs} ms");
        }
    }
}