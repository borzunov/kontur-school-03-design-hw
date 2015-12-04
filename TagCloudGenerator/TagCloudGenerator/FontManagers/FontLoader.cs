using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace TagCloudGenerator.FontManagers
{
    static class FontLoader
    {
        public static FontFamily LoadFontFamily(string relativeFilename)
        {
            var collection = new PrivateFontCollection();
            collection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeFilename));
            return collection.Families[0];
        }
    }
}
