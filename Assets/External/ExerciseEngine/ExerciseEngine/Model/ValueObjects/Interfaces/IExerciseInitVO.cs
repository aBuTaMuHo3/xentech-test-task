using System.Collections.Generic;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Settings.Enums;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IExerciseInitVO
    {
        int StartDifficulty { get; }
        ExerciseMode Mode { get; }
        bool WarmUpEnabled { get; }
        Dictionary<ExerciseSettingsEnum, bool> Settings { get; }
        bool StartWithTutorial { get; }
        bool TutorialSystemEnabled { get; } //Sascha it should return false for you to dont use new tutorial system
        bool DictionaryAvailable { get; }
    }
}