using System;
using ExerciseEngine.HUD.ValueObjects.Interfaces;
using ExerciseEngine.Terminator.Triggers.Interfaces;
using ExerciseEngine.Terminator.ValueObjects.Interfaces;

namespace ExerciseEngine.Terminator.Interfaces
{
    public delegate void TerminatorUpdateHandler(IExerciseTerminatorUpdateVO vo);

    public interface IExerciseTerminator : IDisposable
    {
        /// <summary>
        /// Configuration of the hud depends on the termination type
        /// Terminator gifes the config
        /// </summary>
        /// <value>The hud inint vo.</value>
        IHudInintVO HudInintVO { get; }

        /// <summary>
        /// Handels the termiantion trigger.
        /// </summary>
        /// <param name="trigger">Trigger.</param>
        void HandelTermiantionTrigger(ITerminatorTrigger trigger);

        /// <summary>
        /// Occurs when on terminate.
        /// </summary>
        event TerminatorUpdateHandler OnTerminate;
    }
}
