namespace ExerciseEngine.Model.Multiplier
{
    public delegate void ChangeHandler(int scoreMultiplier, int change);

    public interface IMultiplier
    {
        event ChangeHandler OnChanged;

        /// <summary>
        /// Called when solution was wrong, adjust the mutliplier
        /// </summary>
        void OnWrongSolution();

        /// <summary>
        /// Called when solution was entered correctly, adjust the mutliplier
        /// </summary>
        /// <param name="levelIncrease"> If levelIncrease is anticipated and ResetMultiplierOnDifficultyUp is true, the dispatch of the signal is suppressed.</param>
        void OnCorrectSolution(bool levelIncrease);

        /// <summary>
        /// Called when difficulty is chenged, if resetMultiplierOnDifficultyIncrease is set, this might change the multiplier
        /// </summary>
        /// <value>The current difficulty.</value>
        int CurrentDifficulty { set; }

        /// <summary>
        /// Gets the multiplier value.
        /// </summary>
        /// <value>The multiplier value.</value>
        int MultiplierValue { get; }

        /// <summary>
        /// Gets the multiplier increment.
        /// </summary>
        /// <value>The multiplier increment.</value>
        int MultiplierIncrement { get; }

        /// <summary>
        /// Corrects the steps needed for increase multiplier for given multiplier value.
        /// </summary>
        /// <returns>The steps needed for increase.</returns>
        /// <param name="multiplierValue">Multiplier value.</param>
        int CorrectStepsNeededForIncrease(int multiplierValue);

        /// <summary>
        /// Gets a value indicating whether multiplier should be reseted on difficulty increase
        /// </summary>
        /// <value></value>
        bool ResetMultiplierWhenDifficultyIncreases { get; }

        /// <summary>
        /// Gets the multiplier max value.
        /// </summary>
        /// <value>The multiplier max value.</value>
        int MultiplierMaxValue { get; }
    }
}