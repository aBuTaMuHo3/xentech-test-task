using System;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    public class WorkoutDomainWeightVO : IWorkoutDomainWeightVO
    {
        public WorkoutDomainWeightVO(int domainId, int workoutId, float weight)
        {
            DomainId = domainId;
            WorkoutId = workoutId;
            Weight = weight;
        }

        public int DomainId { get; }
        public int WorkoutId { get; }
        public float Weight { get; }
    }
}
