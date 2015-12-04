using System.Collections.Generic;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.FontManagers
{
    interface IFontManager
    {
        IEnumerable<WordRectangle> GenerateFonts(WordRating rating);
    }
}
