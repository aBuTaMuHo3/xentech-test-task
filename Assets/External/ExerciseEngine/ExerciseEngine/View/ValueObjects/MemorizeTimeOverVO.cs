using System.Collections.Generic;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.View.ValueObjects.Interfaces
{
    public class MemorizeTimeOverVO: IRoundIndependentUpdateResultVO {
        public List<IModelPropertyUpdateVO> Updates => new List<IModelPropertyUpdateVO>() { };
    }
}
