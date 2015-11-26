using TagCloudGenerator.CloudGenerators;

namespace TagCloudGenerator.CloudRenderers
{
    interface ICloudRenderer
    {
        void Render(CloudScheme scheme);
    }
}
