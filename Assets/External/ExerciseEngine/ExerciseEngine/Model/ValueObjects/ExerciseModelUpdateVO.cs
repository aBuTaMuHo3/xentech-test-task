using System;
using System.Collections.Generic;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    public class ExerciseModelUpdateVO:IExerciseModelUpdateVO
    {
        public List<IModelPropertyUpdateVO> Updates { get; }

        public ExerciseModelUpdateVO(List<IModelPropertyUpdateVO> updates)
        {
            Updates = updates;
        }
    }
}
