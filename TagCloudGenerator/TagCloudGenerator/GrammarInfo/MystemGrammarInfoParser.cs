using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TagCloudGenerator.GrammarInfo
{
    class MystemGrammarInfoParser : IGrammarInfoParser
    {
        public static readonly string MystemPath = AppDomain.CurrentDomain.BaseDirectory;

        public const string MystemExecutableFilename = "mystem.exe";
        const string MystemArguments = "-i -n";

        List<string> CommunicateWithProcess(IEnumerable<string> inputLines)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(MystemPath, MystemExecutableFilename),
                    Arguments = MystemArguments,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    CreateNoWindow = true,
                }
            };

            var outputLines = new List<string>();
            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                    outputLines.Add(args.Data);
            };
            process.Start();
            process.BeginOutputReadLine();
            
            using (var utf8Writer = new StreamWriter(process.StandardInput.BaseStream, Encoding.UTF8))
                foreach (var line in inputLines)
                    utf8Writer.WriteLine(line);
            process.WaitForExit();

            return outputLines;
        }

        static readonly Regex OutputRegex = new Regex(@"^(.+?)\{(.+?)=([A-Z]+)");

        static readonly Dictionary<string, PartOfSpeech> PartOfSpeechCodes = new Dictionary<string, PartOfSpeech>
        {
            { "A", PartOfSpeech.Adjective },
            { "ADV", PartOfSpeech.Adverb },
            { "ADVPRO", PartOfSpeech.PronominalAdverb },
            { "ANUM", PartOfSpeech.NumeralAdjective },
            { "APRO", PartOfSpeech.PronominalAdjective },
            { "COM", PartOfSpeech.PartOfCompound },
            { "CONJ", PartOfSpeech.Conjunction },
            { "INTJ", PartOfSpeech.Interjection },
            { "NUM", PartOfSpeech.Numeral },
            { "PART", PartOfSpeech.Participle },
            { "PR", PartOfSpeech.Preposition },
            { "S", PartOfSpeech.Noun },
            { "SPRO", PartOfSpeech.PronominalNoun },
            { "V", PartOfSpeech.Verb },
        };

        static Tuple<string, WordGrammarInfo> ParseOutputLine(string line)
        {
            var match = OutputRegex.Match(line);
            if (!match.Success)
                return null;

            var word = match.Groups[1].Value;
            var initialForm = match.Groups[2].Value;

            var partOfSpeechCode = match.Groups[3].Value;
            PartOfSpeech partOfSpeech;
            try
            {
                partOfSpeech = PartOfSpeechCodes[partOfSpeechCode];
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException(
                    $"Mystem returned unknown part of speech {partOfSpeechCode}");
            }

            return Tuple.Create(word, new WordGrammarInfo(initialForm, partOfSpeech));
        }

        public IReadOnlyDictionary<string, WordGrammarInfo> GetGrammarInfo(IEnumerable<string> words)
        {
            var wordList = words
                .Where(word => !word.Contains('\''))
                .ToList();
            var parsedOutput = CommunicateWithProcess(wordList)
                .Select(ParseOutputLine)
                .Where(parsed => parsed != null);

            var result = new Dictionary<string, WordGrammarInfo>();
            foreach (var parsed in parsedOutput)
                result[parsed.Item1] = parsed.Item2;
            return result;
        }
    }
}
