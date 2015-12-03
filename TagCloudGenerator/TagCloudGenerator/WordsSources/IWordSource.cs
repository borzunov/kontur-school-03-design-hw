using System.Collections.Generic;

namespace TagCloudGenerator.WordsSources
{
    interface IWordSource
    {
        List<string> GetWords();
    }
}
