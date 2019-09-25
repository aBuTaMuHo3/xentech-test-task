using System;
namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IWorkoutExclusionFeedbackVO
    {
        int[] UntrainableDomains { get; }

        bool ToLessWorkoutsRemaining { get; }

        bool ExcusionPossible { get; }

    }
}
