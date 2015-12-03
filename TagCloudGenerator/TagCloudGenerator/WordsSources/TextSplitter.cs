using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.WordsSources.TextSources;

namespace TagCloudGenerator.WordsSources
{
    class TextSplitter : IWordSource
    {
        readonly ITextSource textSource;

        public TextSplitter(ITextSource textSource)
        {
            this.textSource = textSource;
        }

        public List<string> GetWords()
        {
            var text = textSource.GetText() + '\0';
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
