using MinLibs.Signals;

namespace WebExercises.Runner
{
	public interface IAppState
	{
		bool IsPaused { get; }
		bool Pause(bool isPaused);
		Signal<bool> OnPaused { get; }
	}

	public class AppState : IAppState
	{
		public Signal<bool> OnPaused { get; } = new Signal<bool>();

        public bool IsPaused { get; private set; }

        public bool Pause(bool isPaused)
		{
			var wasPaused = IsPaused;
			IsPaused = isPaused;
			var hasToggled = wasPaused != IsPaused;

			if (hasToggled)
			{
				OnPaused.Dispatch(IsPaused);
			}

			return hasToggled;
		}
	}
}