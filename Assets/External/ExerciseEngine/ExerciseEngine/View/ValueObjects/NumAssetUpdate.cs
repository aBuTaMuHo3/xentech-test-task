using System;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;

namespace ExerciseEngine.View.ValueObjects
{
    public class NumAssetUpdate: IExerciseViewUpdateVO, IRoundIndependentVO
    {
        public int NumSymbols { get; }

        public NumAssetUpdate(int numSymbols)
        {
            NumSymbols = numSymbols;
        }
    }
}
