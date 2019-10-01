using System;
using System.Collections.Generic;
using System.Linq;
using ExerciseEngine.Model;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Settings.Enums;
using SynaptikonFramework.Interfaces.DebugLog;
using SynaptikonFramework.Util.Math;
using FlashGlance.Model.ValueObjects;
using ExerciseEngine.Colors;
using SynaptikonFramework.Util;

namespace FlashGlance.Model
{
    public class FlashGlanceModel : BaseExerciseModel
    {
        /* current cypher to be looked at */
        private int _currentSolutionIndex;

        /* parser for mathematical expressions */
        private readonly ExpressionParser _expressionParser;

        /* hold all solutions  */
        private readonly List<FlashGlanceRoundItemVO> _solutionChain;

        /* hold which number are in wich field  */
        private readonly List<FlashGlanceRoundItemVO> _allItems;

        /* all possible positions */
        private readonly List<SafeHashCodePoint> _allPositions;

        private readonly HashSet<SafeHashCodePoint> _freePositions;

        /* last cypher created */
        private int _lastCypher;

        /* lid of the last item */
        private int _lastItemId;

        private readonly FlashGlanceConfiguration _castedConfig;

        private readonly Random _random;
        private readonly IColorManagerInitializer _colorManager;
        private readonly AvailableColors[] _colors;
        private int _colorIndex;

        private int _minItemAmount;

        public const int FORECAST_SIZE = 2;
        public const int HISTORY_SIZE = 0;

        public FlashGlanceModel(IExerciseConfiguration config, IExerciseInitVO initVO, ILogger logger, IColorManagerInitializer colorManager) : base(config, initVO, logger)
        {
            _castedConfig = ExerciseConfiguration as FlashGlanceConfiguration;

            _currentSolutionIndex = 0;

            _lastItemId = 0;

            _expressionParser = new ExpressionParser();

            _solutionChain = new List<FlashGlanceRoundItemVO>();
            _allItems = new List<FlashGlanceRoundItemVO>();
            _allPositions = new List<SafeHashCodePoint>();
            _freePositions = new HashSet<SafeHashCodePoint>();
            for (int i = 0; i < _castedConfig.MapWidth; i++)
            {
                for (int j = 0; j < _castedConfig.MapHeight; j++)
                {
                    _allPositions.Add(new SafeHashCodePoint(i, j));
                    _freePositions.Add(new SafeHashCodePoint(i, j));
                }
            }

            _random = new Random();

            var startCyphers = _castedConfig.GetStartingCyphersByLevel(initVO.StartDifficulty);
            _lastCypher = _random.Next(startCyphers.Min, startCyphers.Max + 1);

            _minItemAmount = _castedConfig.GetStartingItemsByLevel(initVO.StartDifficulty);
            _colorManager = colorManager;

            _colors = _colorManager.GetRandomColors(_colorManager.NumColors);
            _colors.Shuffle();
        }

        public override void CreateRound(Action<IExerciseRoundDataVO> callback, IExerciseRoundConfigurationVO exerciseRoundConfiguration = null)
        {
            ValidateMinItemsOnRound();

            int timeout = ExerciseConfiguration.GetTimeoutByLevel(CurrentDifficulty);

            List<IRoundItem> solutions = new List<IRoundItem> {
                _solutionChain[_currentSolutionIndex]
            };

            if (_lastRoundLevelChange != LevelState.EQUAL) {
                _colorIndex = CurrentDifficulty % _colors.Length;
                for (int i = _solutionChain.IndexOf(_allItems[0]); i < _solutionChain.Count; i++)
                {
                    _solutionChain[i].Color = _colors[_colorIndex];
                }
            }

            _currentRound = new FlashGlanceRoundDataVO(CloneItems(_allItems),
                                                       solutions,
                                                       _lastRoundLevelChange,
                                                       _warmUpState,
                                                       timeout,
                                                       _solutionChain, 
                                                       _currentSolutionIndex,
                                                       _castedConfig.MapHeight,
                                                       _castedConfig.MapWidth);

            base.CreateRound(callback, exerciseRoundConfiguration);
        }

        private List<FlashGlanceRoundItemVO> CloneItems(List<FlashGlanceRoundItemVO> orginal){
            List<FlashGlanceRoundItemVO> clone = new List<FlashGlanceRoundItemVO>();
            foreach (var item in orginal){
                clone.Add(new FlashGlanceRoundItemVO(item.Id, item.Cypher, item.GridPosition, item.Rotation, item.Scale, _colors[_colorIndex]));
            }
            return clone;
        }

        /* fill the map to have the min items on  */
        private void ValidateMinItemsOnRound()
        {
            int missingItems;
            if (_lastRoundLevelChange == LevelState.NEW)
            {
                missingItems = Math.Max(_castedConfig.GetStartingItemsByLevel(CurrentDifficulty), _minItemAmount) - _allItems.Count;
            }
            else
            {
                missingItems = Math.Max(_castedConfig.GetNumberOfItemsByLevel(CurrentDifficulty).Min, _minItemAmount) - _allItems.Count;
            }

            if (missingItems > 0)
            {
                //add the items are missed to complete them min setting
                for (int i = 0; i < missingItems; i++)
                {
                    AddItem();
                }
            }
        }

        protected override bool IsAnswerCorrect(IAnswerVO answer, bool isStepped = false, int stepNr = -1)
        {
            bool isCorrect = base.IsAnswerCorrect(answer, false, stepNr);
            if(answer == null || !isCorrect)
            {
                _allItems.Remove(_solutionChain[_currentSolutionIndex]);
                _currentSolutionIndex++;
            }
            else if (isCorrect)
            {
                var givenAnswer = answer.Solution as FlashGlanceRoundItemVO;
                _allItems.Remove(givenAnswer);
                _currentSolutionIndex++;
            }

            return isCorrect;
        }

        public override IRoundIndependentUpdateResultVO RoundIndependentUpdate(IRoundIndependentVO data)
        {
            if(data is FlashGlanceEventRequestVO){
                int i;
                switch(((FlashGlanceEventRequestVO)data).EventType){
                    case FlashGlanceEventType.ItemSpawning:
                        var spawnConfig = (_castedConfig.GetNewItemtemSpawningByLevel(CurrentDifficulty));
                        if (spawnConfig.Amount > 0 && _allItems.Count < MaxItems)
                        {
                            int itemsToAdd = Math.Min(MaxItems - _allItems.Count, spawnConfig.Amount);
                            for (i = 0; i < itemsToAdd; i++)
                            {
                                var newItem = AddItem();
                                _minItemAmount = newItem == null ? _minItemAmount : _minItemAmount + 1;
                            }
                        }
                        return new FlashGlanceEventResultVO(CloneItems(_allItems));

                    case FlashGlanceEventType.ItemMovement:
                        var moveConfig = (_castedConfig.GetItemMovingByLevel(CurrentDifficulty));
                        if(moveConfig.Amount > 0 && _allItems.Count < _allPositions.Count )
                        {
                            int itemsToMove = Math.Min(_allPositions.Count - _allItems.Count, moveConfig.Amount);
                            SafeHashCodePoint freePostion;
                            FlashGlanceRoundItemVO item;
                            for (i = 0; i < itemsToMove; i++)
                            {
                                freePostion = GetEmptyPosition();
                                item = GetQuiteItem();
                                if(!(freePostion == null || item == null) ){
                                    _freePositions.Remove(freePostion);
                                    _freePositions.Add(item.GridPosition);
                                    item.GridPosition = freePostion;
                                    item.IsBusy = true;
                                }
                            }
                        }
                        return new FlashGlanceEventResultVO(CloneItems(_allItems));

                    case FlashGlanceEventType.ItemSwitching:
                        var switchConfig = (_castedConfig.GetItemSwitchingByLevel(CurrentDifficulty));
                        if (switchConfig.Amount > 0 && _allItems.Count >= (switchConfig.Amount * 2) )
                        {
                            int pairsToSwitch = (int)Math.Min(Math.Floor(_allItems.Count / 2f), switchConfig.Amount);
                            FlashGlanceRoundItemVO item1;
                            FlashGlanceRoundItemVO item2;
                            for (i = 0; i < pairsToSwitch; i++)
                            {
                                item1 = GetQuiteItem();
                                item2 = GetQuiteItem(item1);
                                if (!(item1 == null || item2 == null))
                                {
                                    SafeHashCodePoint tempPosition = item1.GridPosition;
                                    item1.GridPosition = item2.GridPosition;
                                    item2.GridPosition = tempPosition;
                                    item1.IsBusy = true;
                                    item2.IsBusy = true;
                                }
                            }
                        }
                        return new FlashGlanceEventResultVO(CloneItems(_allItems));
                }
            }

            else if(data is FlashGlanceItemUnlockedVO)
            {
                var itemUnlocked = ((FlashGlanceItemUnlockedVO)data).Item;
                foreach (var item in _allItems){
                    if (item.Id == itemUnlocked.Id){
                        item.IsBusy = false;
                        //_logger.LogMessage(LogLevel.Informational, "Item unlocked " + item.Cypher);
                        break;
                    } 
                }
            }
            else if(data is FlashGlanceItemHiddenVO)
            {
                var itemHidden = ((FlashGlanceItemHiddenVO)data).Item;
                _freePositions.Add(itemHidden.GridPosition);
                _logger.LogMessage(LogLevel.Informational, "Item hidden " + itemHidden.Cypher);
            }
            return base.RoundIndependentUpdate(data);
        }

        /* adds a single new item on the map. if  */
        private FlashGlanceRoundItemVO AddItem()
        {
            if (_allItems.Count >= MaxItems) return null; //map is full
            SafeHashCodePoint emptyPosition = GetEmptyPosition();
            FlashGlanceRoundItemVO item = null;
            if (emptyPosition != null)
            {
                _freePositions.Remove(emptyPosition);
                int rotation = (_random.NextDouble() <= _castedConfig.GetRotationProbabilityByLevel(CurrentDifficulty)) ? Rotation2D.GetRandom(false) : Rotation2D.DEGREE_0;
                float scale = (float)((_random.NextDouble() <= _castedConfig.GetScalingProbabilityByLevel(CurrentDifficulty)) ? _random.NextDouble() * 0.3f + 0.7f : 1f);
                item = new FlashGlanceRoundItemVO(_lastItemId, _lastCypher, emptyPosition, rotation, scale, _colors[_colorIndex]);
                _allItems.Add(item);
                _solutionChain.Add(item);
                item.IsBusy = true;

                _lastItemId++;

                //generate next cypher
                string formula = _castedConfig.GetNextItemFormulaByLevel(CurrentDifficulty);
                Expression exp = _expressionParser.EvaluateExpression(formula);
                if (exp.Parameters.ContainsKey("x"))
                    exp.Parameters["x"].Value = _lastCypher; // set the named parameter "x"
                else
                    exp.Parameters.Add("x", new Parameter("x") { Value = _lastCypher});
                int formulaResult = (int)exp.Value;
                _lastCypher = (formulaResult > 0) ? formulaResult : _lastCypher + 1;

            }

            //_logger.LogMessage(LogLevel.Informational, "New item created: " + item.ToString());

            return item;
        }

        /* gets a random empty position of the map */
        private SafeHashCodePoint GetEmptyPosition()
        {
            //check if the max amount of elements where reached so to not waste time in impossible search
            if(_freePositions.Count <= 0) return null;
            var shuffeledPositions = _freePositions.OrderBy(a => _random.Next());
            return shuffeledPositions.ElementAt(0);
        }

        /* gets an random item from the map that is not busy */
        private FlashGlanceRoundItemVO GetQuiteItem(FlashGlanceRoundItemVO exceptionItem = null)
        {
            var shuffledItems = _allItems.OrderBy(a => _random.Next());
            foreach(var item in shuffledItems){

                if (!item.IsBusy && item != _solutionChain[_currentSolutionIndex] && item != exceptionItem) return item;
            }

            return null;
        }

        private int MaxItems
        {
            get
            {
                int max = Math.Min(_allPositions.Count, _castedConfig.GetNumberOfItemsByLevel(CurrentDifficulty).Max);
                return max;
            }
        }

        public override ExerciseIdEnum ExerciseId => ExerciseIdEnum.Flash_Glance;
    }

    public enum FlashGlanceEventType
    {
        ItemSpawning,
        ItemMovement,
        ItemSwitching
    }
}