using System;
using System.Collections.Generic;

namespace ExerciseEngine.View.Interfaces
{
    public delegate void InputHandler<T>(T data);
    public delegate void ShownHandler();
    public delegate void HideHandler();

    public interface IExerciseInput<T> : IDisposable
    {
        event InputHandler<T> OnInput;
        event ShownHandler OnShown;
        event HideHandler OnHidden;
        void ShowInput(float animationDuration = 0.25f);
        void HideInput(float animationDuration = 0.25f);
        void SetInputs(List<IInputDataVO<T>> inputs, uint columns, float buttonHeight = 100);
        void ShowCorrect(T correctinput, float animationDuration = 0.25f);
        bool IsShown { get; }
        float Height { get; }
    }

    public interface IInputDataVO<T>
    {
        T Data { get; }
    }
}