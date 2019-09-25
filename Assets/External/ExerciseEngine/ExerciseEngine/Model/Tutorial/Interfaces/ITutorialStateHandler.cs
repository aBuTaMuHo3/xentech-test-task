using System;
using ExerciseEngine.Controller;
using ExerciseEngine.Model.Interfaces;
using ExerciseEngine.View.Interfaces;

namespace ExerciseEngine.Model.Tutorial.Interfaces
{
    public interface ITutorialStateHandler : IDisposable, IExerciseStateHandler
    {
        ITutorialTrigger Trigger { get; set; }

        // function returning number that tutorial loop needs to be increased
        int TutorialLoopIncrease { get; }

        void Init(IExerciseModel model, BaseExerciseController controller,IExerciseView view/*, IMessageBox messageBox*/);

        bool Initialised { get; }

        int Loops { get; }
    }
}
