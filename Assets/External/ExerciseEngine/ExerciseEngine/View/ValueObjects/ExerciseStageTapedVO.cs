using System;
using ExerciseEngine.View.ValueObjects.Interfaces;
using SynaptikonFramework.Util.Math;

namespace ExerciseEngine.View.ValueObjects
{
    public class ExerciseStageTapedVO : IExerciseViewUpdateVO
    {
        readonly public Vector2D Position;

        public ExerciseStageTapedVO(Vector2D position)
        {
            Position = position;
        }
    }
}
