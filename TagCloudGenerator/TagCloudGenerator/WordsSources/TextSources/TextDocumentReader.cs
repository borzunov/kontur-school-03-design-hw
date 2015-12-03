using System.IO;

namespace TagCloudGenerator.WordsSources.TextSources
{
    class TextDocumentReader : TextSource
    {
        readonly string filename;

        public TextDocumentReader(Options options)
        {
            filename = options.TextDocument;
        }

        public override string GetText()
        {
            return File.ReadAllText(filename);
        }
    }
}
