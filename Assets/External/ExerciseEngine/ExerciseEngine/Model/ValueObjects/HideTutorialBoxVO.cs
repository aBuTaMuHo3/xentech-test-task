using ExerciseEngine.Controller.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    public class HideTutorialBoxVO : IExerciseControllerUpdateVO
    {
        private bool _animated;

        public HideTutorialBoxVO(bool animated = true)
        {
            _animated = animated;
        }

        public bool animated
        {
            get
            {
                return _animated;
            }
        }
    }
}