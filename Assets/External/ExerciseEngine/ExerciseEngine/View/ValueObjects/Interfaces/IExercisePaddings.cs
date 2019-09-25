namespace ExerciseEngine.View.ValueObjects.Interfaces
{
    public interface IExercisePaddings
    {
        float Top { get; }
        float Right { get; }
        float Left { get; }
        float Bottom { get; }
        float TopIncludingExtraSpace { get; }
    }
}
