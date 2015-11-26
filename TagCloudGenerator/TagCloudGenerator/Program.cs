using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using TagCloudGenerator.CloudGenerators;
using TagCloudGenerator.CloudRenderers;
using TagCloudGenerator.GrammarInfo;
using TagCloudGenerator.WordsFilters;
using TagCloudGenerator.WordsSources.TextSources;

namespace TagCloudGenerator
{
    static class Program
    {
        static void Main()
        {
            var cloudProcessor = new CloudProcessor(
                new TextDocumentReader("WarAndPeace_Part1.txt"),
                new MystemGrammarInfoParser(),
                new IWordsFilter[]
                {
                    new PartsOfSpeechFilter(new HashSet<PartOfSpeech>
                    {
                        PartOfSpeech.Adjective,
                        PartOfSpeech.Noun,
                    }),
                    new GrammarFormsJoiner(),
                    new MostCommonWordsFilter(20),
                },
                new CenteredCloudGenerator(Color.White, Color.Green, "Times New Roman", new Size(350, 350),
                    new Random()), 
                new BitmapRenderer("output.png", ImageFormat.Png)
            );
            cloudProcessor.Process();
        }
    }
}
