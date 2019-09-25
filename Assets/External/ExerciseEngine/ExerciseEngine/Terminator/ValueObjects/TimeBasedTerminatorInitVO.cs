using System;
using ExerciseEngine.Terminator.ValueObjects.Interfaces;

namespace ExerciseEngine.Terminator.ValueObjects
{
    public class TimeBasedTerminatorInitVO : ITerminatorInitVO
    {
        public int ExerciseDuration { get; }

        public TimeBasedTerminatorInitVO(int exerciseDuration)
        {
            ExerciseDuration = exerciseDuration;
        }
    }
}
