using System;
using ExerciseEngine.Terminator.ValueObjects.Interfaces;

namespace ExerciseEngine.Terminator.ValueObjects
{
    public class SurvivorTerminatorInitVO : ITerminatorInitVO
    {
        public int NumBadRuns { get; }

        public SurvivorTerminatorInitVO(int numBadRuns)
        {
            NumBadRuns = numBadRuns;
        }
    }
}
