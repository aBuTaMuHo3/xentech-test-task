using System;
namespace ExerciseEngine.Goal
{
    public enum GoalType
    {
        No,
        Score,
        GoodRuns,
        Rounds
    }

    public interface IExerciseGoal
    {
        GoalType Type { get; }
        int Value { get; set; }
    }
}
