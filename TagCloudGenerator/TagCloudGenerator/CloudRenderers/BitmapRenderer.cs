using System.Drawing;
using System.Drawing.Imaging;
using TagCloudGenerator.CloudGenerators;

namespace TagCloudGenerator.CloudRenderers
{
    class BitmapRenderer : ICloudRenderer
    {
        readonly string filename;
        readonly ImageFormat format;

        public BitmapRenderer(string filename, ImageFormat format)
        {
            this.filename = filename;
            this.format = format;
        }

        public void Render(CloudScheme scheme)
        {
            using (var bitmap = new Bitmap(scheme.Size.Width, scheme.Size.Height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.Clear(scheme.BackgroundColor);
                    foreach (var view in scheme.WordViews)
                    {
                        g.DrawString(view.Word, view.Font, new SolidBrush(view.Color), view.Position);
                        //g.DrawRectangle(new Pen(Color.Black), new Rectangle(view.Position, view.Size));
                    }
                }
                bitmap.Save(filename, format);
            }
        }
    }
}
