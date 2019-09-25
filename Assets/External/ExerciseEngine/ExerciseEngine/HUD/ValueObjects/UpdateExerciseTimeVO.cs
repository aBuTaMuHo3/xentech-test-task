using System;
using ExerciseEngine.HUD.ValueObjects.Interfaces;
using SynaptikonFramework.Interfaces.Util.Timer;

namespace ExerciseEngine.HUD.ValueObjects
{
    public class UpdateExerciseTimeVO : IExerciseHudVO
    {
        public ITimerUpdateVO Update { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExerciseEngine.HUD.ValueObjects.UpdateExerciseTimeVO"/> class.
        /// </summary>
        /// <param name="update">Update.</param>
        public UpdateExerciseTimeVO(ITimerUpdateVO update)
        {
            Update = update;
        }
    }
}
