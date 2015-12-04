using System.Collections.Generic;

namespace TagCloudGenerator.WordsSources
{
    interface IWordSource
    {
        IEnumerable<string> GetWords();
    }
}
