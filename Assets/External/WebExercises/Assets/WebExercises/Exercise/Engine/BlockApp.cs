using System;
using MinLibs.MVC;
using WebExercises.HUD;
using WebExercises.Runner;
using WebExercises.Shared;

namespace WebExercises.Exercise
{
	public interface IBlockApp
	{
		void Execute(bool isPaused);
		void Execute(bool isPaused, string buttonKey);
	}

	public class BlockApp : IBlockApp
	{
		[Inject] public IExerciseControllerProxy controller;
		[Inject] public IAppState appState;
		[Inject] public ILocalization localization;
		[Inject] public OnShowDialogue onShowDialogue;
		[Inject] public OnHideDialogue onHideDialogue;

		public void Execute(bool isPaused)
		{
			Execute(isPaused, TextKeys.CONTINUE, ok => {});
		}

		public void Execute(bool isPaused, string buttonKey)
		{
			Execute(isPaused, buttonKey, ok => Execute(false));
		}

		private void Execute(bool isPaused, string buttonKey, Action<bool> handler)
		{
			var hasAppToggled = appState.Pause(isPaused);
			
			controller.IsPaused = isPaused;

			if (!hasAppToggled)
			{
				return;
			}

			if (isPaused)
			{
				var data = new DialogueData
				{
					Message = localization.Get(TextKeys.PAUSE_TEXT),
					BackgroundAlpha = 1f
				};

				if (!string.IsNullOrEmpty(buttonKey))
				{
					var buttonText = localization.Get(buttonKey);
					data.AddAction(buttonText, handler);
				}
				
				onShowDialogue.Dispatch(data);
			}
			else
			{
				onHideDialogue.Dispatch();
			}
		}
	}
}