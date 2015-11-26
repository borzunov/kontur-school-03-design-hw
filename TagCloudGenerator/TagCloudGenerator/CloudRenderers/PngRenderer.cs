using System.Drawing;
using System.Drawing.Imaging;
using TagCloudGenerator.CloudGenerators;

namespace TagCloudGenerator.CloudRenderers
{
    class PngRenderer : ICloudRenderer
    {
        readonly string filename;

        public PngRenderer(string filename)
        {
            this.filename = filename;
        }

        public void Render(CloudScheme scheme)
        {
            using (var bitmap = new Bitmap(scheme.Size.Width, scheme.Size.Height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    foreach (var view in scheme.WordViews)
                        g.DrawString(view.Word, view.Font, new SolidBrush(view.Color), view.Position);
                }
                bitmap.Save(filename, ImageFormat.Png);
            }
        }
    }
}
