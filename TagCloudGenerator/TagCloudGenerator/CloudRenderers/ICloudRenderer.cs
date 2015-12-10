using TagCloudGenerator.ColorManagers;

namespace TagCloudGenerator.CloudRenderers
{
    interface ICloudRenderer
    {
        void Render(ColoredCloudScheme<WordView> scheme);
    }
}
