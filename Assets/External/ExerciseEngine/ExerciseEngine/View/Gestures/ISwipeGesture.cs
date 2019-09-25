using SynaptikonFramework.Util.Math;

namespace ExerciseEngine.View.Gestures
{
    public interface ISwipeGesture
    {
        bool CheckDirection(GestureDirection gestureDirectionToCheck);

        Vector2D GesturePoint { get; }
    }
}