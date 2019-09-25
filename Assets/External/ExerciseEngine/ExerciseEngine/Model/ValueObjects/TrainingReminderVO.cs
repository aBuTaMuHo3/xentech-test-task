using System;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    public class TrainingReminderVO : ITrainingReminderVO
    {
        public TrainingReminderVO(int reminderId, string reminderTime)
        {
            this.ReminderId = reminderId;
            this.ReminderTime = reminderTime;
        }

        public int ReminderId { get; }

        public string ReminderTime { get; }
    }
}
