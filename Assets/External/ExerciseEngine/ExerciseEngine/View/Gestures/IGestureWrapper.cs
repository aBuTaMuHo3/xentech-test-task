using SynaptikonFramework.Util.Math;
using System;

namespace ExerciseEngine.View.Gestures
{
    public delegate void GestureRecognised(ISwipeGesture gesture);

    public interface IGestureWrapper : IDisposable
    {
        bool Active { get; set; }
        bool IsSwallowTouches { get; set; }
        Point2D LastTouchPosition { get; } 
        event GestureRecognised OnGetureRecognised;
    }
}
