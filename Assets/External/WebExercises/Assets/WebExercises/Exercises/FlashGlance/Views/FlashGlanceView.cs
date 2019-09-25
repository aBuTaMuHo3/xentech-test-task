using System.Threading.Tasks;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using FlashGlance.Model.ValueObjects;
using MinLibs.MVC;
using MinLibs.Signals;

namespace WebExercises.FlashGlance
{
    // Scaffold of an possible solution for implementing the exercise view
    // Ideally there should be one prefab that works as the presenting component on top of the view and another prefab
    // for the single items (depends how you do it you can also use one for the grid layout).
    // On the long run the "slider component" on top should be reusable in different exercises
    public class FlashGlanceView: MediatedBehaviour, IFlashGlanceView
    {
        public Signal<IRoundItem> ItemSelected { get; } = new Signal<IRoundItem>();
        
        // Needs to be dispatched after each created round to switch to input state and process given answers
        // If there is some show animation it should be dispatched after they are completed
        public Signal RoundCreated { get; } = new Signal();
        
        // Needs to be dispatched after feedback was shown by the view to finish the current round and start a new one
        public Signal FeedbackShown { get; } = new Signal();
        
        // Exercise specific feature, needs to be dispatched after an item was removed to update the available item positions in the model
        public Signal<IRoundItem> ItemHidden { get; } = new Signal<IRoundItem>();

        private FlashGlanceRoundDataVO _roundData;
       
        // Called on the very first round, for now here the initial elements can be initialized
        public void CreateInitialRound(IExerciseRoundDataVO dataVo)
        {
            _roundData = dataVo as FlashGlanceRoundDataVO;
            System.Diagnostics.Debug.WriteLine("CreateInitialRound");
            RoundCreated.Dispatch();

            TestAnswer();
        }

       // This is called every round after the initial one, update the elements here
        public void CreateRound(IExerciseRoundDataVO dataVo)
        {
            _roundData = dataVo as FlashGlanceRoundDataVO;
            System.Diagnostics.Debug.WriteLine("CreateRound");
            RoundCreated.Dispatch();
            
            TestAnswer();
        }

        // Sets correct answer for letting the exercise run round by round
        private async void TestAnswer()
        {
            System.Diagnostics.Debug.WriteLine("TestAnswer current solution: " 
                                               + ((FlashGlanceRoundItemVO)_roundData.Solutions[0]).Cypher 
                                               + " QuestQueue length: " + _roundData.QuestQueue.Count
                                               + " current index: " + _roundData.QuestIndex);
            
            await Task.Delay(1000);
            OnItemSelected();
        }

       // Usually here some indication is shown that the answer was correct, for now we just need to hide the selected item in here
       // and dispatch both necessary signals. Preferably dispatch them when animations are finished  
        public void ShowCorrect(IRoundEvaluationResultVO dataVo)
        {
            System.Diagnostics.Debug.WriteLine("ShowCorrect");
            ItemHidden.Dispatch(dataVo.Solutions[0]);
            FeedbackShown.Dispatch();
        }

        // Usually here some indication is shown that the answer was wrong, for now we do nothing but finish the round
        public void ShowWrong(IRoundEvaluationResultVO dataVo)
        {
            System.Diagnostics.Debug.WriteLine("ShowWrong");
            FeedbackShown.Dispatch();
        }

        // Suggested callback for the selected item, it needs to dispatch the item's IRoundItem data
        // Replace _roundData.Solutions[0] with the actually selected item's data
        private void OnItemSelected()
        {
            ItemSelected.Dispatch(_roundData.Solutions[0]);
        }
    }
}