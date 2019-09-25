using System;
namespace ExerciseEngine.HUD.Interfaces
{
    public interface ITimeBasedHudView : IHudView
    {
        long TotalTime { set; }

        [Obsolete("StartExerciseTimeProgress is deprecated, please use UpdateExerciseTimeVO & TotalTime instead.")]
        void StartExerciseTimeProgress(long totalTime);

        void UpdateExerciseTimeVO(double timePassed);
    }
}
