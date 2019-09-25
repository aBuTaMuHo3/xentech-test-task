using System;
using ExerciseEngine.Model.Enum;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface ISessionWorkoutVO
    {
        int TrainingId { get; }

        int SessionId { get; }

        long SyncedAt { get; set; }

        int WorkoutId { get; }

        int CycleId { get; }

        int UserId { get; }

        SessionStatusEnum Status { get; set; }

        IWorkoutConfigurationVO WorkoutConfiguration { get; }

        int Index { get; }

        void Update(ISessionWorkoutVO sessionWorkout);
    }
}
