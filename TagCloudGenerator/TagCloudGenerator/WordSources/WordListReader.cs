using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagCloudGenerator.WordSources
{
    static class WordListReader
    {
        public static IEnumerable<string> GetWordsFrom(string filename)
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