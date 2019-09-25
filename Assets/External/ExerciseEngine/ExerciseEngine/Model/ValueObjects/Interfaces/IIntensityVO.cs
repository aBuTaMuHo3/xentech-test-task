using System;
namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IIntensityVO
    {
        int SessionAmountInCycle { get; set; }

        int IntensityId { get; set; }

        int WorkoutAmountInsession { get; set; }

        int CycleDurationDays { get; set; }
    }
}
