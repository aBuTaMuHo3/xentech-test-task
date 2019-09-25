using ExerciseEngine.Model.Enum;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IWorkoutResultVO : IExerciseResultVO
    {
        int Result { get; }

        int ConfigVersion { get; }

        string Variant { get; }

        int TrainingId { get; }

        int SessionId { get; }

        int CycleId { get; }

        int WorkoutId { get; }

        long OnlineTimestamp { get; }

        void UpdateCycleId(int newCycleId);
    }
}