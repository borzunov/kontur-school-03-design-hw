﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagCloudGenerator.WordsSources
{
    class WordListReader : IWordSource
    {
        readonly string filename;

        public WordListReader(Options options)
        {
            filename = options.WordList;
        }

        public List<string> GetWords()
        {
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines.Where(line => line != ""))
            {
                if (!line.All(WordUtils.CanWordInclude))
                    throw new FormatException(
                        $"Line \"${line}\" contains characters that isn't allowed in a word");
            }
            return lines.ToList();
        }
    }
}