using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
                // CR (krait): Regex можно закешировать.
                if (!new Regex(@"^[\p{L}-']+$").IsMatch(line))
                    throw new FormatException(
                        $"Line \"${line}\" contains characters that isn't allowed in a word");
            return lines.ToList();
        }
    }
}