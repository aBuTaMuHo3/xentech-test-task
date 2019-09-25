using System;
namespace ExerciseEngine.Model.ValueObjects
{
    public class GoodRunsToLevelUpUpdateVO : ModelPropertyUpdateVO
    {
        public GoodRunsToLevelUpUpdateVO(int currentValue, int valueChange) : base(currentValue, valueChange)
        {
        }
    }
}
