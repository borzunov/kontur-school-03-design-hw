using System.Collections.Generic;

namespace TagCloudGenerator.WordsSources.TextSources
{
    public abstract class TextSource : IWordsSource
    {
        public abstract string GetText();

        public List<string> GetWords()
        {
            var text = GetText() + '\0';
            int? wordStart = null;
            var words = new List<string>();
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
                    words.Add(text.Substring(wordStart.Value, i - wordStart.Value));
                    wordStart = null;
                }
            }
            return words;
        }
    }
}
