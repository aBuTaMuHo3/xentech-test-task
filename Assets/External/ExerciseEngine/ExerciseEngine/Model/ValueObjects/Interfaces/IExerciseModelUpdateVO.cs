using System.Collections.Generic;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IExerciseModelUpdateVO
    {
        /// <summary>
        /// Gets the list of Model properties updates.
        /// </summary>
        /// <value>The model properties updates.</value>
        List<IModelPropertyUpdateVO> Updates { get; }
    }
}
