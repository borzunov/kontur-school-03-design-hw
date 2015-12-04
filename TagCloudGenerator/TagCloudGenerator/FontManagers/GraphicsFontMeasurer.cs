using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.FontManagers
{
    static class GraphicsFontMeasurer
    {
        static readonly Graphics FakeGraphics = Graphics.FromImage(new Bitmap(1, 1));

        public static Size MeasureString(string text, Font font)
        {
            return FakeGraphics.MeasureString(text, font).ToSize();
        }
    }
}
