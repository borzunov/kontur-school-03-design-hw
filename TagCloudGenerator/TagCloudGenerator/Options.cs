using System.Drawing;

namespace TagCloudGenerator
{
    class Options
    {
        public string WordsList { get; set; }
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
