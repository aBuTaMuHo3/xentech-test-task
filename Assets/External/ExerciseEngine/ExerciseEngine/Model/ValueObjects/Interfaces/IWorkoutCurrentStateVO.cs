using System;
namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IWorkoutCurrentStateVO
    {
        int WorkoutId { get; }

        int Goal { get; }

        /// <summary>
        /// Gets the reached difficulty.
        /// </summary>
        /// <value>Difficulty reached by the user.</value>
        int ReachedDifficulty { get; }

        /// <summary>
        /// Gets the start difficulty.
        /// </summary>
        /// <value>Difficulty that workout will be started with.</value>
        int StartDifficulty { get; }

        int GoalForStartDifficuty(int difficulty);

        long Timestamp { get; }
    }
}
