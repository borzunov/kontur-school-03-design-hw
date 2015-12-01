using System.Collections.Generic;

namespace TagCloudGenerator.WordsSources
{
    interface IWordsSource
    {
        List<string> GetWords();
    }
}
