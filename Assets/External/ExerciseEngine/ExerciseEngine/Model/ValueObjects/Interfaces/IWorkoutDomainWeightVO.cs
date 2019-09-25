using System;
namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IWorkoutDomainWeightVO
    {
        int DomainId { get; }
        int WorkoutId { get; }
        float Weight { get; }
    }
}
