using System.Collections.Generic;
using TagCloudGenerator.FontManagers;

namespace TagCloudGenerator.CloudGenerators
{
    interface ICloudGenerator
    {
        CloudScheme<PlacedWordRectangle> Generate(IEnumerable<WordRectangle> wordRectangles);
    }
}
