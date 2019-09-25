using MinLibs.MVC;
using MinLibs.Signals;

namespace WebExercises.Runner
{
	public class FrontendReceiverBehaviour : MediatedBehaviour, IFrontendReceiver
	{
		public Signal<string> OnInitializeApp { get; } = new Signal<string>();
		public Signal<string> OnInitializeExercise { get; } = new Signal<string>();
		public Signal<string> OnStartExercise { get; } = new Signal<string>();
		
		public void InitializeApp(string config)
		{
			OnInitializeApp.Dispatch(config);
		}

		public void InitializeExercise(string config)
		{
			OnInitializeExercise.Dispatch(config);
		}

		public void StartExercise(string config)
		{
			OnStartExercise.Dispatch(config);
		}
	}
}
