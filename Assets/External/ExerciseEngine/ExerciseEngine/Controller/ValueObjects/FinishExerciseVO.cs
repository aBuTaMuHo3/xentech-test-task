using System;
using ExerciseEngine.Controller.ValueObjects.Interfaces;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using SynaptikonFramework.MVC;

namespace ExerciseEngine.Controller.ValueObjects
{
    public class FinishExerciseVO : IExerciseControllerUpdateVO, IFinishExerciseVO, IViewValueObject
    {
        public IExerciseResultVO ExerciseResultVO { get; }

        public bool Finished { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExerciseEngine.Controller.ValueObjects.FinishExerciseVO"/> class.
        /// Ends the exercise.
        /// </summary>
        /// <param name="exerciseResultVO">Exercise result VO.</param>
        public FinishExerciseVO( IExerciseResultVO exerciseResultVO, bool finished )
        {
            ExerciseResultVO = exerciseResultVO;
            this.Finished = finished;
        }
    }
}
