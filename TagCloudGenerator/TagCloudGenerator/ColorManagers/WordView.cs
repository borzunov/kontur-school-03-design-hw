using System.Drawing;
using TagCloudGenerator.CloudGenerators;

namespace TagCloudGenerator.ColorManagers
{
    class WordView : PlacedWordRectangle
    {
        public readonly Color Color;

        public WordView(PlacedWordRectangle placedRectangle, Color color) : base(placedRectangle)
        {
            Color = color;
        }

        public WordView(WordView view) : base(view)
        {
            Color = view.Color;
        }
    }
}
