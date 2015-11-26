using System.Drawing;

namespace TagCloudGenerator.CloudGenerators
{
    class WordView
    {
        public readonly string Word;
        public readonly Font Font;
        public readonly Color Color;
        public readonly Point Position;
        public readonly Size Size;

        public WordView(string word, Font font, Color color, Point position, Size size)
        {
            Word = word;
            Font = font;
            Color = color;
            Position = position;
            Size = size;
        }
    }
}
