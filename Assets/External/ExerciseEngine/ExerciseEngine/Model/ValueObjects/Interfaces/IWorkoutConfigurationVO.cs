using ExerciseEngine.Goal;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Settings.Enums;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IWorkoutConfigurationVO
    {
        int WorkoutId { get; }

        string ExerciseId { get; }

        int ConfigVersion { get; }

        string Type { get; }

        string Variant { get; }

        ExerciseMode Mode { get; }

        string JsonId { get; }

        GoalType GoalType{ get; }

        int Duration { get; }

        ExerciseCategory Category { get;}

        int CategoryGroupId { get; }
    }
}
