using System;
using MinLibs.MVC;
using SynaptikonFramework.Interfaces.Util.Timer;
using WebExercises.Exercise;
using WebExercises.Shared;

namespace WebExercises.HUD
{
	public interface IHUDTime : IHUDItem
	{
		void SetTime(string time);
	}

	public class HUDTimeMediator : HUDItemMediator<IHUDTime>
	{
		[Inject] public IExerciseState exerciseState;
		
		protected override void Register()
		{
			base.Register();
			
			signals.Register(controller.OnUpdateTime, OnUpdateTime);
			signals.Register(controller.OnStartExercise, OnStart);
			
			InitView(TextKeys.HUD_TIME_HEADER);
		}

		private void OnStart()
		{
			UpdateTime(exerciseState.Timeout);
		}

		private void OnUpdateTime(ITimerUpdateVO vo)
		{
			var timeLeft = exerciseState.Timeout - (float)vo.TimePassed;
			UpdateTime(timeLeft);
		}

		private void UpdateTime(float timeLeft)
		{
			timeLeft = Math.Max(0, timeLeft);
			var timespan = TimeSpan.FromMilliseconds(timeLeft);
			mediated.SetTime(timespan.ToString(@"mm\:ss"));
		}
	}
}