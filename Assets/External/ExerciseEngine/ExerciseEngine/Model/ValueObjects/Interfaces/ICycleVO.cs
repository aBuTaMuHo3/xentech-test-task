using System.Collections.Generic;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface ICycleVO
    {
        int CycleId { get; }
        long CycleStartedAt { get; }
        long CycleFinishedAt { get; set; }
        int Posted { get; }
        CycleState State { get; }
        IList<ICycleSessionVO> Sessions { get; }
        ISessionWorkoutVO CurrentWorkout { get; }
        ISessionWorkoutVO LastFinishedWorkout { get; }
        ICycleSessionVO CurrentSession { get; }

        void UpdateSessions(IList<ICycleSessionVO> sessions);
    }

    public enum CycleState
    {
        IN_ASSESSMENT,
        FINISHED_ASSESSMENTS,
        START_NEW_SESSION,
        IN_SESSION,
        FINISHED_DAILY_SESSIONS,
        FINISHED_CYCLE,
        UNKNOWN
    }
}
