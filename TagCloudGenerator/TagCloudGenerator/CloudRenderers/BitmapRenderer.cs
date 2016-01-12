using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.CloudRenderers
{
    static class BitmapRenderer
    {   
        static readonly Dictionary<string, ImageFormat> ImageFormats = new Dictionary<string, ImageFormat>()
        {
            {".png", ImageFormat.Png},
            {".bmp", ImageFormat.Bmp},
            {".gif", ImageFormat.Gif},
            {".jpg", ImageFormat.Jpeg},
            {".jpeg", ImageFormat.Jpeg},
        };

        static ImageFormat GetImageFormat(string filename)
        {
            var imageExtension = Path.GetExtension(filename);
            if (imageExtension == null)
                throw new ArgumentException("Can't determine image format by extension");
            imageExtension = imageExtension.ToLower();
            if (!ImageFormats.ContainsKey(imageExtension))
                throw new ArgumentException($"*.{imageExtension} images aren't supported");
            return ImageFormats[imageExtension];
        }

        public static CloudRenderer GetCloudRenderer(string filename)
        {
            var format = GetImageFormat(filename);
            return scheme =>
            {
                using (var bitmap = new Bitmap(scheme.Size.Width, scheme.Size.Height))
                {
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.Clear(scheme.BackgroundColor);
                        foreach (var view in scheme.Words)
                        {
                            g.DrawString(view.Word, view.Font, new SolidBrush(view.Color), view.Position);
                            //g.DrawRectangle(new Pen(Color.Black), new Rectangle(view.Position, view.Size));
                        }
                    }
                    bitmap.Save(filename, format);
                }
            };
        }
    }
}
