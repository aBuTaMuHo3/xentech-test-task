using System;
using ExerciseEngine.Controller;
using ExerciseEngine.Terminator.Triggers.Interfaces;

namespace ExerciseEngine.Terminator.Triggers
{
    public class StateTerminationTrigger : ITerminatorTrigger
    {
        public int State { get; }
        public bool AfterState { get; }

        public bool IsRoundFinished =>
            (State == BaseStates.WRONG_ANSWER || State == BaseStates.CORRECT_ANSWER) && AfterState;

        public StateTerminationTrigger(int state, bool afterState)
        {
            AfterState = afterState;
            State = state;
        }
    }
}
