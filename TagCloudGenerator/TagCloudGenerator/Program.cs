using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using DocoptNet;
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
    TagCloudGenerator.exe (--word-list=<filename> | --text=<filename>) <output-image> [options]
    TagCloudGenerator.exe (-h | --help)

Options:
    -h, --help               Show this screen.
    --word-list=<filename>   Load words from text file that contains one word per line.
    --text=<filename>        Load words from document with text (only *.txt is supported yet).
    --count=<count>          Maximal count of words that will be displayed [default: 20].
    --bg-color=<color>       Set image background color [default: white].
    --text-color=<color>     Set text color [default: green].
    --font-family=<name>     Set font family [default: Times New Roman].
    --width=<pixels>         Set image width [default: 350].
    --height=<pixels>        Set image height [default: 350].";

        static void Main(string[] args)
        {
            try
            {
                var options = new Docopt().Apply(Usage, args, exit: true);

                var container = new StandardKernel();

                if (options["--word-list"] != null)
                    container.Bind<IWordsSource>().To<WordsListReader>()
                        .WithConstructorArgument(options["--word-list"].ToString());
                else if (options["--text"] != null)
                    container.Bind<IWordsSource>().To<TextDocumentReader>()
                        .WithConstructorArgument(options["--text"].ToString());
                else
                    throw new ArgumentException("You should specify either --word-list or --text");

                container.Bind<IGrammarInfoParser>().To<MystemGrammarInfoParser>();

                container.Bind<PartsOfSpeechFilter>().ToSelf()
                    .WithConstructorArgument(new HashSet<PartOfSpeech>
                    {
                        PartOfSpeech.Adjective,
                        PartOfSpeech.Noun,
                    });
                container.Bind<MostCommonWordsFilter>().ToSelf()
                    .WithConstructorArgument("count", options["--count"].AsInt);

                container.Bind<ICloudGenerator>().To<CenteredCloudGenerator>()
                    .WithConstructorArgument("backgroundColor",
                        ColorTranslator.FromHtml(options["--bg-color"].ToString()))
                    .WithConstructorArgument("textColor",
                        ColorTranslator.FromHtml(options["--text-color"].ToString()))
                    .WithConstructorArgument("fontFamilyName", options["--font-family"].ToString())
                    .WithConstructorArgument("size",
                        new Size(options["--width"].AsInt, options["--height"].AsInt));

                var imageFilename = options["<output-image>"].ToString();
                container.Bind<ICloudRenderer>().To<BitmapRenderer>()
                    .WithConstructorArgument("filename", imageFilename)
                    .WithConstructorArgument("format", ImageFormat.Png);

                container.Bind<CloudProcessor>().ToSelf()
                    .WithConstructorArgument("wordsFilters", new IWordsFilter[]
                    {
                        container.Get<PartsOfSpeechFilter>(),
                        container.Get<GrammarFormsJoiner>(),
                        container.Get<MostCommonWordsFilter>(),
                    });

                container.Get<CloudProcessor>().Process();
                Console.WriteLine($"[+] Cloud saved to \"{imageFilename}\"");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[-] Error: {e.Message}");
            }
        }
    }
}
