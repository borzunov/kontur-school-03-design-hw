using System.IO;

namespace TagCloudGenerator.WordsSources.TextSources
{
    class TextDocumentReader : ITextSource
    {
        readonly string filename;

        public TextDocumentReader(Options options)
        {
            filename = options.TextDocument;
        }

        public string GetText()
        {
            return File.ReadAllText(filename);
        }
    }
}
