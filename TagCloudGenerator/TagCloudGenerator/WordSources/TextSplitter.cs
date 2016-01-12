using System.Collections.Generic;

namespace TagCloudGenerator.WordSources
{
    static class TextSplitter
    {
        public static IEnumerable<string> GetWords(string text)
        {
            text = text + '\0';
            int? wordStart = null;
            for (var i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                if (WordUtils.CanWordInclude(ch))
                {
                    if (wordStart == null)
                        wordStart = i;
                    continue;
                }
                if (wordStart != null)
                {
                    yield return text.Substring(wordStart.Value, i - wordStart.Value);
                    wordStart = null;
                }
            }
        }
    }
}
