using System.Collections.Generic;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace FlashGlance.Model.ValueObjects
{
    public class FlashGlanceEventResultVO : RoundIndependentUpdateResultVO
    {
        public FlashGlanceEventResultVO(List<FlashGlanceRoundItemVO> items) : base(new List<IModelPropertyUpdateVO>())
        {
            Items = items;
        }

        public List<FlashGlanceRoundItemVO> Items { get; }
    }
}