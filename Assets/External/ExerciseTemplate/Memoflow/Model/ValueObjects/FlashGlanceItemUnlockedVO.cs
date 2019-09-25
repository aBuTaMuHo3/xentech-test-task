using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;
using FlashGlance.Model.ValueObjects;

namespace FlashGlance.Model.ValueObjects
{
    public class FlashGlanceItemUnlockedVO : IExerciseViewUpdateVO, IRoundIndependentVO
    {
        public FlashGlanceItemUnlockedVO(FlashGlanceRoundItemVO item)
        {
            Item = item;
        }

        public FlashGlanceRoundItemVO Item { get; }
    }
}