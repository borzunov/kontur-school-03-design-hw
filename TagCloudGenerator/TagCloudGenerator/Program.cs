using System;
using System.Drawing;
using System.IO;
using Ninject;
using TagCloudGenerator.CloudGenerators;
using TagCloudGenerator.CloudRenderers;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.Processor;
using TagCloudGenerator.WordsFilters;
using TagCloudGenerator.WordsSources;
using TagCloudGenerator.WordsSources.TextSources;

namespace TagCloudGenerator
{
    static class Program
    {
        const string Usage = @"Simple Tag Clound Generator

Usage:
    TagCloudGenerator.exe (--words-list=<filename> | --text-document=<filename>) <output-image> [options]
    TagCloudGenerator.exe (-h | --help)

Options:
    -h, --help               Show this screen.
    --words-list=<filename>  Load words from text file that contains one word per line.
    --text-document=<filename>        Load words from document with text (only *.txt is supported yet).
    --count=<count>          Maximal count of words that will be displayed [default: 40].
    --min-length=<value>     Minimal length of words that will be displayed [default: 3].
    --bg-color=<color>       Image background color (in HTML notation) [default: #f7b352].
    --font-file=<name>       Font family file name [default: Fonts/BradobreiRegular.ttf].
    --width=<pixels>         Image width [default: 500].
    --height=<pixels>        Image height [default: 500].

This program works only with Russian texts. Only nouns and adjectives are displayed.
If a word was found in various forms, all occurrences are counted but only
the most common form is displayed.

Yandex Mystem is used to find out grammar properties of the words. More info:
    https://tech.yandex.ru/mystem/";

        public static void RunWithOptions(Options options)
        {
            var container = new StandardKernel();

            container.Bind<Options>().ToConstant(options);

            if (options.WordsList != null)
                container.Bind<IWordsSource>().To<WordsListReader>();
            else if (options.TextDocument != null)
                container.Bind<IWordsSource>().To<TextDocumentReader>();
            else
                throw new ArgumentException("You should specify either --word-list or --text");

            container.Bind<IGrammarInfoParser>().To<MystemGrammarInfoParser>();
            
            container.Bind<FontFamily>().ToConstant(FontLoader.LoadFontFamily(options.FontFile));

            container.Bind<ICloudGenerator>().To<GravityCloudGenerator>();
            container.Bind<ICloudRenderer>().To<BitmapRenderer>();

            container.Bind<CloudProcessor>().ToSelf()
                .WithConstructorArgument("wordsFilters", new IWordsFilter[]
                {
                    container.Get<PartsOfSpeechFilter>(),
                    container.Get<GrammarFormsJoiner>(),
                    container.Get<LengthFilter>(),
                    container.Get<MostCommonWordsFilter>(),
                });

            container.Get<CloudProcessor>().Process();
        }

        static void Main(string[] args)
        {
            try
            {
                var options = OptionsFiller.Fill<Options>(Usage, args);

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
