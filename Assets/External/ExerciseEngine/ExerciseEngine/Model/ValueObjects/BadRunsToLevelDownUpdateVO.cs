using System;
namespace ExerciseEngine.Model.ValueObjects
{
    public class BadRunsToLevelDownUpdateVO : ModelPropertyUpdateVO
    {
        public BadRunsToLevelDownUpdateVO(int currentValue, int valueChange) : base(currentValue, valueChange)
        {
        }
    }
}