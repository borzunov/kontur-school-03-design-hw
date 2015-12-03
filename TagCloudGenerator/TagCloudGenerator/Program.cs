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
        public static void RunWithOptions(Options options)
        {
            var container = new StandardKernel();

            container.Bind<Options>().ToConstant(options);

            if (options.WordList != null)
                container.Bind<IWordSource>().To<WordListReader>();
            else if (options.TextDocument != null)
                container.Bind<IWordSource>().To<TextDocumentReader>();
            else
                throw new ArgumentException("You should specify either --word-list or --text");

            container.Bind<IGrammarInfoParser>().To<MystemGrammarInfoParser>();

            container.Bind<IWordFilter>().To<PartOfSpeechFilter>();
            container.Bind<IWordFilter>().To<LengthFilter>();
            
            container.Bind<FontFamily>().ToConstant(FontLoader.LoadFontFamily(options.FontFile));

            container.Bind<ICloudGenerator>().To<GravityCloudGenerator>();
            container.Bind<ICloudRenderer>().To<BitmapRenderer>();

            container.Get<CloudProcessor>().Process();
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
