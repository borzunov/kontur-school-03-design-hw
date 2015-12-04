using TagCloudGenerator.CloudGenerators;

namespace TagCloudGenerator.ColorManagers
{
    interface IColorManager
    {
        ColoredCloudScheme<WordView> GenerateColors(CloudScheme<PlacedWordRectangle> scheme);
    }
}
