using System.Collections.Generic;
using System.Drawing;

namespace TagCloudGenerator.CloudGenerators
{
    interface ICloudGenerator
    {
        CloudScheme Generate(KeyValuePair<string, int>[] wordsRating);
    }
}
