using System;
using System.Collections.Generic;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    public class RoundIndependentUpdateResultVO:ExerciseModelUpdateVO, IRoundIndependentUpdateResultVO
    {
        public RoundIndependentUpdateResultVO(List<IModelPropertyUpdateVO> updates) : base(updates)
        {

        }
    }
}
