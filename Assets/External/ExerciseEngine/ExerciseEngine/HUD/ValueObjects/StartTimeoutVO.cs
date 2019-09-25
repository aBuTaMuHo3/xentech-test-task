using System;
using ExerciseEngine.HUD.Enum;
using ExerciseEngine.HUD.ValueObjects.Interfaces;
namespace ExerciseEngine.HUD.ValueObjects
{
    public class StartTimeoutVO : IExerciseHudVO
    {
        public int Timeout { get; }
        public TimeoutType Type { get; }

        /// <summary>
        /// Starts the timeout.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="type">Type.</param>
        public StartTimeoutVO(int timeout, TimeoutType type)
        {
            Type = type;
            Timeout = timeout;
        }
    }
}
