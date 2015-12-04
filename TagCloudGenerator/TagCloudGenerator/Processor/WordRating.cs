namespace TagCloudGenerator.Processor
{
    class WordRating
    {
        public readonly string Word;
        public readonly int OccurencesCount;

        public WordRating(string word, int occurencesCount)
        {
            Word = word;
            OccurencesCount = occurencesCount;
        }
    }
}
