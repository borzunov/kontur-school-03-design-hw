using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagCloudGenerator.WordsSources
{
    class WordListReader
    {
        readonly string filename;

        public WordListReader(string filename)
        {
            this.filename = filename;
        }

        public IEnumerable<string> GetWords()
        {
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines.Where(line => line != ""))
            {
                if (!line.All(WordUtils.CanWordInclude))
                    throw new FormatException(
                        $"Line \"${line}\" contains characters that isn't allowed in a word");
            }
            return lines;
        }
    }
}