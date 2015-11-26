using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        static void Main()
        {
            var container = new StandardKernel();

            container.Bind<IWordsSource>().To<TextDocumentReader>()
                .WithConstructorArgument("WarAndPeace_Part1.txt");

            container.Bind<IGrammarInfoParser>().To<MystemGrammarInfoParser>();

            container.Bind<PartsOfSpeechFilter>().ToSelf()
                .WithConstructorArgument(new HashSet<PartOfSpeech>
                {
                    PartOfSpeech.Adjective,
                    PartOfSpeech.Noun,
                });
            container.Bind<MostCommonWordsFilter>().ToSelf()
                .WithConstructorArgument("count", 20);

            container.Bind<ICloudGenerator>().To<CenteredCloudGenerator>()
                .WithConstructorArgument("backgroundColor", Color.White)
                .WithConstructorArgument("textColor", Color.Green)
                .WithConstructorArgument("fontFamilyName", "Times New Roman")
                .WithConstructorArgument("size", new Size(350, 350));

            container.Bind<ICloudRenderer>().To<BitmapRenderer>()
                .WithConstructorArgument("filename", "output.png")
                .WithConstructorArgument("format", ImageFormat.Png);

            container.Bind<CloudProcessor>().ToSelf()
                .WithConstructorArgument("wordsFilters", new IWordsFilter[]
                {
                    container.Get<PartsOfSpeechFilter>(),
                    container.Get<GrammarFormsJoiner>(),
                    container.Get<MostCommonWordsFilter>(),
                });

            container.Get<CloudProcessor>().Process();
        }
    }
}
