using ExerciseEngine.Model.ValueObjects.Interfaces;
using MinLibs.MVC;
using MinLibs.Signals;

namespace WebExercises.FlashGlance
{
    public interface IFlashGlanceView: IMediatedBehaviour
    {
        void CreateInitialRound(IExerciseRoundDataVO dataVo);
        void CreateRound(IExerciseRoundDataVO dataVo);
        void ShowCorrect(IRoundEvaluationResultVO dataVo);
        void ShowWrong(IRoundEvaluationResultVO dataVo);
        
        Signal<IRoundItem> ItemSelected { get; }
        Signal RoundCreated { get; }
        Signal FeedbackShown { get; }
        Signal<IRoundItem> ItemHidden { get; }
    }
}