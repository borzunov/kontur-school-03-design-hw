using System.Drawing;

namespace TagCloudGenerator
{
    class Options
    {
        public const string Usage = @"Simple Tag Clound Generator

Usage:
    TagCloudGenerator.exe (--word-list=<filename> | --text-document=<filename>) <output-image> [options]
    TagCloudGenerator.exe (-h | --help)

Options:
    -h, --help                  Show this screen.
    --word-list=<filename>      Load words from text file that contains one word per line.
    --text-document=<filename>  Load words from document with text (only *.txt is supported yet).
    --count=<count>             Maximal count of words that will be displayed [default: 40].
    --min-length=<value>        Minimal length of words that will be displayed [default: 3].
    --bg-color=<color>          Image background color (in HTML notation) [default: #f7b352].
    --font-file=<name>          Font family file name [default: Fonts/BradobreiRegular.ttf].
    --width=<pixels>            Image width [default: 500].
    --height=<pixels>           Image height [default: 500].

This program works only with Russian texts. Only nouns and adjectives are displayed.
If a word was found in various forms, all occurrences are counted but only
the most common form is displayed.

Yandex Mystem is used to find out grammar properties of the words. More info:
    https://tech.yandex.ru/mystem/";

        public string WordList { get; set; }
        public string TextDocument { get; set; }
        public string OutputImage { get; set; }

        public int Count { get; set; }
        public int MinLength { get; set; }

        public Color BgColor { get; set; }
        public string FontFile { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
