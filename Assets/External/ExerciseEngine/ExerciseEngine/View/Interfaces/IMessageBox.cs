using System;

namespace ExerciseEngine.View.Interfaces
{

    public delegate void MessageShownHandler();
    public delegate void MessageHideHandler();
    public interface IMessageBox<T> : IDisposable
    {
        event MessageShownHandler OnShown;
        event MessageHideHandler OnHidden;
        void ShowMessage(float animationDuration = 0f);
        void ShowMessage(string message, float animationDuration = 0f, float paddingBottom = 0);
        void ShowMessage(string message, T picture,  float animationDuration = 0f, float paddingBottom = 0);
        void HideMessage(float animationDuration = 0f);
    }
}
