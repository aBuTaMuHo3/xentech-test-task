namespace ExerciseEngine.Language
{
    public static class LanguageDatabaseConstants
    {
        public const uint MAX_WORD_LENGTH = 11;
        public const uint MIN_WORD_LENGTH = 3;
    }

    public enum LanguageDictionaryType
    {
        Task = 0,
        BlackList = 1,
        WhiteList = 2
    }
}
