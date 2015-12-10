﻿using System;
using System.Collections.Generic;
using System.IO;
using TagCloudGenerator.CloudGenerators;
using TagCloudGenerator.CloudRenderers;
using TagCloudGenerator.ColorManagers;
using TagCloudGenerator.FontManagers;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;
using TagCloudGenerator.WordsFilters;
using TagCloudGenerator.WordsSources;
using TagCloudGenerator.WordsSources.TextSources;

namespace TagCloudGenerator
{
    static class Program
    {
        public static void RunWithOptions(Options options)
        {
            IEnumerable<string> words;
            if (options.WordList != null)
                words = new WordListReader(options).GetWords();
            else if (options.TextDocument != null)
            {
                var text = new TextDocumentReader(options).GetText();
                words = TextSplitter.GetWords(text);
            }
            else
                throw new ArgumentException("You should specify either --word-list or --text");

            var fontFamily = FontLoader.LoadFontFamily(options.FontFile);

            var random = new Random();

            CloudProcessor.Process(
                words,
                MystemGrammarInfoParser.GetGrammarInfo,
                new WordFilter[]
                {
                    new PartOfSpeechFilter().Filter,
                    new LengthFilter(options).Filter, 
                },
                options.Count,
                new LinearSizeFontManager(fontFamily).GenerateFonts, 
                new GravityCloudGenerator(random, options).Generate,
                new RandomColorManager(random, options).GenerateColors, 
                new BitmapRenderer(options).Render);
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
