using System.Drawing;
using TagCloudGenerator.FontManagers;

namespace TagCloudGenerator.CloudGenerators
{
    class PlacedWordRectangle : WordRectangle
    {
        public readonly Point Position;

        public PlacedWordRectangle(WordRectangle rectangle, Point position) : base(rectangle)
        {
            Position = position;
        }

        public PlacedWordRectangle(PlacedWordRectangle placedRectangle) : base(placedRectangle)
        {
            Position = placedRectangle.Position;
        }
    }
}
