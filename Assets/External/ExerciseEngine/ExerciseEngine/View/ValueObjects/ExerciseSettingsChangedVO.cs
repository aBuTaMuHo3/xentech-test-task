using System.Collections.Generic;
using ExerciseEngine.Controller.ValueObjects.Interfaces;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.View.ValueObjects.Interfaces;

namespace ExerciseEngine.View.ValueObjects
{
    public class ExerciseSettingsChangedVO : IExerciseViewUpdateVO, IExerciseControllerUpdateVO
    {
        public ExerciseSettingsChangedVO(Dictionary<ExerciseSettingsEnum, bool> exerciseSettings)
        {
            ExerciseSettings = exerciseSettings;
        }

        public Dictionary<ExerciseSettingsEnum, bool> ExerciseSettings { get; }
    }
}
