using System;
using System.Collections.Generic;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.Tutorial.Interfaces;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.View.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;
using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Utils;

namespace WebExercises.Shared
{
	public interface IExerciseViewAdapter : IExerciseView
	{
	}

	public class ExerciseViewAdapter : IExerciseViewAdapter
	{
		[Inject] public IParser parser;
		[Inject] public ILogging logger;
		[Inject] public IExerciseControllerProxy controllerProxy;

		public event ViewUpdateHandler OnUpdate;
		public IExercisePaddings Paddings { get; set; }
		public IExerciseBackgroundView ExerciseBackground { get; set; }

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public virtual void OnGameStart(IInitialViewDataVO initVO)
		{
			Trace(initVO);
			controllerProxy.OnGameStart.Dispatch(initVO);
		}

		public virtual void CreateRound(IExerciseRoundDataVO roundDataVO)
		{
			Trace(roundDataVO);
			
			controllerProxy.OnCreateRound.Dispatch(roundDataVO);
		}

		public virtual void CreateInitialRound(IExerciseRoundDataVO roundDataVO)
		{
			Trace(roundDataVO);

			controllerProxy.OnCreateInitialRound.Dispatch(roundDataVO);
		}

		public virtual void CreateRoundLevelUp(IExerciseRoundDataVO roundDataVO)
		{
			Trace(roundDataVO);
			controllerProxy.OnCreateRound.Dispatch(roundDataVO);
		}

		public virtual void CreateRoundLevelDown(IExerciseRoundDataVO roundDataVO)
		{
			Trace(roundDataVO);
			controllerProxy.OnCreateRound.Dispatch(roundDataVO);
		}

		public virtual void ShowCorrect(IRoundEvaluationResultVO feedbackData)
		{
			Trace(feedbackData);
			controllerProxy.OnShowCorrect.Dispatch(feedbackData);
		}

		public virtual void ShowWrong(IRoundEvaluationResultVO feedbackData)
		{
			Trace(feedbackData);
			controllerProxy.OnShowWrong.Dispatch(feedbackData);
		}

		public virtual void Update(IRoundIndependentUpdateResultVO result)
		{
			Trace(result);

			if (result is MemorizeTimeOverVO)
			{
				controllerProxy.OnMemorizeTimeOver.Dispatch();
			}
			
			
		}

		public virtual void EndExercise()
		{
			Trace("EndExercise");
			
			//controllerProxy.OnEndExercise.Dispatch();
		}

		public virtual void Suspend()
		{
			Trace("Suspend");
		}

		public virtual void Resume()
		{
			Trace("Resume");
		}

		public virtual void RunOnGameThread(Action action)
		{
			//Trace("RunOnGameThread " + action);
			action.Invoke();
		}

		public virtual Dictionary<ExerciseSettingsEnum, bool> Settings { get; set; }

		public virtual void ShowTutorial(ITutorialVO tutorial)
		{
			Trace(tutorial);
		}

		public virtual void HideTutorial(object[] options = null)
		{
			Trace(options);
		}

		protected void NotifyUpdate(IExerciseViewUpdateVO vo)
		{
			OnUpdate?.Invoke(vo);
		}

		private void Trace(object vo)
		{
//			logger.Trace(vo);
//			logger.Trace(parser.Serialize(vo));
		}
	}
}