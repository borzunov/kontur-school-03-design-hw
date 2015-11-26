using System.Drawing;

namespace TagCloudGenerator.CloudGenerators
{
    class WordView
    {
        public readonly string Word;
        public readonly Font Font;
        public readonly Color Color;
        public readonly Point Position;

        public WordView(string word, Font font, Color color, Point position)
        {
            Word = word;
            Font = font;
            Color = color;
            Position = position;
        }
    }
}
