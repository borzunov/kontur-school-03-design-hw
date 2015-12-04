using System.Drawing;
using TagCloudGenerator.FontManagers;

namespace TagCloudGenerator.CloudGenerators
{
    class WordView : WordRectangle
    {
        public readonly Point Position;
        public readonly Color Color;

        public WordView(WordRectangle rectangle, Point position, Color color) :
            base(rectangle.Word, rectangle.Font, rectangle.Size)
        {
            Color = color;
            Position = position;
        }
    }
}
