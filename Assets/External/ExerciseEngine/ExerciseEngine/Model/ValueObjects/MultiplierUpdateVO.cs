namespace ExerciseEngine.Model.ValueObjects
{
    public class MultiplierUpdateVO : ModelPropertyUpdateVO
    {
        public MultiplierUpdateVO(int currentValue, int valueChange) : base(currentValue, valueChange)
        {
        }
    }
}