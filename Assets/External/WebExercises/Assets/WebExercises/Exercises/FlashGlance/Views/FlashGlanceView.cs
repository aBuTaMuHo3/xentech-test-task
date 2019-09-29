using System.Threading.Tasks;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using FlashGlance.Model.ValueObjects;
using DG.Tweening;
using MinLibs.MVC;
using MinLibs.Signals;
using MinLibs.Utils;
using UnityEngine;
using System.Collections.Generic;
using SynaptikonFramework.Util.Math;


namespace WebExercises.FlashGlance
{
    // Scaffold of an possible solution for implementing the exercise view
    // Ideally there should be one prefab that works as the presenting component on top of the view and another prefab
    // for the single items (depends how you do it you can also use one for the grid layout).
    // On the long run the "slider component" on top should be reusable in different exercises
    public class FlashGlanceView: MediatedBehaviour, IFlashGlanceView
    {
        [SerializeField] private FlashGlanceItem itemPrefab;
        [SerializeField] private FlashGlanceSliderItem sliderItemPrefab;
        [SerializeField] private Transform slider;
        [SerializeField] private int sliderQueueLength;

        public Signal<IRoundItem> ItemSelected { get; } = new Signal<IRoundItem>();
        
        // Needs to be dispatched after each created round to switch to input state and process given answers
        // If there is some show animation it should be dispatched after they are completed
        public Signal RoundCreated { get; } = new Signal();
        
        // Needs to be dispatched after feedback was shown by the view to finish the current round and start a new one
        public Signal FeedbackShown { get; } = new Signal();
        
        // Exercise specific feature, needs to be dispatched after an item was removed to update the available item positions in the model
        public Signal<IRoundItem> ItemHidden { get; } = new Signal<IRoundItem>();

        private RectTransform _rect;

        private RectTransform Rect
        {
            get
            {
                _rect = _rect ?? (RectTransform)transform;
                return _rect;
            }
        }

        private FlashGlanceRoundDataVO _roundData;
        private readonly IDictionary<SafeHashCodePoint, FlashGlanceItem> _cachedField = new Dictionary<SafeHashCodePoint, FlashGlanceItem>();
        private readonly IDictionary<SafeHashCodePoint, FlashGlanceRoundItemVO> _cachedItems = new Dictionary<SafeHashCodePoint, FlashGlanceRoundItemVO>();
        private IList<FlashGlanceSliderItem> _sliderItems = new List<FlashGlanceSliderItem>();
        private FlashGlanceSliderItem _hiddenSliderItem;
        private int _fieldHeight;
        private int _fieldWidth;
        private float _gap;
        private float _itemSize;
        // Called on the very first round, for now here the initial elements can be initialized
        public void CreateInitialRound(IExerciseRoundDataVO dataVo)
        {
            _roundData = dataVo as FlashGlanceRoundDataVO;
            _fieldHeight = _roundData.NumberOfRows;
            _fieldWidth = _roundData.NumberOfColumns;
            _gap = Rect.rect.width/ _fieldWidth;
            var tempGap = Rect.rect.height / _fieldHeight;
            _gap = _gap > tempGap ? tempGap : _gap;
            _itemSize = _gap * 0.8f;
            System.Diagnostics.Debug.WriteLine("CreateInitialRound");

            foreach (FlashGlanceRoundItemVO item in _roundData.Items)
            {
                InitItem(item);
            }

            InitSlider();
            RoundCreated.Dispatch();

            //TestAnswer();
        }

        
        private void InitSlider()
        {
            for (int i = 0; i<= sliderQueueLength; ++i)
            {
                var sliderItem = InitSliderItem();
                sliderItem.X = _gap * i;
                sliderItem.SetLabel(_roundData.QuestQueue[i].Cypher.ToString());
                _sliderItems.Add(sliderItem);
            }
            _hiddenSliderItem = InitSliderItem();
            _hiddenSliderItem.Hide();
            _hiddenSliderItem.X = _gap * 3;
            _sliderItems[0].SetSearched();
        }

        private Sequence UpdateSlider()
        {            
            var sliderItem = _hiddenSliderItem;
            _hiddenSliderItem.X = _gap * 3;
            _sliderItems.Add(sliderItem);
            sliderItem.SetLabel(_roundData.QuestQueue[_roundData.QuestIndex + 2].Cypher.ToString());
            var sequence = _sliderItems[1].SetSearched();
            foreach(var item in _sliderItems)
            {
                sequence.Join(item.MoveBy(-_gap));
            }
            _hiddenSliderItem = _sliderItems.Pop();
            sequence.Join(sliderItem.Appear());
            sequence.Join(_hiddenSliderItem.Disappear());
            sequence.Join(_hiddenSliderItem.SetUpcoming());
            return sequence;
        }

        private FlashGlanceSliderItem InitSliderItem()
        {
            var newItem = Instantiate(sliderItemPrefab, slider);
            newItem.SetUpcoming();
            return newItem;
        }

        private FlashGlanceItem InitItem(FlashGlanceRoundItemVO item)
        {
            //var newItem = cachedItems.Count > 0 ? cachedItems.Pop() : Instantiate(itemPrefab, transform);
            FlashGlanceItem newItem;
            if (!_cachedField.TryGetValue(item.GridPosition, out newItem))
            {
                newItem = Instantiate(itemPrefab, transform);
                newItem.X = (item.GridPosition.X - _fieldWidth/2f +0.5f) * _gap;
                newItem.Y = (item.GridPosition.Y - _fieldHeight/2f +0.5f) * _gap;
                newItem.Size = _itemSize;
                _cachedField.Add(item.GridPosition, newItem);
                newItem.Button.onClick.AddListener(() => OnItemSelected(_cachedItems[item.GridPosition]));
            }
            _cachedItems[item.GridPosition] = item;
            newItem.Show();
            newItem.gameObject.SetActive(true);
            newItem.SetLabel(item.Cypher.ToString());
            newItem.Scale = item.Scale;
            
            if(newItem.Rotation != item.Rotation)
                newItem.Rotation = item.Rotation;
            return newItem;
        }

        

        // This is called every round after the initial one, update the elements here
        public void CreateRound(IExerciseRoundDataVO dataVo)
        {
            FlashGlanceRoundDataVO lastRoundData = _roundData;
            var sequence = DOTween.Sequence();
            _roundData = dataVo as FlashGlanceRoundDataVO;
            System.Diagnostics.Debug.WriteLine("CreateRound");

            foreach (FlashGlanceRoundItemVO item in _roundData.Items)
            {
                var newItem = InitItem(item);
            }
            if(lastRoundData.QuestIndex != _roundData.QuestIndex)
                sequence = UpdateSlider();
            sequence.AppendCallback(() => RoundCreated.Dispatch());
            
            //TestAnswer();
        }

        // Sets correct answer for letting the exercise run round by round
        private async void TestAnswer()
        {
            System.Diagnostics.Debug.WriteLine("TestAnswer current solution: " 
                                               + ((FlashGlanceRoundItemVO)_roundData.Solutions[0]).Cypher 
                                               + " QuestQueue length: " + _roundData.QuestQueue.Count
                                               + " current index: " + _roundData.QuestIndex);
            
            await Task.Delay(1000);
            OnItemSelected(_roundData.Solutions[0]);
            
        }

       // Usually here some indication is shown that the answer was correct, for now we just need to hide the selected item in here
       // and dispatch both necessary signals. Preferably dispatch them when animations are finished  
        public void ShowCorrect(IRoundEvaluationResultVO dataVo)
        {
            System.Diagnostics.Debug.WriteLine("ShowCorrect");
            
            var seq = DOTween.Sequence();
            seq.Join(_cachedField[((FlashGlanceRoundItemVO)_roundData.Solutions[0]).GridPosition].Disappear());
            seq.AppendCallback(() => ItemHidden.Dispatch(dataVo.Solutions[0]));
            seq.AppendCallback(() => FeedbackShown.Dispatch());            
        }

        // Usually here some indication is shown that the answer was wrong, for now we do nothing but finish the round
        public void ShowWrong(IRoundEvaluationResultVO dataVo)
        {
            System.Diagnostics.Debug.WriteLine("ShowWrong");
            FeedbackShown.Dispatch();
        }

        // Suggested callback for the selected item, it needs to dispatch the item's IRoundItem data
        // Replace _roundData.Solutions[0] with the actually selected item's data
        private void OnItemSelected(IRoundItem selected)
        {
            //ItemSelected.Dispatch(_roundData.Solutions[0]);
            ItemSelected.Dispatch(selected);
        }
    }
}