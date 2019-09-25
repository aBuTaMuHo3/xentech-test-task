using System.Collections.Generic;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.View.ValueObjects.Interfaces
{
    public class MemorizeTimeStartedVO: IRoundIndependentUpdateResultVO {
        public MemorizeTimeStartedVO(string text) {
            Text = text;
        }

        public List<IModelPropertyUpdateVO> Updates => new List<IModelPropertyUpdateVO>() { };

        public string Text { get; }
    }
}
