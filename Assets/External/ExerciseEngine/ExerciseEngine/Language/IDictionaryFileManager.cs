using System;
using System.Threading.Tasks;

namespace ExerciseEngine.Language
{
    public interface IDictionaryFileManager
    {
        string DatabasePath { get; }
        string DatabaseName { get; }

        bool DatabaseExists { get; }
        Task<UpdateDictionaryResult> UpdateAsync();
    }

    public enum UpdateDictionaryResult
    {
        UPDATED,
        NOT_UPDATED,
        UP_TO_DATE,
        ERROR
    }
}
