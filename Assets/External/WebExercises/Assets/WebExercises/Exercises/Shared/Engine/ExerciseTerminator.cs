using ExerciseEngine.HUD.ValueObjects.Interfaces;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Terminator.Interfaces;
using ExerciseEngine.Terminator.Triggers;
using ExerciseEngine.Terminator.Triggers.Interfaces;
using MinLibs.MVC;

namespace WebExercises.Exercise
{
	public class ExerciseTerminator : IExerciseTerminator
	{
		[Inject] public IExerciseState exerciseState;

		public event TerminatorUpdateHandler OnTerminate;

		public IHudInintVO HudInintVO { get; }

		public void HandelTermiantionTrigger(ITerminatorTrigger trigger)
		{
			ProcessTrigger(trigger);
			exerciseState.Evaluate();
		}

		public void Dispose()
		{
			throw new System.NotImplementedException();
		}

		private void ProcessTrigger(ITerminatorTrigger trigger)
		{
			switch (trigger)
			{
				case TotalGoodRunsUpdateVO _:
					exerciseState.CountRun(true);
					break;
				case TotalBadRunsUpdateVO _:
					exerciseState.CountRun(false);
					break;
				case StateTerminationTrigger terminationTrigger:
					if (terminationTrigger.IsRoundFinished && exerciseState.IsFinished)
					{
						OnTerminate?.Invoke(null);
					}
					break;
				case TimeTerminationTrigger timeoutTrigger:
					exerciseState.HandleTimeOut(timeoutTrigger.TimerUpdateVO.TimePassed);
					break;
			}
		}
	}
}