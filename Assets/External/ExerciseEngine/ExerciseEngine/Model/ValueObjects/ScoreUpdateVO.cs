namespace ExerciseEngine.Model.ValueObjects
{
    public class ScoreUpdateVO : ModelPropertyUpdateVO
    {
        public ScoreUpdateVO(int currentValue, int valueChange) : base(currentValue, valueChange)
        {
        }
    }
}