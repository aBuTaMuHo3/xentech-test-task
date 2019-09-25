namespace ExerciseEngine.Model.ValueObjects
{
    public class MaxDifficultyUpdateVO : ModelPropertyUpdateVO
    {
        public MaxDifficultyUpdateVO(int currentValue, int valueChange) : base(currentValue, valueChange)
        {
        }
    }
}