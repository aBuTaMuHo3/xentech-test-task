using System.Collections.Generic;

namespace ExerciseEngine.Model.Tutorial.Interfaces
{
    public interface ITutorialVO
    {
        string Message { get; }
        Dictionary<string, string> MessageVariables { get; }
        bool Confirm { get; }
    }
}
