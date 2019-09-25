using System;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace FlashGlance.Model.ValueObjects
{
    public class FlashGlanceEventRequestVO : IRoundIndependentVO
    {
        public FlashGlanceEventRequestVO(FlashGlanceEventType eventType)
        {
            EventType = eventType;
        }

        public FlashGlanceEventType EventType { get; }
    }
}