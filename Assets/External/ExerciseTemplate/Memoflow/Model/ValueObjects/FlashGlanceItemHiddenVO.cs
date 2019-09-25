using System;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;
using FlashGlance.Model.ValueObjects;

namespace FlashGlance.Model.ValueObjects
{
    public class FlashGlanceItemHiddenVO : IExerciseViewUpdateVO, IRoundIndependentVO
    {
        public FlashGlanceItemHiddenVO(FlashGlanceRoundItemVO item)
        {
            Item = item;
        }

        public FlashGlanceRoundItemVO Item { get; }
    }
}