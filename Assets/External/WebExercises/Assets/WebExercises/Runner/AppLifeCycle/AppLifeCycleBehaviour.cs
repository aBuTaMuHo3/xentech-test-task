using MinLibs.MVC;
using MinLibs.Signals;

namespace WebExercises.Runner
{
	public class AppLifeCycleBehaviour : MediatedBehaviour, IAppLifeCycle
	{
		public Signal<bool> OnPause { get; } = new Signal<bool>();

		private void OnApplicationFocus(bool hasFocus)
		{
			UpdatePause(!hasFocus);
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			UpdatePause(pauseStatus);
		}

		private void UpdatePause(bool isPaused)
		{
			OnPause.Dispatch(isPaused);
		}
	}
}