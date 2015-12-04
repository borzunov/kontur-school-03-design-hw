using System.Drawing;

namespace TagCloudGenerator.FontManagers
{
    class WordRectangle
    {
        public readonly string Word;
        public readonly Font Font;
        public readonly Size Size;

        public WordRectangle(string word, Font font, Size size)
        {
            Word = word;
            Font = font;
            Size = size;
        }
    }
}
