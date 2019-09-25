namespace ExerciseEngine.View.Interfaces
{
    public delegate void AutoInputChangeHandler(bool isAuto);
    public interface IExerciseManualAutoInput<T> : IExerciseInput<T>
    {
        event AutoInputChangeHandler OnAutoChange;
        bool IsAutoInput { get; set; }
    }
}