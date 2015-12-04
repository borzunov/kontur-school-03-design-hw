using System.Collections.Generic;
using TagCloudGenerator.FontManagers;

namespace TagCloudGenerator.CloudGenerators
{
    interface ICloudGenerator
    {
        CloudScheme Generate(IEnumerable<WordRectangle> wordRectangles);
    }
}
