using System.Collections.Generic;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Settings.Enums;

namespace ExerciseEngine.Model.ValueObjects
{
    public class ExerciseInitVO : IExerciseInitVO
    {
		public int StartDifficulty { get; }
        public ExerciseMode Mode { get; }
        public Dictionary<ExerciseSettingsEnum, bool> Settings { get; }
        public bool WarmUpEnabled { get; }
        public bool StartWithTutorial { get; }
        public bool TutorialSystemEnabled { get; }
        public bool DictionaryAvailable { get; }

        public ExerciseInitVO(int startDifficulty, ExerciseMode mode, Dictionary<ExerciseSettingsEnum, bool> settings, bool warmUpEnabled, bool startWithTutorial, bool tutorialSystemEnabled = false, bool _dictionaryAvailable = false)
        {
            WarmUpEnabled = warmUpEnabled;
            StartWithTutorial = startWithTutorial;
            TutorialSystemEnabled = tutorialSystemEnabled;
            DictionaryAvailable = _dictionaryAvailable;
            StartDifficulty = startDifficulty;
            Mode = mode;
            Settings = settings;
        }
    }
}