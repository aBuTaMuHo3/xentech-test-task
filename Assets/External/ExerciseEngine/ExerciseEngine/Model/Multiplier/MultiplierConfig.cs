using ExerciseEngine.Settings.Enums;

namespace ExerciseEngine.Model.Multiplier
{
    public class MultiplierConfig : IMultiplier
    {
        public event ChangeHandler OnChanged;

        /// <summary>
        /// The initial value of the multiplier, default is 1
        /// </summary>
        private int _scoreMultiplier;

        /// <summary>
        /// The value which is added to the current multiplier, when it is supposed to increase.
        /// </summary>
        private int _scoreMultiplierIncrease;

        /// <summary>
        /// The number of correct steps in a row after which the multiplier is increased.
        /// </summary>
        private int _multiplierIncreaseSteps;

        private int _multiplierMaxValue;

        /// <summary>
        /// if true, the multiplier is set to 1 if the difficulty rises one step. This is suggested.
        /// </summary>
        private bool _resetMultiplierOnDifficultyUp;

        private int _multiplierCorrectStepCounter;
        private int _currentDifficult;

        public MultiplierConfig(int startDifficulty, int multiplierIncreaseSteps = 0, int initialValue = 1, int multiplierIncrease = 0, bool resetMultiplierOnDifficultyUp = true, int maxValue = 1)
        {
            _multiplierIncreaseSteps = multiplierIncreaseSteps;
            _scoreMultiplier = initialValue;
            _scoreMultiplierIncrease = multiplierIncrease;
            _resetMultiplierOnDifficultyUp = resetMultiplierOnDifficultyUp;
            _multiplierMaxValue = maxValue;
            _currentDifficult = startDifficulty;
        }

        public int CurrentDifficulty
        {
            set
            {
                if (value > _currentDifficult)
                {
                    if (_resetMultiplierOnDifficultyUp && _scoreMultiplier >= 1)
                    {
                        int oldValue = _scoreMultiplier;
                        _scoreMultiplier = 1;
                        _multiplierCorrectStepCounter = 0;

                        if (OnChanged != null)
                        {
                            OnChanged(_scoreMultiplier, _scoreMultiplier - oldValue);
                        }
                    }
                }
                _currentDifficult = value;
            }
        }

        public void OnWrongSolution()
        {
            int oldValue = _scoreMultiplier;

            _multiplierCorrectStepCounter = 0;
            _scoreMultiplier = 1;

            if (oldValue > 1)
            {
                if (OnChanged != null) OnChanged(_scoreMultiplier, _scoreMultiplier - oldValue);
            }
        }

        public void OnCorrectSolution(bool levelIncrease)
        {
            if (++_multiplierCorrectStepCounter > _multiplierIncreaseSteps && (_scoreMultiplier + _scoreMultiplierIncrease) <= _multiplierMaxValue)
            {
                _scoreMultiplier += _scoreMultiplierIncrease;
                _multiplierCorrectStepCounter = 0;

                if (!(levelIncrease && _resetMultiplierOnDifficultyUp))
                {
                    if (OnChanged != null) OnChanged(_scoreMultiplier, _scoreMultiplierIncrease);
                }
            }
        }

        public int MultiplierValue { get { return _scoreMultiplier; } }
        public int MultiplierIncrement { get { return _scoreMultiplierIncrease; } }
        public int CorrectStepsNeededForIncrease(int multiplierValue)
        {
            return _multiplierIncreaseSteps;
        }
        public bool ResetMultiplierWhenDifficultyIncreases { get { return _resetMultiplierOnDifficultyUp; } }
        public int MultiplierMaxValue { get { return _multiplierMaxValue; } }
    }
}