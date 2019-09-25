using System;
using System.Collections.Generic;

namespace ExerciseEngine.Language
{
    public interface IDictionaryManager
    {
        IWordDictionary TaskDictionary { get; }
        IWordDictionary CheckDictionary { get; }
        IWordDictionary Rejectedictionary { get; }
        void GetAllCharacters(Action<string> callback);
        void GetCharsAndSymbols(Action<Dictionary<char, int>> callback);
    }
}
