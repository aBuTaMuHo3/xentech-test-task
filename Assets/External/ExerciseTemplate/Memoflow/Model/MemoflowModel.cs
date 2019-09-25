using System;
using System.Collections.Generic;
using System.Linq;
using ExerciseEngine.Colors;
using ExerciseEngine.Model;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Settings.Enums;
using ExerciseEngine.View.ValueObjects;
using ExerciseEngine.View.ValueObjects.Interfaces;
using Memoflow.Model.ValueObjects;
using SynaptikonFramework.Interfaces.DebugLog;
using SynaptikonFramework.Util;

namespace Memoflow.Model
{
    public class MemoflowModel : BaseExerciseModel
    {
        protected List<IRoundItem> _questQueue;
        private int _numSymbols;
        protected string _startTutorialTextId = "";
        protected MemoflowMatchType[] _matchTypes;
        private float[] _availableRotations = new float[] { 0f };
        private int[] _availableSymbols;
        private AvailableColors[] _availableColors;
        private int _colorIndex;

        private readonly Random _random;

        public MemoflowModel(IExerciseConfiguration config, IExerciseInitVO initVO, ILogger logger, IColorManagerInitializer colorManager) : base(config, initVO, logger)
        {
            _random = new Random();

            _availableColors = colorManager.GetRandomColors(colorManager.NumColors);
            _availableColors.Shuffle();
        }

        public override IRoundIndependentUpdateResultVO RoundIndependentUpdate(IRoundIndependentVO data)
        {
            if (data is NumAssetUpdate)
            {
                _numSymbols = ((NumAssetUpdate)data).NumSymbols;

                MemoflowConfiguration memoflowConfig = (MemoflowConfiguration)ExerciseConfiguration;
                SetupMatchTypes(memoflowConfig.StimuliDefinition);
                SetupAvailabilities();
            }
            return base.RoundIndependentUpdate(data);
        }

        public override void CreateRound(Action<IExerciseRoundDataVO> callback, IExerciseRoundConfigurationVO exerciseRoundConfiguration = null) {
            MemoflowConfiguration configuration = ExerciseConfiguration as MemoflowConfiguration;


            List<IRoundItem> correctAnswers;

            int numCards = configuration.Levels[CurrentDifficulty].NumCards;
            var currenQueueLength = _questQueue == null || (_questQueue != null && _questQueue.Count == 0) ? 
                0 : Math.Min(_questQueue.Count - 1, _questQueue.Count - 1 - numCards);

            string tutorialTextId = "";
            if (_lastRoundLevelChange == LevelState.NEW)
            {
                _questQueue = GetFirstSteps();
                tutorialTextId = _startTutorialTextId;
            }
            else
            {
                _questQueue.Add(CreateStep());
            }

            if (_lastRoundLevelChange != LevelState.EQUAL) {
                _colorIndex = CurrentDifficulty % _availableColors.Length;
                for (int i = currenQueueLength; i < _questQueue.Count; i++) {
                    ((MemoflowStepVO)_questQueue[i]).SymbolColor = _availableColors[_colorIndex];
                }
            }

            correctAnswers = new List<IRoundItem>() { new MemoflowSolution(((MemoflowStepVO)_questQueue[_questQueue.Count - 1]).Match(((MemoflowStepVO)_questQueue[_questQueue.Count - numCards]), _matchTypes)) };
                     

            _currentRound = new MemoflowRoundDataVO(_questQueue,correctAnswers,_lastRoundLevelChange, _warmUpState, configuration.GetTimeoutByLevel(CurrentDifficulty), numCards, _matchTypes, tutorialTextId);

            base.CreateRound(callback, exerciseRoundConfiguration);
        }

        protected List<IRoundItem> GetFirstSteps()
        {
            MemoflowConfiguration memoflowConfig = (MemoflowConfiguration)ExerciseConfiguration;
            //init initial Field
            int numFirststeps = memoflowConfig.Levels[CurrentDifficulty].NumCards;
            List<IRoundItem> firstSteps = new List<IRoundItem>();
            for (int i = 0; i < numFirststeps; i++)
            {
                if (i == 0)
                {
                    firstSteps.Add(new MemoflowStepVO(_availableSymbols[_random.Next(0, _availableSymbols.Length)],
                        _availableColors[_colorIndex], 
                        _availableColors[_random.Next(0, _availableColors.Length)], _availableRotations[_random.Next(0, _availableRotations.Length)]));
                }
                else
                {
                    double rnd = _random.NextDouble();
                    if (rnd < memoflowConfig.MatchProbability)
                    {
                        var validMatches = GetValidMatches();
                        MemoflowMatchType newMatch = validMatches[_random.Next(0, validMatches.Length)];
                        firstSteps.Add(CreateStepWithMatch(firstSteps[firstSteps.Count - 1] as MemoflowStepVO, newMatch));
                    }
                    else
                    {
                        firstSteps.Add(CreateStepWithMatch(firstSteps[firstSteps.Count - 1] as MemoflowStepVO, MemoflowMatchType.NO));
                    }
                }
            }
            return firstSteps;
        }

        protected IRoundItem CreateStep()
        {
            MemoflowConfiguration nBackConfig = (MemoflowConfiguration)ExerciseConfiguration;
            int numCards = nBackConfig.Levels[CurrentDifficulty].NumCards;
            double rnd = _random.NextDouble();
            MemoflowStepVO step = _questQueue[_questQueue.Count - numCards + 1] as MemoflowStepVO;
            if (rnd < nBackConfig.MatchProbability)
            {
                var validMatches = GetValidMatches();
                MemoflowMatchType newMatch = validMatches[_random.Next(0, validMatches.Length)];
                return CreateStepWithMatch(step as MemoflowStepVO, newMatch);
            }
            else
            {
                return CreateStepWithMatch(step as MemoflowStepVO, MemoflowMatchType.NO);
            }
        }

        private MemoflowMatchType[] GetValidMatches()
        {
            List<MemoflowMatchType> ret = new List<MemoflowMatchType>();
            if (_availableColors.Length > 1)
            {
                ret.Add(MemoflowMatchType.COLOR);
            }
            if (_availableSymbols.Length > 1)
            {
                ret.Add(MemoflowMatchType.SYMBOL);
            }
            //if(_availableRotations.Length > 1) ret.Add(MATCH_TYPE.ROTATION);
            return ret.ToArray();
        }

        protected virtual MemoflowStepVO CreateStepWithMatch(MemoflowStepVO nbs, MemoflowMatchType matchType)
        {
            MemoflowConfiguration memoflowConfig = (MemoflowConfiguration)ExerciseConfiguration;

            //create completely different step...
            int symbol = GetUniqueElement(_availableSymbols, nbs.SymbolId);
            AvailableColors borderColor = GetUniqueElement(_availableColors, nbs.BorderColor);
            float rotation = GetUniqueElement(_availableRotations, nbs.Rot);

            //...add a match if necessary
            if (matchType == MemoflowMatchType.COLOR)
                borderColor = nbs.BorderColor;
            if (matchType == MemoflowMatchType.SYMBOL)
                symbol = nbs.SymbolId;
            //if( matchType == MATCH_TYPE.ROTATION) rotation = nbs.rotation;

            //otherwise NO_MATCH was the case.          
            return new MemoflowStepVO(symbol, _availableColors[_colorIndex], borderColor, rotation);
        }

        override protected void EvaluateNormalAnswer(Action<IRoundEvaluationResultVO> callback, IAnswerVO answer, bool isTutorial = false) {
            bool wasCorrect;
            bool isTimeout;

            if (answer == null) {
                isTimeout = true;
                wasCorrect = false;
            } else {
                isTimeout = false;
                wasCorrect = ((MemoFlowAnswerVO)answer.Solution).Correct ? IsAnswerCorrect(answer) : !IsAnswerCorrect(answer);
            }

            if (wasCorrect && (_warmUpState != WarmUpState.Enabled)) {
                UpdateScore();
            }

            CompleteRound(wasCorrect, isTutorial);

            _currentRoundResult = new RoundEvaluationResultVO(
                wasCorrect,
                answer,
                _currentRound.Solutions,
                GetAllPropertiesUpdates(),
                isTimeout,
                _startReactionDateTime,
                1,
                true
            );

            _roundHistory.First(x => x.RoundDataVO == _currentRound).RoundResult.Add(_currentRoundResult);
            callback(_currentRoundResult);
        }

        protected override bool IsAnswerCorrect(IAnswerVO answer, bool isStepped = false, int stepNr = -1) {
            return !((MemoflowSolution)_currentRound.Solutions[0]).MatchTypes.Contains(MemoflowMatchType.NO);
        }

        protected void SetupMatchTypes(MemoflowStimuliDefinition stimuliDefinition)
        {
            switch (stimuliDefinition)
            {
                case MemoflowStimuliDefinition.SINGLE_SYMBOL:
                    _matchTypes = new MemoflowMatchType[] { MemoflowMatchType.SYMBOL };
                    _startTutorialTextId = "Memoflow_memorized";
                    break;
                case MemoflowStimuliDefinition.DUAL_COLOR:
                    _matchTypes = new MemoflowMatchType[] { MemoflowMatchType.SYMBOL, MemoflowMatchType.COLOR };
                    _startTutorialTextId = "Memoflow_memorized";
                    break;
                case MemoflowStimuliDefinition.ROTATION:
                    _availableRotations = new float[] { 0f, 90f, 180f, 270f };
                    _matchTypes = new MemoflowMatchType[] { MemoflowMatchType.SYMBOL };
                    _startTutorialTextId = "Memoflow_memorized";
                    break;
            }
        }

        private void SetupAvailabilities()
        {
            MemoflowConfiguration memoflowConfig = (MemoflowConfiguration)ExerciseConfiguration;

            if (Array.IndexOf(_matchTypes, MemoflowMatchType.SYMBOL) != -1)
            {
                if (memoflowConfig.MaxSymbols > _numSymbols)
                {
                    throw new Exception("MaxSymbols must not be bigger than _numSymbols");
                }

                var availableSymbols = new HashSet<int>();

                while (availableSymbols.Count < memoflowConfig.MaxSymbols)
                {
                    int symbolId = _random.Next(0, _numSymbols);
                    availableSymbols.Add(symbolId);
                }

                _availableSymbols = new int[availableSymbols.Count];
                availableSymbols.CopyTo(_availableSymbols);       
            }
            else
            {
                _availableSymbols = new int[] { _random.Next(0, memoflowConfig.MaxSymbols) };
            }
            if (Array.IndexOf(_matchTypes, MemoflowMatchType.COLOR) != -1)
            {
                // todo: Currently not used for gameplay, if turned on need to be defined how
                //_availableColors = new AvailableColors[memoflowConfig.MaxColors];
                //int numAddedColors = 0;
                //while (numAddedColors < memoflowConfig.MaxColors)
                //{
                //    // todo: might be wrong ... unity uses special border color list
                //    AvailableColors color = memoflowConfig.ColorNamesPerDifficulty[_random.Next(0, memoflowConfig.ColorNamesPerDifficulty.Length)];
                //    if (Array.IndexOf(_availableColors, color) == -1)
                //    {
                //        _availableColors[numAddedColors] = color;
                //        numAddedColors++;
                //    }
                //}
            }
            else
            {
                //_availableColors = new AvailableColors[] { AvailableColors.Orange };
            }
        }

        override public IExerciseViewUpdateVO BotAnswer
        {
            get
            {
                if (IsAnswerCorrect(null)) {
                    return new BaseAnswerVO(new MemoFlowAnswerVO(true));
                } else {
                    return new BaseAnswerVO(new MemoFlowAnswerVO(false));
                }
            }
        }

        //override public int BotRoundDelay => 1000;

        public override ExerciseIdEnum ExerciseId => ExerciseIdEnum.Memoflow;

        /// <summary>
        /// Gets the unique element.
        /// </summary>
        /// <returns>The unique element.</returns>
        /// <param name="source">Source.</param>
        /// <param name="curAttribute">Current attribute. if possible the return value is differnt to that</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private T GetUniqueElement<T>(T[] source, T curAttribute)
        {
            if (source.Length > 1)
            {
                int randomIdex;
                do
                {
                    randomIdex = _random.Next(0, source.Length);
                } while (Array.IndexOf(source, curAttribute) == randomIdex);
                return source[randomIdex];
            }
            else
            {
                return source[0];
            }
        }
    }
}
