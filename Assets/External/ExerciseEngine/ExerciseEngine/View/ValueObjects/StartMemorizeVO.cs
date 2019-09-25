namespace ExerciseEngine.View.ValueObjects.Interfaces
{
    public class StartMemorizeVO : IExerciseViewUpdateVO
    {
        public int Timeout { get; }

        public StartMemorizeVO(int timeout)
        {
            Timeout = timeout;
        }
    }
}
