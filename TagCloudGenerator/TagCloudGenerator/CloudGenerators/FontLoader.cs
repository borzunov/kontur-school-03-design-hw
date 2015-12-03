using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudGenerator.CloudGenerators
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
