using System.IO;

namespace TagCloudGenerator.WordsSources.TextSources
{
    class TextDocumentReader : TextSource
    {
        readonly string filename;

        public TextDocumentReader(string filename)
        {
            this.filename = filename;
        }

        protected override string GetText()
        {
            return File.ReadAllText(filename);
        }
    }
}
