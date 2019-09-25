namespace ExerciseEngine.Model.ValueObjects
{
    public class DifficultyUpdateVO : ModelPropertyUpdateVO
    {
        public DifficultyUpdateVO(int currentValue, int valueChange) : base(currentValue, valueChange)
        {
        }
    }
}