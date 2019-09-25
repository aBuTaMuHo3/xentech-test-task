using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    /*
     * Generic VO to notify an answer 
     */
    public class BaseAnswerVO : IExerciseViewUpdateVO, IAnswerVO
    {

        /* item user selected */
        protected IRoundItem _solution;
        public IRoundItem Solution
        {
            get { return _solution; }
        }

        public BaseAnswerVO(IRoundItem solution)
        {
            _solution = solution;
        }
    }
}