namespace ExerciseEngine.View.Interfaces
{
    public delegate void MessageConfirmHandler();

    public interface IMessageConfirmBox<T> : IMessageBox<T>
    {
        event MessageConfirmHandler OnConfirm;
        void ShowConfirmMessage(string message, float animationDuration = 0f, float paddingBottom = 0);
        void ShowConfirmMessage(string message, T picture, float animationDuration = 0f, float paddingBottom = 0);

        void ShowMemorizeMessage(string message, string buttonText);
    }
}
