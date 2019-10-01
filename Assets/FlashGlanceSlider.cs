using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FlashGlance.Model.ValueObjects;
using MinLibs.Utils;

namespace WebExercises.FlashGlance
{
    public class FlashGlanceSlider : MonoBehaviour
    {
        [SerializeField] private FlashGlanceSliderItem sliderItemPrefab;
        [SerializeField] private int sliderQueueLength;
        [SerializeField] private float sliderRotationAngle;

        private float _rotationRadius;
        private IList<FlashGlanceSliderItem> _sliderItems = new List<FlashGlanceSliderItem>();
        private FlashGlanceSliderItem _hiddenSliderItem;
        private FlashGlanceRoundDataVO _roundData;
        private Sequence _updateSequence;

        public float RotationRadius
        {
            get
            {
                _rotationRadius = _rotationRadius == 0f ? ((RectTransform)transform).rect.height / 2 : _rotationRadius;
                return _rotationRadius;
            }
        }

        public FlashGlanceSliderItem InitItem()
        {
            var newItem = Instantiate(sliderItemPrefab, transform);
            newItem.SetUpcoming();
            return newItem;
        }

        public void Init(FlashGlanceRoundDataVO newRoundData)
        {
            _roundData = newRoundData;
            _updateSequence = DOTween.Sequence();
            for (int i = 0; i <= sliderQueueLength; ++i)
            {
                var sliderItem = InitItem();
                sliderItem.X = RotationRadius;
                sliderItem.SetLabel(_roundData.QuestQueue[i].Cypher.ToString());
                _sliderItems.Add(sliderItem);
                sliderItem.SetRotation(sliderRotationAngle * (i - 2));
            }
            _hiddenSliderItem = InitItem();
            _hiddenSliderItem.Hide();
            _hiddenSliderItem.X = RotationRadius;
            _hiddenSliderItem.Y = 0;
            _hiddenSliderItem.SetRotation(sliderRotationAngle);
            _sliderItems[0].SetSearched();
        }

        public Sequence Update(FlashGlanceRoundDataVO newRoundData)
        {
            if (_roundData.QuestIndex == newRoundData.QuestIndex)
                return null;
            if (!_updateSequence.IsComplete())
                _updateSequence.Complete();
            _roundData = newRoundData;
            var sliderItem = _hiddenSliderItem;
            sliderItem.X = RotationRadius;
            sliderItem.Y = 0;
            sliderItem.SetRotation(sliderRotationAngle);
            _sliderItems.Add(sliderItem);
            sliderItem.SetLabel(_roundData.QuestQueue[_roundData.QuestIndex + 2].Cypher.ToString());
            var sequence = _sliderItems[1].SetSearched();
            foreach (var item in _sliderItems)
            {
                sequence.Join(item.RotateBy(-sliderRotationAngle));
            }
            _hiddenSliderItem = _sliderItems.Pop();
            sequence.Join(sliderItem.Appear());
            sequence.Join(_hiddenSliderItem.Disappear());
            sequence.Join(_hiddenSliderItem.SetUpcoming());
            _updateSequence = sequence;
            return sequence;
        }
    }
}
