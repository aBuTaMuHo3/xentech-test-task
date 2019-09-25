using System;
using ExerciseEngine.HUD.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IModelPropertyUpdateVO : IExerciseHudVO
    {
        /// <summary>
        /// Gets the current value of the property.
        /// </summary>
        /// <value>The current value.</value>
        int CurrentValue { get; }

        /// <summary>
        /// Gets the value change of the property.
        /// </summary>
        /// <value>The value change.</value>
        int ValueChange { get; set; }
    }
}
