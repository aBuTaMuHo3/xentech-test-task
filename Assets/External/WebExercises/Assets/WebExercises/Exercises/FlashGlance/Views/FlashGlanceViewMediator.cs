using ExerciseEngine.Model.ValueObjects.Interfaces;
using FlashGlance.Model.ValueObjects;
using MinLibs.MVC;
using WebExercises.Shared;

namespace WebExercises.FlashGlance
{
    public class FlashGlanceViewMediator: Mediator<IFlashGlanceView>
    {
        [Inject] public IExerciseControllerProxy controller;
        
        protected override void Register()
        {
            base.Register();
			
            signals.Register(controller.OnCreateInitialRound, mediated.CreateInitialRound);
            signals.Register(controller.OnCreateRound, mediated.CreateRound);
            signals.Register(controller.OnShowCorrect, mediated.ShowCorrect);
            signals.Register(controller.OnShowWrong, mediated.ShowWrong);
            
            signals.Register(mediated.ItemSelected, OnItemSelected);
            signals.Register(mediated.RoundCreated, OnRoundCreated);
            signals.Register(mediated.FeedbackShown, OnFeedbackShown);
            signals.Register(mediated.ItemHidden, OnItemHidden);
        }

        private void OnItemHidden(IRoundItem obj)
        {
            controller.Controller.OnViewUpdate(new FlashGlanceItemHiddenVO(obj as FlashGlanceRoundItemVO));
        }

        private void OnFeedbackShown()
        {
            controller.FeedbackShown();
        }

        private void OnItemSelected(IRoundItem item)
        {
            controller.CheckAnswer(item);
        }

        private void OnRoundCreated()
        {
            controller.RoundCreated();
        }
    }
}