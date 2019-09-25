using System;
using System.Collections.Generic;
using ExerciseEngine.HUD.ValueObjects.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;

namespace ExerciseEngine.HUD.Interfaces
{
    public delegate void HudUpdateHandler(IExerciseHUDUpdateVO updateVO);

    public interface IExerciseHUD : IDisposable
    {
        event HudUpdateHandler OnUpdate;
        void Update(List<IExerciseHudVO> data);
        /// <summary>
        /// Sets the paddings. On iOS it's the safe area.
        /// </summary>
        /// <value>The paddings.</value>
        IExercisePaddings Paddings { set; }
        /// <summary>
        /// Gets the insets. This is used to increse the safe area. I game coordinates.
        /// </summary>
        /// <value>The insets.</value>
        IExercisePaddings Insets { get; }

        IHudView View { get; }
    }
}