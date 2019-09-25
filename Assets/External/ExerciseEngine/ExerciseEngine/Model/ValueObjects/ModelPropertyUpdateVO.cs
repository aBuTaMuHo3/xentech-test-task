using System;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    public class ModelPropertyUpdateVO: IModelPropertyUpdateVO
    {
        public int CurrentValue { get; }
        public int ValueChange { get; set; }

        public ModelPropertyUpdateVO(int currentValue, int valueChange)
        {
            ValueChange = valueChange;
            CurrentValue = currentValue;
        }
    }
}
