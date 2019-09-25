using ExerciseEngine.Model.ValueObjects.Interfaces;
using Memoflow.Model;
using Memoflow.Model.ValueObjects;
using MinLibs.MVC;
using MinLibs.Signals;
using WebExercises.Exercise;
using WebExercises.Shared;

namespace WebExercises.Memoflow
{
	public interface IMemoflowItemPanel : IMediatedBehaviour
	{
		Signal OnItemsMoved { get; }
		void AddItem(int symbolId);
		void AppendItem(int symbolId, int countDelta);
		void HideItems();
		void ShowIndicator(bool isCorrect);
		void ShowArrow();
	}
	
	public class MemoflowItemPanelMediator : Mediator<IMemoflowItemPanel>
	{
		[Inject] public IExerciseControllerProxy controller;
		[Inject] public IExerciseSettings settings;
		[Inject] public OnFinishInitialRound onFinishInitialRound;

		private int _numCards;
		
		protected override void Register()
		{
			base.Register();
			
			signals.Register(controller.OnCreateInitialRound, CreateInitialRound);
			signals.Register(controller.OnCreateRound, CreateRound);
			signals.Register(controller.OnShowCorrect, ShowCorrect);
			signals.Register(controller.OnShowWrong, ShowWrong);
			
			signals.Register(onFinishInitialRound, FinishInitialRound);
			
			signals.Register(mediated.OnItemsMoved, controller.RoundCreated);
		}

		private void FinishInitialRound()
		{
			mediated.HideItems();
			mediated.ShowArrow();
		}

		private void CreateInitialRound(IExerciseRoundDataVO dataVO)
		{
			var roundData = dataVO as MemoflowRoundDataVO;
			_numCards = roundData.NumCards;
			var items = roundData.Items;

			foreach (MemoflowStepVO item in items)
			{
				mediated.AddItem(item.SymbolId);
			}
		}

		private void CreateRound(IExerciseRoundDataVO dataVO)
		{
			var roundData = dataVO as MemoflowRoundDataVO;
			var countDelta = 0;

			if (_numCards != roundData.NumCards)
			{
				countDelta = roundData.NumCards - _numCards;
				_numCards = roundData.NumCards;
			}
			
			var items = roundData.Items;
			var nextItem = items[items.Count - 1] as MemoflowStepVO;
			mediated.AppendItem(nextItem.SymbolId, countDelta);
		}

		private void ShowCorrect(IRoundEvaluationResultVO dataVO)
		{
			ShowFeedback(true);
		}

		private void ShowWrong(IRoundEvaluationResultVO dataVO)
		{
			ShowFeedback(false);
		}

		private void ShowFeedback(bool isCorrect)
		{
			if (settings.Mode == ExerciseConsts.PRACTICE)
			{
				mediated.ShowIndicator(isCorrect);
			}
			
			controller.FeedbackShown();
		}
	}
}