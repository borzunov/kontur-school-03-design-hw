using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Ninject;
using TagCloudGenerator.CloudGenerators;
using TagCloudGenerator.CloudRenderers;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.WordsFilters;
using TagCloudGenerator.WordsSources;
using TagCloudGenerator.WordsSources.TextSources;

namespace TagCloudGenerator
{
    static class Program
    {
        const string Usage = @"Simple Tag Clound Generator

Usage:
    TagCloudGenerator.exe (--words-list=<filename> | --text=<filename>) <output-image> [options]
    TagCloudGenerator.exe (-h | --help)

Options:
    -h, --help               Show this screen.
    --words-list=<filename>  Load words from text file that contains one word per line.
    --text=<filename>        Load words from document with text (only *.txt is supported yet).
    --count=<count>          Maximal count of words that will be displayed [default: 20].
    --min-length=<value>     Minimal length of words that will be displayed [default: 3].
    --bg-color=<color>       Image background color (in HTML notation) [default: white].
    --text-color=<color>     Text color (in HTML notation) [default: green].
    --font-family=<name>     Font family [default: Times New Roman].
    --width=<pixels>         Image width [default: 350].
    --height=<pixels>        Image height [default: 350].

This program works only with Russian texts. Only nouns and adjectives are displayed.
If a word was found in various forms, all occurrences are counted but only
the most common form is displayed.

Yandex Mystem is used to find out grammar properties of the words. More info:
    https://tech.yandex.ru/mystem/";

        internal class Options
        {
            public string WordsList { get; set; }
            public string Text { get; set; }
            public string OutputImage { get; set; }

            public int Count { get; set; }
            public int MinLength { get; set; }

            public Color BgColor { get; set; }
            public Color TextColor { get; set; }
            public string FontFamily { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        static readonly Dictionary<string, ImageFormat> ImageFormats = new Dictionary<string, ImageFormat>()
        {
            {".png", ImageFormat.Png},
            {".bmp", ImageFormat.Bmp},
            {".gif", ImageFormat.Gif},
            {".jpg", ImageFormat.Jpeg},
            {".jpeg", ImageFormat.Jpeg},
        };

        static ImageFormat GetImageFormat(string filename)
        {
            var imageExtension = Path.GetExtension(filename);
            if (imageExtension == null)
                throw new ArgumentException("Can't determine image format by extension");
            imageExtension = imageExtension.ToLower();
            if (!ImageFormats.ContainsKey(imageExtension))
                throw new ArgumentException($"*.{imageExtension} images aren't supported");
            return ImageFormats[imageExtension];
        }

        static void Main(string[] args)
        {
            try
            {
                var options = OptionsFiller.Fill<Options>(Usage, args);

                RunWithOptions(options);
            }
            catch (Exception e) when (e is ArgumentException || e is FormatException || e is IOException)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }

        public static void RunWithOptions(Options options)
        {
            var container = new StandardKernel();

            if (options.WordsList != null)
                container.Bind<IWordsSource>().To<WordsListReader>()
                    .WithConstructorArgument(options.WordsList);
            else if (options.Text != null)
                container.Bind<IWordsSource>().To<TextDocumentReader>()
                    .WithConstructorArgument(options.Text);
            else
                throw new ArgumentException("You should specify either --word-list or --text");

            container.Bind<IGrammarInfoParser>().To<MystemGrammarInfoParser>();

            container.Bind<PartsOfSpeechFilter>().ToSelf()
                .WithConstructorArgument(new HashSet<PartOfSpeech>
                {
                    PartOfSpeech.Adjective,
                    PartOfSpeech.Noun,
                });
            container.Bind<LengthFilter>().ToSelf()
                .WithConstructorArgument("minLength", options.MinLength);
            container.Bind<MostCommonWordsFilter>().ToSelf()
                .WithConstructorArgument("count", options.Count);

            container.Bind<ICloudGenerator>().To<CenteredCloudGenerator>()
                .WithConstructorArgument("backgroundColor", options.BgColor)
                .WithConstructorArgument("textColor", options.TextColor)
                .WithConstructorArgument("fontFamilyName", options.FontFamily)
                .WithConstructorArgument("size", new Size(options.Width, options.Height));

            container.Bind<ICloudRenderer>().To<BitmapRenderer>()
                .WithConstructorArgument("filename", options.OutputImage)
                .WithConstructorArgument("format", GetImageFormat(options.OutputImage));

            container.Bind<CloudProcessor>().ToSelf()
                .WithConstructorArgument("wordsFilters", new IWordsFilter[]
                {
                    container.Get<PartsOfSpeechFilter>(),
                    container.Get<GrammarFormsJoiner>(),
                    container.Get<LengthFilter>(),
                    container.Get<MostCommonWordsFilter>(),
                });

            container.Get<CloudProcessor>().Process();

            Console.WriteLine(
                $"[+] Cloud saved to \"{options.OutputImage}\" ({options.Width}x{options.Height})");
        }
    }
}
