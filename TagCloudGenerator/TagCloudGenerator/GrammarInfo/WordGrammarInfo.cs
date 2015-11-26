namespace TagCloudGenerator.GrammarInfo
{
    class WordGrammarInfo
    {
        public readonly string InitialForm;
        public readonly PartOfSpeech PartOfSpeech;

        public WordGrammarInfo(string initialForm, PartOfSpeech partOfSpeech)
        {
            InitialForm = initialForm;
            PartOfSpeech = partOfSpeech;
        }
    }
}
