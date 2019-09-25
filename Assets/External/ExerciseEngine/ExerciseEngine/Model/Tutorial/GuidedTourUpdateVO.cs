using System;
using ExerciseEngine.Controller.ValueObjects.Interfaces;
using ExerciseEngine.Model.Tutorial.Interfaces;

namespace ExerciseEngine.Model.Tutorial
{
    public class GuidedTourUpdateVO : IExerciseControllerUpdateVO, ITutorialManagerUpdateVO
    {
        public bool TutStarted { get; }
        public string TutType { get; }

        public GuidedTourUpdateVO(bool tutStarted, string tutType)
        {
            TutStarted = tutStarted;
            TutType = tutType;
        }
    }
}
