using System;
using System.Collections.Generic;
using System.Linq;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.Interfaces;
using ExerciseEngine.Model.Multiplier;
using ExerciseEngine.Model.Tutorial;
using ExerciseEngine.Model.Tutorial.Interfaces;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Settings.Enums;
using ExerciseEngine.Terminator.Triggers;
using ExerciseEngine.View.ValueObjects.Interfaces;
using SynaptikonFramework.Interfaces.DebugLog;

namespace ExerciseEngine.Model
{
    public abstract class BaseExerciseModel : IExerciseModel
    {
        public IExerciseInitVO ExerciseInitVO { get; protected set; }
        protected readonly ILogger _logger;
        protected readonly IMultiplier _multiplier;

        protected IExerciseRoundDataVO _currentRound;
        protected IRoundEvaluationResultVO _currentRoundResult;
        /** indicate how the level changed because the results of the previews round **/
        protected LevelState _lastRoundLevelChange;

        protected readonly List<FullRoundVO> _roundHistory = new List<FullRoundVO>();

        protected bool _disposed { get; private set; }
        private int _score;
        private readonly int _bonusScore;
        private int _currentDifficulty;
        private int _currentGoodRuns;
        private int _currentBadRuns;
        private int _totalGoodRuns;
        private int _startDifficulty;
        private bool _warmUpEnabeld;
        private int _warmUpRoundsCounter;
        protected DateTime _startReactionDateTime;

        protected Dictionary<ExerciseSettingsEnum, bool> _exerciseSettings;

        /// <summary>
        /// Holds the information about model properties changes
        /// </summary>
        private Dictionary<Type, IModelPropertyUpdateVO> _propertiesUpdates;
        public event TutorialTriggerHandler OnTutorialTrigger;

        protected WarmUpState _warmUpState;


        public virtual bool IsTimeoutEnabled
        {
            get
            {
                if (_warmUpState != WarmUpState.Enabled && CurrentRound.Timeout > 0)
                {
                    if (ExerciseConfiguration.EnforceTimeout)
                    {
                        return true;
                    }
                    
                    if(!(CurrentRound.LevelState == LevelState.NEW || CurrentRound.LevelState == LevelState.DOWN))
                    {
                        return true;
                    }
                }

                return false;
                //CurrentRound.Timeout > 0 && !(CurrentRound.LevelState == LevelState.NEW || CurrentRound.LevelState == LevelState.DOWN)
            }
        }

        public override string ToString()
        {
            return " <b><color=black>Model state:</color></b> <b>Score</b> => " + _score + ", <b>CurrDiff</b> => " + _currentDifficulty + ", <b>CurrGoodRuns</b> => " + _currentGoodRuns + ", <b>CurrBadRuns</b> => " + _currentBadRuns;
        }

        public BaseExerciseModel(IExerciseConfiguration config, IExerciseInitVO initVO, ILogger logger)
        {
            _logger = logger;
            ExerciseConfiguration = config;
            ExerciseInitVO = initVO;
            _exerciseSettings = initVO.Settings;

            // Init the property changes for tracking
            _propertiesUpdates = new Dictionary<Type, IModelPropertyUpdateVO>();

            _totalGoodRuns = 0;
            TotalRuns = 0;
            _currentGoodRuns = 0;
            _currentBadRuns = 0;
            GoodRunsInARow = 0;
            _warmUpRoundsCounter = 1;
            _warmUpEnabeld = initVO.WarmUpEnabled;

            _startDifficulty = (initVO.StartDifficulty > ExerciseConfiguration.MaxDifficulty) ? ExerciseConfiguration.MaxDifficulty : initVO.StartDifficulty;

#pragma warning disable RECS0021 // Warns about calls to virtual member functions occuring in the constructor
            _multiplier = InitMultiplier(_startDifficulty, ExerciseConfiguration);
#pragma warning restore RECS0021 // Warns about calls to virtual member functions occuring in the constructor
            // listen to multiplier changes
            _multiplier.OnChanged += OnMultiplierChanged;

#pragma warning disable RECS0021 // Warns about calls to virtual member functions occuring in the constructor
            _bonusScore = GetScoreForStartingDifficulty(_startDifficulty);
#pragma warning restore RECS0021 // Warns about calls to virtual member functions occuring in the constructor

            _currentDifficulty = _startDifficulty;

            AddPropertyUpdate(new BadRunsToLevelDownUpdateVO(ExerciseConfiguration.GetBadRunsByLevel(_startDifficulty), ExerciseConfiguration.GetBadRunsByLevel(_startDifficulty)));
            AddPropertyUpdate(new GoodRunsToLevelUpUpdateVO(ExerciseConfiguration.GetGoodRunsByLevel(_startDifficulty), ExerciseConfiguration.GetGoodRunsByLevel(_startDifficulty)));
            AddPropertyUpdate(new DifficultyUpdateVO(_startDifficulty, _startDifficulty));
            AddPropertyUpdate(new MaxDifficultyUpdateVO(ExerciseConfiguration.MaxDifficulty, ExerciseConfiguration.MaxDifficulty));

            _warmUpState = _warmUpEnabeld && (ExerciseConfiguration.GetWarmUpRoundsByLevel(_currentDifficulty) > 0)?WarmUpState.Enabled:WarmUpState.Disabled;

            if(_warmUpState == WarmUpState.Enabled){
                var originalValue = ExerciseConfiguration.GetWarmUpRoundsByLevel(_currentDifficulty);
                AddPropertyUpdate(new WarmUpUpdateVO(originalValue, originalValue, originalValue));
            }

            _lastRoundLevelChange = LevelState.NEW;
        }

        #region Interface Implementation

        #pragma warning disable CS1998
        /// <inheritdoc />
        public virtual void CreateRound(Action<IExerciseRoundDataVO> callback, IExerciseRoundConfigurationVO exerciseRoundConfiguration = null)
        {
            // Remove empty rounds from the history (might be created by tutorial)
            if(_roundHistory.Count > 0 && _roundHistory[_roundHistory.Count - 1].RoundResult.Count == 0)
            {
                _roundHistory.RemoveAt(_roundHistory.Count - 1);
            }

            if ( _lastRoundLevelChange == LevelState.NEW && CurrentDifficulty == 0)
            {
                TriggerTutorial(new StartupTutorialTrigger());
            }

            _currentRoundResult = null;
            _currentRound.Difficulty = _currentDifficulty;
            _roundHistory.Add(new FullRoundVO(_currentRound));
            callback(_currentRound);
        }

        public virtual void TriggerTutorial(ITutorialTrigger trigger)
        {
            OnTutorialTrigger?.Invoke(trigger);
        }

        /// <inheritdoc />
        public virtual void EvaluateAnswer(Action<IRoundEvaluationResultVO> callback, IAnswerVO answer, bool isTutorial = false)
        {
            // In specific exercise make here a decision what kind of answer to evaluate.
            EvaluateNormalAnswer(callback, answer, isTutorial);
        }
        #pragma warning restore CS1998

        /// <inheritdoc />
        public virtual IRoundIndependentUpdateResultVO RoundIndependentUpdate(IRoundIndependentVO data)
        {
            return new RoundIndependentUpdateResultVO(GetAllPropertiesUpdates());
        }

        /// <inheritdoc />
        public IExerciseRoundDataVO CurrentRound => _currentRound;

        /// <inheritdoc />
        public virtual IRoundEvaluationResultVO CurrentRoundResult => _currentRoundResult;

        /// <inheritdoc />
        public IExerciseConfiguration ExerciseConfiguration { get; }

        /// <inheritdoc />
        public virtual IExerciseResultVO Stop()
        {
            // store results locally
            return Result;
        }

        public virtual void StartMonitorReactionTime(DateTime date)
        {
            _startReactionDateTime = date;
        }

        /// <summary>
        /// Gives a answer that the bot will use to play alone
        /// </summary>
        /// <value>The bot answer.</value>
        virtual public IExerciseViewUpdateVO BotAnswer => (CurrentRound == null) ? null : new BaseAnswerVO(CurrentRound.Solutions[0]);

        /// <summary>
        /// Delay the boot use between rounds
        /// </summary>
        /// <value>The bot answer.</value>
        virtual public int BotRoundDelay => 500;

        abstract public ExerciseIdEnum ExerciseId { get; }

        #endregion

        /// <summary>
        /// Adds the property change for tracking model property changes.
        /// </summary>
        /// <param name="propertyUpdate">Property update.</param>
        protected void AddPropertyUpdate(IModelPropertyUpdateVO propertyUpdate)
        {
            // Update type
            Type updateType = propertyUpdate.GetType();

            if (_propertiesUpdates.ContainsKey(updateType))
            {
                // If that property update already exists sum value changes
                IModelPropertyUpdateVO currentPropertyUpdate = _propertiesUpdates[updateType];
                propertyUpdate.ValueChange += currentPropertyUpdate.ValueChange;
                if (propertyUpdate.ValueChange == 0)
                {
                    // If combined value change is 0 there is no property change, remove it from tracking
                    _propertiesUpdates.Remove(updateType);
                }
                else
                {
                    // If combined value change is still a change, update that property tracking
                    _propertiesUpdates[updateType] = propertyUpdate;
                }
            }
            else
            {
                // If property update is not yet tracked and its value really changed, track it
                if (propertyUpdate.ValueChange != 0)
                {
                    _propertiesUpdates[updateType] = propertyUpdate;
                }
            }
        }

        public IRoundEvaluationResultVO EvaluateNormalAnswer(IAnswerVO answer, bool isTutorial)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all properties updates, and resets tracking changes
        /// </summary>
        /// <returns>The all properties updates.</returns>
        protected List<IModelPropertyUpdateVO> GetAllPropertiesUpdates()
        {
            List<IModelPropertyUpdateVO> currentUpdates = new List<IModelPropertyUpdateVO>();
            foreach (KeyValuePair<Type, IModelPropertyUpdateVO> entry in _propertiesUpdates)
            {
                currentUpdates.Add(entry.Value);
            }
            _propertiesUpdates.Clear();
            return currentUpdates;
        }

        /// <summary>
        /// Inits the multiplier.
        /// </summary>
        /// <returns>The multiplier.</returns>
        /// <param name="startDifficulty">Start difficulty.</param>
        /// <param name="config">Exercise Config.</param>
        protected virtual IMultiplier InitMultiplier(int startDifficulty, IExerciseConfiguration config)
        {
            return new DynamicMultiplierConfig(startDifficulty, config.MultiplierSteps);
        }

        /// <summary>
        /// Gets the score for starting difficulty.
        /// This is the sum of the scores for each step, needed to reach the given difficulty level. 
        /// In the course starting in a higher difficulty means a score bonus at the start.
        /// 
        /// NOTE: init the multiplier before this function is called. It uses the multiplier settings in the model.
        /// 
        /// </summary>
        /// <returns>The score for starting difficulty.</returns>
        /// <param name="difficulty">Difficulty.</param>
        protected virtual int GetScoreForStartingDifficulty(int difficulty)
        {
            if (difficulty == -1) return 0;

            int score = 0;
            int curMultiplier = 1;
            int multiplierCounter = 0;
            int goodRuns = 0;
            int currentDifficulty = 0;
            //
            while (currentDifficulty < difficulty)
            {
                // Increase score.
                score += ExerciseConfiguration.GetScoresByLevel(currentDifficulty) * curMultiplier;

                // Check if multiplier should update.
                if (++multiplierCounter > _multiplier.CorrectStepsNeededForIncrease(curMultiplier) && curMultiplier < _multiplier.MultiplierMaxValue)
                {
                    curMultiplier += _multiplier.MultiplierIncrement;
                    multiplierCounter = 0;
                }

                // Check if difficulty should increase.
                if (++goodRuns >= ExerciseConfiguration.GetGoodRunsByLevel(currentDifficulty))
                {
                    currentDifficulty++;
                    if (_multiplier.ResetMultiplierWhenDifficultyIncreases)
                    {
                        multiplierCounter = 0;
                        curMultiplier = 1;
                    }
                    goodRuns = 0;
                }
            }
            return score;
        }

        public int BonusScore {
            get{
                return _bonusScore;
            }
        }

        /// <summary>
        /// Tracks the multiplier changes.
        /// </summary>
        /// <param name="multiplier">Current Multiplier value.</param>
        /// <param name="change">Multiplier Change value.</param>
        virtual protected void OnMultiplierChanged(int multiplier, int change)
        {
            // Track Multiplier value change
            AddPropertyUpdate(new MultiplierUpdateVO(multiplier, change));
        }

        /// <summary>
        /// Function checking if given answer was correct.
        /// </summary>
        /// <returns><c>true</c>, if answer correct was ised, <c>false</c> otherwise.</returns>
        /// <param name="answer">Answer.</param>
        /// <param name="isStepped">If set to <c>true</c> is stepped answer.</param>
        virtual protected bool IsAnswerCorrect(IAnswerVO answer, bool isStepped = false, int stepNr = -1)
        {
            var result = false;
            
            if(answer == null){
                return result;
            }
            // Handling "multiple-step-round" exercises
            if (isStepped)
            {
                // if stepNr = -1, use the stepnr from the currentround or 0 (first step), if not we force it to be a stepnr use that
                var fallbackStep = _currentRoundResult?.CurrentStep ?? 0;
                var step = stepNr == -1 ? fallbackStep : stepNr;
                var solution = _currentRound.Solutions[step];
                result = answer.Solution.Equals(solution);
            }
            else
            {
                result = _currentRound.Solutions.Contains(answer.Solution);
            }
             
            // Handling "one-step-round" exercises
            return result;
        }

        /// <summary>
        /// Evaluates the normal answer.
        /// </summary>
        /// <returns>The normal answer.</returns>
        /// <param name="answer">Answer.</param>
        /// <param name="isTutorial">If set to <c>true</c> is tutorial.</param>
        protected virtual void EvaluateNormalAnswer(Action<IRoundEvaluationResultVO> callback, IAnswerVO answer, bool isTutorial = false)
        {
            int oldPoints = Score;
            bool wasCorrect = IsAnswerCorrect(answer);

            if (wasCorrect && (_warmUpState != WarmUpState.Enabled)) {
                UpdateScore();
            }

            CompleteRound(wasCorrect, isTutorial);

            _currentRoundResult = new RoundEvaluationResultVO(
                wasCorrect,
                answer,
                _currentRound.Solutions,
                GetAllPropertiesUpdates(),
                answer == null,
                _startReactionDateTime,
                1,
                true
            );

            _roundHistory.First(x => x.RoundDataVO == _currentRound).RoundResult.Add(_currentRoundResult);
            callback(_currentRoundResult);
        }

        /// <summary>
        /// Evaluates the stepped answer.
        /// </summary>
        /// <returns>The stepped answer.</returns>
        /// <param name="answer">Answer.</param>
        /// <param name="isTutorial">If set to <c>true</c> is tutorial.</param>
        protected virtual void EvaluateSteppedAnswer(Action<IRoundEvaluationResultVO> callback, IAnswerVO answer, bool isTutorial = false, int stepNr = -1)
        {
            int oldPoints = Score;
            bool wasCorrect = IsAnswerCorrect(answer, true, stepNr);
            int finishedStepsNr = CalculateFinishedStepsNr();
            bool roundCompleted = RoundCompleted(wasCorrect, finishedStepsNr);

            if (wasCorrect && (_warmUpState != WarmUpState.Enabled))
            {
                UpdateScore();
                UpdateStepGoodrunPercent(finishedStepsNr);
            }
            if (roundCompleted)
            {
                // adjust whole round result if needed
                CompleteRound(IsWholeRoundCorrect(wasCorrect), isTutorial);
            }

            _currentRoundResult = new RoundEvaluationResultVO(
                wasCorrect,
                answer,
                _currentRound.Solutions,
                GetAllPropertiesUpdates(),
                answer == null,
                _startReactionDateTime,
                finishedStepsNr,
                roundCompleted
            );

            // reset _startReactionDateTime so we have a correct reaction time in between step answers if the round is not completed
            if(!roundCompleted)
                StartMonitorReactionTime(DateTime.Now);

            _roundHistory.First(x => x.RoundDataVO == _currentRound).RoundResult.Add(_currentRoundResult);
            callback(_currentRoundResult);
        }

        /// <summary>
        /// Calculates the current step.
        /// use this carefully
        /// </summary>
        /// <returns>The current step.</returns>
        protected virtual int CalculateFinishedStepsNr()
        {
            return (_currentRoundResult != null) ? _currentRoundResult.CurrentStep + 1 : 1;
        }

        protected virtual void UpdateStepGoodrunPercent(int currentStep)
        {
            //update substep good run for the hud 
            float percent = (float)CurrentGoodRuns / (float)ExerciseConfiguration.GetGoodRunsByLevel(CurrentDifficulty);
            //add the porcent for the step
            percent += ((float)currentStep / (float)_currentRound.Solutions.Count) / (float)ExerciseConfiguration.GetGoodRunsByLevel(CurrentDifficulty);
        }

        /// <summary>
        /// In some stepped answer exercises we need to evaluate all steps to see if the round results in a good run
        /// Override this method to adjust that value here before it gets passed into completeRound 
        /// </summary>
        /// <param name="wasLastAnswerCorrect">The last round result</param>
        /// <returns>As default the evaluation of the last step</returns>
        protected virtual bool IsWholeRoundCorrect(bool wasLastAnswerCorrect)
        {
            return wasLastAnswerCorrect;
        }

        /// <summary>
        /// Function checking if round is completed
        /// </summary>
        /// <returns><c>true</c>, if completed was rounded, <c>false</c> otherwise.</returns>
        /// <param name="wasCorrect">If set to <c>true</c> was correct.</param>
        /// <param name="finishedStepsNr">Current step.</param>
        protected virtual bool RoundCompleted(bool wasCorrect, int finishedStepsNr)
        {
            return !wasCorrect || finishedStepsNr == _currentRound.Solutions.Count;
        }

        /// <summary>
        /// Completes the round.
        /// </summary>
        /// <param name="wasCorrect">If set to <c>true</c> was correct.</param>
        /// <param name="isTutorial">If set to <c>true</c> is tutorial.</param>
        protected virtual void CompleteRound(bool wasCorrect, bool isTutorial)
        {
            if(_warmUpState == WarmUpState.Enabled){
                WarmUpRoundsCounter++;
                _lastRoundLevelChange = LevelState.EQUAL;
                return;
            }
            if (_warmUpEnabeld)
            {
                WarmUpRoundsCounter++;
            }

            if (wasCorrect)
            {
                _multiplier.OnCorrectSolution((ExerciseConfiguration.GetGoodRunsByLevel(CurrentDifficulty) - 1) == CurrentGoodRuns);
                GoodRunsInARow++;
                TotalGoodRuns++;
            }
            else
            {
                _multiplier.OnWrongSolution();
                GoodRunsInARow = 0;
                AddPropertyUpdate(new TotalBadRunsUpdateVO(TotalRuns + 1 - TotalGoodRuns, 1));
            }
            TotalRuns++;
            if (!isTutorial)
            {
                UpdateDifficulty(wasCorrect);
            }
            else
            {
                _lastRoundLevelChange = LevelState.EQUAL;
            }
        }

        /// <summary>
        /// Updates the score depending on the answer.
        /// </summary>
        /// <param name="increase">If set to <c>true</c> increase.</param>
        protected virtual void UpdateScore(bool increase = true)
        {
            int scoreAdd = ExerciseConfiguration.GetScoresByLevel(_currentDifficulty);
            if (scoreAdd == -1) return;
            if (increase)
            {
                Score += scoreAdd * _multiplier.MultiplierValue;
            }
            else
            {
                Score -= scoreAdd * _multiplier.MultiplierValue;
            }
        }

        /// <summary>
        /// Updates the difficulty depending if answer was correct or not.
        /// </summary>
        /// <param name="correct">If set to <c>true</c> correct.</param>
        protected virtual void UpdateDifficulty(bool correct)
        {
            if (!correct)
            {
                // reset current good runs
                CurrentGoodRuns = 0;

                // both conditions have to be combined in one if to have only one else condition
                if ((++CurrentBadRuns >= ExerciseConfiguration.GetBadRunsByLevel(CurrentDifficulty)) && (CurrentDifficulty > ExerciseConfiguration.MinDifficulty))
                {
                    CurrentBadRuns = 0;
                    CurrentDifficulty--;
                }
                else
                {
                    _lastRoundLevelChange = LevelState.EQUAL;
                }
            }
            else
            {
                CurrentBadRuns = 0;

                // both conditions have to be combined in one if to have only one else condition
                if ((++CurrentGoodRuns >= ExerciseConfiguration.GetGoodRunsByLevel(CurrentDifficulty)) && (CurrentDifficulty < ExerciseConfiguration.MaxDifficulty))
                {
                    CurrentGoodRuns = 0;
                    CurrentDifficulty++;
                }
                else
                {
                    _lastRoundLevelChange = LevelState.EQUAL;
                }
            }
            _logger.LogMessage(LogLevel.Informational, "Last round level change: " + _lastRoundLevelChange.ToString());
        }

        protected int CurrentDifficulty
        {
            get { return _currentDifficulty; }

            set
            {
                if (_currentDifficulty == value)
                {
                    _lastRoundLevelChange = LevelState.EQUAL;
                }
                else
                {
                    if (value > _currentDifficulty)
                    {
                        _lastRoundLevelChange = LevelState.UP;
                    }
                    else
                    {
                        _lastRoundLevelChange = LevelState.DOWN;
                    }

                    OnLevelChange();
                }
                // Track CurrentDifficultyLevel value change
                AddPropertyUpdate(new BadRunsToLevelDownUpdateVO(ExerciseConfiguration.GetBadRunsByLevel(value), ExerciseConfiguration.GetBadRunsByLevel(value) - ExerciseConfiguration.GetBadRunsByLevel(CurrentDifficulty)));
                AddPropertyUpdate(new GoodRunsToLevelUpUpdateVO(ExerciseConfiguration.GetGoodRunsByLevel(value), ExerciseConfiguration.GetGoodRunsByLevel(value) - ExerciseConfiguration.GetGoodRunsByLevel(CurrentDifficulty)));
                AddPropertyUpdate(new DifficultyUpdateVO(value, value - _currentDifficulty)); 
   
                _currentDifficulty = value;

                _logger.LogMessage(LogLevel.Informational, "Set update diff vo: " + _lastRoundLevelChange.ToString());
            }
        }

        /// <summary>
        /// Function called when level is changed
        /// </summary>
        protected virtual void OnLevelChange()
        {
            _multiplier.CurrentDifficulty = _currentDifficulty;
        }

        protected int Score
        {
            get { return _score; }

            set
            {
                _logger.LogMessage(LogLevel.Informational, "SET SCORE TO " + value);
                // Track Score value change
                AddPropertyUpdate(new ScoreUpdateVO(value, value - _score));
                _score = value;
            }
        }

        protected int CurrentGoodRuns
        {
            get { return _currentGoodRuns; }

            set
            {
                // Track CurrentGoodRuns value change
                AddPropertyUpdate(new GoodRunsUpdateVO(value, value - _currentGoodRuns));
                _currentGoodRuns = value;
            }
        }

        protected int CurrentBadRuns
        {
            get { return _currentBadRuns; }

            set
            {
                // Track CurrentBadRuns value change
                AddPropertyUpdate(new BadRunsUpdateVO(value, value - _currentBadRuns));
                _currentBadRuns = value;
            }
        }

        protected int WarmUpRoundsCounter
        {
            get { return _warmUpRoundsCounter; }

            set
            {
                if (_warmUpEnabeld && value <= ExerciseConfiguration.GetWarmUpRoundsByLevel(_currentDifficulty))
                {
                    _warmUpState = WarmUpState.Enabled;
                }
                else if (_warmUpEnabeld && ExerciseConfiguration.GetWarmUpRoundsByLevel(_currentDifficulty) != 0 && value == ExerciseConfiguration.GetWarmUpRoundsByLevel(_currentDifficulty) + 1)
                {
                    _warmUpState = WarmUpState.JustCompleted;
                }
                else
                {
                    _warmUpState = WarmUpState.Disabled;
                    _warmUpEnabeld = false;
                }

                // Track WarmUpRoundsCounter value change
                if (_warmUpState == WarmUpState.Enabled || _warmUpState == WarmUpState.JustCompleted)
                {
                    var originalValue = ExerciseConfiguration.GetWarmUpRoundsByLevel(_currentDifficulty);
                    AddPropertyUpdate(new WarmUpUpdateVO(originalValue - (value - 1), -(value - _warmUpRoundsCounter), originalValue));
                }
                _warmUpRoundsCounter = value;
            }
        }

        protected int GoodRunsInARow { get; set; }

        protected int TotalGoodRuns
        {
            get
            {
                return _totalGoodRuns;
            }

            set
            {
                AddPropertyUpdate(new TotalGoodRunsUpdateVO(value, value - _currentGoodRuns));
                _totalGoodRuns = value;
                _logger.LogMessage(LogLevel.Informational, "Total goood runs: " + _totalGoodRuns);
            }
        }

        protected int TotalRuns { get; set; }

        protected float Accuracy => TotalRuns > 0f ? (float)(_totalGoodRuns) / (TotalRuns) : 0f;

        public Dictionary<ExerciseSettingsEnum, bool> ExerciseSettings => new Dictionary<ExerciseSettingsEnum, bool>(_exerciseSettings);

        protected IExerciseResultVO Result => new ExerciseResultVO(
            Score,
            Accuracy,
            _bonusScore,
            _roundHistory,
            _currentDifficulty
        );

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // stop the GC clearing us up, 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Free any unmanaged objects here.
                _disposed = true;
                _roundHistory.Clear();
                // Free any other managed objects here.
                _logger.LogMessage(LogLevel.Informational, "Dispose called");
            }
        }
    }
}