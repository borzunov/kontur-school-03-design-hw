namespace TagCloudGenerator.WordsSources
{
    static class WordUtils
    {
        public static bool CanWordInclude(char ch)
        {
            return char.IsLetter(ch) || ch == '-' || ch == '\'';
        }
    }
}
