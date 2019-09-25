using System;
namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface ITrainingReminderVO
    {
        int ReminderId { get; }

        string ReminderTime { get; }
    }
}
