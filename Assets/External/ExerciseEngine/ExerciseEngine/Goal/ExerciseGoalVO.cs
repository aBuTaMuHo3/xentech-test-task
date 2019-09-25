using Newtonsoft.Json;

namespace ExerciseEngine.Goal
{
    public class ExerciseGoalVO : IExerciseGoal
    {
        public GoalType Type { get; }
        public int Value { get; set; }

        public ExerciseGoalVO(GoalType type, int value) : this(type)
        {
            Value = value;
        }

        public ExerciseGoalVO(GoalType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}