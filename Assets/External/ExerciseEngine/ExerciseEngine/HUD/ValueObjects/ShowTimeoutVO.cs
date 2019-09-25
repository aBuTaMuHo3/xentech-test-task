using ExerciseEngine.HUD.Enum;
using ExerciseEngine.HUD.ValueObjects.Interfaces;

namespace ExerciseEngine.HUD.ValueObjects
{
    public class ShowTimeoutVO : IExerciseHudVO
    {
        public TimeoutType Type { get; }

        public ShowTimeoutVO(TimeoutType type)
        {
            Type = type;
        }
    }
}
