using System;
namespace ExerciseEngine.Model.Tutorial.Interfaces
{
    public interface IExerciseStateHandler
    {
        void OnStateShowRound();
        void AfterStateShowRound();

        void OnStateInput();
        void AfterStateInput();

        void OnStateCorrectAnswer();
        void AfterStateCorrectAnswer();

        void OnStateCorrectStep();
        void AfterStateCorrectStep();

        void OnStateWrongAnswer();
        void AfterStateWrongAnswer();

        void OnStateWrongStep();
        void AfterStateWrongStep();

        void OnStateMemorize();
        void AfterStateMemorize();
    }
}
