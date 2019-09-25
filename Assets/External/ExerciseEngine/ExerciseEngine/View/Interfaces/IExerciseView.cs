using System;
using System.Collections.Generic;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.Tutorial.Interfaces;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;

namespace ExerciseEngine.View.Interfaces
{
    public delegate void ViewUpdateHandler(IExerciseViewUpdateVO vo);

    public interface IExerciseView : IDisposable
    {
        event ViewUpdateHandler OnUpdate;

        IExercisePaddings Paddings { set; }
        IExerciseBackgroundView ExerciseBackground { set; }

        void OnGameStart(IInitialViewDataVO initVO);
        void CreateRound(IExerciseRoundDataVO roundDataVO);
        void CreateInitialRound(IExerciseRoundDataVO roundDataVO);
        void CreateRoundLevelUp(IExerciseRoundDataVO roundDataVO);
        void CreateRoundLevelDown(IExerciseRoundDataVO roundDataVO);
        void ShowCorrect(IRoundEvaluationResultVO feedbackData);
        void ShowWrong(IRoundEvaluationResultVO feedbackData);
		void Update(IRoundIndependentUpdateResultVO result);
        void EndExercise();
        void Suspend();
        void Resume();
        void RunOnGameThread(Action action);

        Dictionary<ExerciseSettingsEnum, bool> Settings { set; }

        void ShowTutorial(ITutorialVO tutorial);
        void HideTutorial(Object[] options = null);
    }
}
