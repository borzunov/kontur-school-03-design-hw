using System.IO;

namespace TagCloudGenerator.WordsSources.TextSources
{
    class TextDocumentReader
    {
        readonly string filename;

        public TextDocumentReader(string filename)
        {
            this.filename = filename;
        }

        public string GetText()
        {
            return File.ReadAllText(filename);
        }
    }
}
