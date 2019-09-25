using System;
using SynaptikonFramework.Util.Math;

namespace ExerciseEngine.View.Interfaces
{
    public interface IExerciseBackgroundView : IDisposable
    {
        void LevelUp(Action callback = null);

        void LevelDown(Action callback = null);

        void ShowCorrectIndicator();
        void ShowWrongIndicator();
        void ShowTimeoutIndicator();

        void ShowCorrectAnswer(Action callback = null);
        void ShowCorrectAnswer(float duration, Action callback = null);

        void ShowWrongAnswer(Action callback = null);
        void ShowWrongAnswer(float duration, Action callback = null);

        void SetMultiplier(int mulitiplierLevel);

        void Tap(Vector2D point);

        void EnableBackground(bool enabled);

        void ToggleGradient();
    }
}
