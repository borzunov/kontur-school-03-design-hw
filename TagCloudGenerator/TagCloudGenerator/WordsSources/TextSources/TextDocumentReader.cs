using System.IO;

namespace TagCloudGenerator.WordsSources.TextSources
{
    static class TextDocumentReader
    {
        public static string GetTextFrom(string filename)
        {
            return File.ReadAllText(filename);
        }
    }
}
