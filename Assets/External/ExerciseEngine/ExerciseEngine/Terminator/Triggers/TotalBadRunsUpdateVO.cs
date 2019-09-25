using System;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Terminator.Triggers.Interfaces;

namespace ExerciseEngine.Terminator.Triggers
{
    public class TotalBadRunsUpdateVO : ModelPropertyUpdateVO , ITerminatorTrigger
    {
        public TotalBadRunsUpdateVO(int currentValue, int valueChange) : base(currentValue, valueChange)
        {

        }
    }
}
