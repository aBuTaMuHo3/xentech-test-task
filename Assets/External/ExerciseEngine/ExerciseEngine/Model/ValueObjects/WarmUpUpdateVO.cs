using System;
namespace ExerciseEngine.Model.ValueObjects
{
    public class WarmUpUpdateVO : ModelPropertyUpdateVO
    {
        public int OriginalValue { get; }

        public WarmUpUpdateVO(int currentValue, int valueChange, int originalValue) : base(currentValue, valueChange)
        {
            OriginalValue = originalValue;
        }
    }
}
