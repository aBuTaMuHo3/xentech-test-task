using MinLibs.MVC;
using MinLibs.Signals;

namespace WebExercises.Runner
{
	public interface IFrontendReceiver : IMediatedBehaviour
	{
		Signal<string> OnInitializeApp { get; }
		Signal<string> OnInitializeExercise { get; }
		Signal<string> OnStartExercise { get; }
	}
	
	public class FrontendReceiverMediator : Mediator<IFrontendReceiver>
	{
		[Inject] public IInitApp initApp;
		[Inject] public ILoadExercise loadExercise;
		[Inject] public OnStartExercise onStartExercise;
		
		protected override void Register()
		{
			base.Register();
			
			signals.Register(mediated.OnInitializeApp, initApp.Execute);
			signals.Register(mediated.OnInitializeExercise, loadExercise.Execute);
			signals.Register(mediated.OnStartExercise, onStartExercise.Dispatch);
		}
	}
}