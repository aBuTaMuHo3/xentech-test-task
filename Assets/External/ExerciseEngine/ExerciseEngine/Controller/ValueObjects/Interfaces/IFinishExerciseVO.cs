using System;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.Controller.ValueObjects.Interfaces
{
    public interface IFinishExerciseVO
    {
        IExerciseResultVO ExerciseResultVO { get; }

        bool Finished { get; }
    }
}
