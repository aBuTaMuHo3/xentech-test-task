using System;
using System.Collections.Generic;
using ExerciseEngine.Settings.Enums;

namespace ExerciseEngine.Model.Multiplier
{
    /// <summary>
    /// Multiplier config with non regular multiplier steps and max amount per level
    /// In the balancing tool add a array with needed good runs for the next multiplier level, eg: [15,15]
    /// Maximum multiplier is the length of that array + 1 (base multiplier is 1, so the first number is the needed good runs to reach multiplier 2 and so on)
    /// Inside this class its represented by int[][] to have the values for every level
    /// </summary>
    public class DynamicMultiplierConfig:IMultiplier
    {
        private int _currentDifficult;

        private int _multiplierCorrectStepCounter;

        private int _multiplierMaxValue;

        private int _firstLevelWithMultiplier;

        /// <summary>
        /// The initial value of the multiplier, default is 1
        /// </summary>
        private int _scoreMultiplier;

        /// <summary>
        /// if true, the multiplier is set to 1 if the difficulty rises one step. This is suggested.
        /// </summary>
        private bool _resetMultiplierOnDifficultyUp;

        private int[][] _stepsIncreaseByLevel;

        private int[] _stepsIncrease;

        /// <summary>
        /// Initializes a new instance of the DynamicMultiplierConfig class.
        /// </summary>
        /// <param name="startDifficulty">Start difficulty.</param>
        /// <param name="stepsIncreaseByLevel">Steps increase by level.</param>
        /// <param name="resetGoodRunsOnFirstMultiplierStage">If true _multiplierCorrectStepCounter will be reset 
        /// when the first level with available multiplier is reached to not immediately enable the multiplier.</param>
        public DynamicMultiplierConfig(int startDifficulty, int[][] stepsIncreaseByLevel, bool resetGoodRunsOnFirstMultiplierStage = true)
        {
            _stepsIncreaseByLevel = stepsIncreaseByLevel;
            _scoreMultiplier = 1;
            _multiplierCorrectStepCounter = 0;
            _currentDifficult = startDifficulty;
            _resetMultiplierOnDifficultyUp = false;
            _stepsIncrease = stepsIncreaseByLevel[startDifficulty];
            _multiplierMaxValue = _stepsIncrease.Length + 1;
            if (resetGoodRunsOnFirstMultiplierStage)
            {
                for (var i = 0; i < stepsIncreaseByLevel.Length; i++) {
                    if (stepsIncreaseByLevel[i].Length > 0)
                    {
                        _firstLevelWithMultiplier = i;
                        break;
                    }
                }
            }
            else
            {
                _firstLevelWithMultiplier = -1;
            }
        }

		public event ChangeHandler OnChanged;

        public void OnWrongSolution()
        {
            _multiplierCorrectStepCounter = 0;
            int oldVal = _scoreMultiplier;
            _scoreMultiplier = 1;
            if (oldVal > 1)
            {
                if (OnChanged != null)
                {
                    OnChanged(_scoreMultiplier, _scoreMultiplier - oldVal);
                }
            }
        }

        public void OnCorrectSolution(bool levelIncrease)
        {
            if (++_multiplierCorrectStepCounter > CorrectStepsNeededForIncrease(_scoreMultiplier) // Multiplier Increase Steps
                && (_scoreMultiplier + MultiplierIncrement) <= _multiplierMaxValue // Multiplier Max Value
            )
            {
                _scoreMultiplier += MultiplierIncrement;
                _multiplierCorrectStepCounter = 0;
                if (!(levelIncrease && _resetMultiplierOnDifficultyUp))
                { // anticipated level increase.
                    if (OnChanged != null)
                    {
                        OnChanged(_scoreMultiplier, MultiplierIncrement);
                    }
                }
            }
        }


        public int MultiplierValue { get { return _scoreMultiplier; } }

        public int MultiplierIncrement { get { return 1; } }

        public bool ResetMultiplierWhenDifficultyIncreases { get { return _resetMultiplierOnDifficultyUp; } }

        public int MultiplierMaxValue { get { return _multiplierMaxValue; } }


        public int CorrectStepsNeededForIncrease(int multiplierValue)
        {
            int numGoodRuns;
            if (_stepsIncrease.Length == 0)
            {
                numGoodRuns = int.MaxValue;
            }
            else if (_stepsIncrease.Length < multiplierValue)
            {
                numGoodRuns = _stepsIncrease[_stepsIncrease.Length - 1];
            }
            else
            {
                numGoodRuns = _stepsIncrease[multiplierValue - 1];
            }
            return numGoodRuns;
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
                _stepsIncrease = _stepsIncreaseByLevel[_currentDifficult];
                _multiplierMaxValue = _stepsIncrease.Length + 1;
                if (_firstLevelWithMultiplier != -1 && _firstLevelWithMultiplier == _currentDifficult)
                {
                    _multiplierCorrectStepCounter = 0;
                }
            }
        }
    }
}