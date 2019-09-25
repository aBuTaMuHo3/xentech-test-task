using ExerciseEngine.Model.Enum;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IExerciseResultVO
    {
        /// <summary>
        /// Number of good runs for this exercise
        /// </summary>
        /// <value>The good runs.</value>
        int GoodRuns { get; }

        /// <summary>
        /// Number of bad runs for this exercise
        /// </summary>
        /// <value>The bad runs.</value>
        int BadRuns { get; }

        string GameHistory { get; }

        /// <summary>
        /// Difficulty of the exercise in the moment it was finished.
        /// </summary>
        /// <value>The current difficulty.</value>
        int CurrentDifficulty { get; }

        int StartDifficulty { get; }

        /// <summary>
        /// Difficulty on which first error has been made.
        /// </summary>
        /// <value>The first error difficulty.</value>
        int FirstErrorDifficulty { get; }

        /// <summary>
        /// Max reached difficulty during exercise.
        /// </summary>
        /// <value>The max difficulty reached.</value>
        int MaxDifficulty { get; }

        /// <summary>
        /// Gets the lowest difficulty.
        /// </summary>
        /// <value>The lowest difficulty.</value>
        int MinDifficulty { get; }

        /// <summary>
        /// Mean Difficulty during exercise.
        /// </summary>
        /// <value>The mean difficulty.</value>
        float MeanDifficulty { get; }

        /// <summary>
        /// Gets the relation of correct answers to total answers.
        /// </summary>
        /// <value>The accuracy.</value>
        float Accuracy { get; }

        /// <summary>
        /// Gets the score reached in this exercise.
        /// </summary>
        /// <value>The score.</value>
        int Score { get; }

        int FinalScore { get; }

        /// <summary>
        /// Gets the bonus score in this exercise.
        /// In the past this was given as "start score"
        /// Is needed to have a compareabele score, for runs with stiffent start-level
        /// </summary>
        /// <value>The score.</value>
        int BonusScore { get; }

        int ReactionTimeGoodRunAverage { get; }

        int ReactionTimeBadRunAverage { get; }

        int ReactionTimeAverage { get; }

        long FinishedTimestamp { get; }

        object RawData { get; }
    }
}