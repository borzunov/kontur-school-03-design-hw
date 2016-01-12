using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TagCloudGenerator.CloudGenerators;
using TagCloudGenerator.CloudRenderers;
using TagCloudGenerator.ColorManagers;
using TagCloudGenerator.FontManagers;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;
using TagCloudGenerator.WordFilters;
using TagCloudGenerator.WordSources;
using TagCloudGenerator.WordSources.TextSources;

namespace TagCloudGenerator
{
    static class Program
    {
        public static void RunWithOptions(Options options)
        {
            IEnumerable<string> words;
            if (options.WordList != null)
                words = WordListReader.GetWordsFrom(options.WordList);
            else if (options.TextDocument != null)
            {
                var text = TextDocumentReader.GetTextFrom(options.TextDocument);
                words = TextSplitter.GetWords(text);
            }
            else
                throw new ArgumentException(
                    "No word source specified (it had to be required by the argument parser)");

            var fontFamily = FontLoader.LoadFontFamily(options.FontFile);
            var random = new Random();
            var cloudSize = new Size(options.Width, options.Height);

            CloudProcessor.Process(
                words,
                MystemGrammarInfoParser.GetGrammarInfo,
                new[]
                {
                    PartOfSpeechFilter.GetFilter(),
                    LengthFilter.GetFilter(options.MinLength)
                },
                options.Count,
                LinearSizeFontManager.GetFontManager(fontFamily),
                GravityCloudGenerator.GetCloudGenerator(random, cloudSize),
                RandomColorManager.GetColorManager(random, options.BgColor),
                BitmapRenderer.GetCloudRenderer(options.OutputImage));
        }

        static void Main(string[] args)
        {
            try
            {
                var options = OptionsFiller.Fill<Options>(Options.Usage, args);

                RunWithOptions(options);

                Console.WriteLine(
                    $"[+] Cloud saved to \"{options.OutputImage}\" ({options.Width}x{options.Height})");
            }
            catch (Exception e) when (e is ArgumentException || e is FormatException || e is IOException)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }
    }
}
