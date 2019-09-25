using System;
using System.Collections.Generic;
using ExerciseEngine.HUD.Enum;
using ExerciseEngine.HUD.ValueObjects;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.View.ValueObjects;
using ExerciseEngine.View.ValueObjects.Interfaces;

namespace ExerciseEngine.HUD.Interfaces
{
    public interface IHudView : IDisposable
    {
        event HudUpdateHandler OnUpdate;
        /// <summary>
        /// Sets the paddings. On iOS it's the safe area.
        /// </summary>
        /// <value>The paddings.</value>
        IExercisePaddings Paddings { get; set; }
        /// <summary>
        /// Gets the insets. This is used to increse the safe area. I game coordinates.
        /// </summary>
        /// <value>The insets.</value>
        IExercisePaddings Insets { get; }

        void PauseAnimations();
        void ResumeAnimations();

        void SetTimeoutBarSize(float startSize, float endSize, double duration = double.Epsilon);

        int GoodrunsToLevelUp { set; }
        int BadrunsToLevelDown { set; }

        int Goodruns { set; }
        int Badruns { set; }

        int CurrentLevel { set; }
        int MaxLevel { set; }

        int Goal { set; }

        float TimeoutBarSize { get; set; }

        int WarmupRounds { set; }
        int TotalWarmupRounds { set; }
        TimeoutType TimeoutBarType { get; set; }

        void WarmupDone();

        void ShowScoreAnimation(ShowScoreAnimationVO data);
    }
}
