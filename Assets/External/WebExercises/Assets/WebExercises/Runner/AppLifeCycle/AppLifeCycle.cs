using MinLibs.MVC;
using MinLibs.Signals;
using WebExercises.Shared;

namespace WebExercises.Runner
{
	public interface IAppLifeCycle : IMediated
	{
		Signal<bool> OnPause { get; }
	}
	
	public class AppLifeCycleMediator : Mediator<IAppLifeCycle>, IAppLifeCycleMediator
	{
		[Inject] public IAppState appState;
		[Inject] public OnBlockApp onBlockApp;

		protected override void Register()
		{
			base.Register();
			
			signals.Register(mediated.OnPause, OnPause);
		}

		private void OnPause(bool isPaused)
		{
			if (appState.IsPaused != isPaused)
			{
				onBlockApp.Dispatch(isPaused);
			}
		}
	}

	public interface IAppLifeCycleMediator : IMediator
	{
	}

	public class DummyAppLifeCycleMediator : Mediator<IAppLifeCycle>, IAppLifeCycleMediator
	{
	}
}