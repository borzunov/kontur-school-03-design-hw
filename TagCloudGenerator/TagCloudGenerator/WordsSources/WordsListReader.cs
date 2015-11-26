using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagCloudGenerator.WordsSources
{
    class WordsListReader : IWordsSource
    {
        readonly string filename;

        public WordsListReader(string filename)
        {
            this.filename = filename;
        }

        public List<string> GetWords()
        {
            return File.ReadAllLines(filename).ToList();
        }
    }
}