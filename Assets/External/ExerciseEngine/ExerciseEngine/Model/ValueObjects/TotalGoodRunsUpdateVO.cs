using System;
using ExerciseEngine.Terminator.Triggers.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    public class TotalGoodRunsUpdateVO : ModelPropertyUpdateVO, ITerminatorTrigger
    {
        public TotalGoodRunsUpdateVO(int currentValue, int valueChange) : base(currentValue, valueChange)
        {
        }
    }
}
