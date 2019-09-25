using System;

namespace ExerciseEngine.Model.Tutorial.Interfaces
{
    public interface ITutorialConfiguration
    {
        /// <summary>
        /// class must implement ITutorialTrigger
        /// </summary>
        /// <returns>The class.</returns>
        Type TriggerType { get; }

        /// <summary>
        /// class must implement ITutorialStateHandler
        /// </summary>
        /// <value>The handler class.</value>
        Type HandlerType { get; }

        int Priority { get; }

        int Repeats { get; set; }
    }
}