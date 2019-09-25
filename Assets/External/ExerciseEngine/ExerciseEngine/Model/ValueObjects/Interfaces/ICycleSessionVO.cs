using System;
using System.Collections.Generic;
using ExerciseEngine.Model.Enum;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface ICycleSessionVO
    {
        int CycleId { get; }
        int SessionId { get; }
        SessionStatusEnum SessionStatus { get; set; }
        long FinishedTimestamp { get; set; }
        long SyncedAt { get; set; }
        ISessionWorkoutVO[] Workouts { get; set; }
        int FocusDomainId { get; }
    }
}
