using ExerciseEngine.HUD.ValueObjects.Interfaces;

namespace ExerciseEngine.HUD.ValueObjects
{
    public class TimeBasedHudInitVO : IHudInintVO
    {
        public long TotalTime { get; }

        public TimeBasedHudInitVO(long totalTime)
        {
            TotalTime = totalTime;
        }
    }
}
