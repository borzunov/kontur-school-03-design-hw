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

        static readonly Regex WordRegex = new Regex(@"^[\p{L}-']+$");

        public List<string> GetWords()
        {
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
                if (!WordRegex.IsMatch(line))
                    throw new FormatException(
                        $"Line \"${line}\" contains characters that isn't allowed in a word");
            return lines.ToList();
        }
    }
}