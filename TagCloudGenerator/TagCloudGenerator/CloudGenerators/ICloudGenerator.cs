using System.Collections.Generic;
using System.Drawing;
using TagCloudGenerator.Processor;

namespace TagCloudGenerator.CloudGenerators
{
    interface ICloudGenerator
    {
        CloudScheme Generate(WordsRating rating);
    }
}
