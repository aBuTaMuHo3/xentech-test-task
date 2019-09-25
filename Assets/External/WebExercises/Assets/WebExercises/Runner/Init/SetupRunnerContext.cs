using System.Threading.Tasks;
using MinLibs.MVC;
using MinLibs.Utils;
using WebExercises.Shared;

namespace WebExercises.Runner
{
	public class SetupRunnerContext : SetupContext
	{
		[Inject] public IFrontendSender sender;
		[Inject] public OnExerciseFinished onExerciseFinished;
		[Inject] public IRemoveExercise removeExercise;
		[Inject] public IScreen screen;
		[Inject] public IHotKeys hotKeys;
		
		public override void Execute(IMediating view)
		{
			base.Execute(view);
			
			sender.OnAppReady();
		}

		protected override void SetupSignals()
		{
			base.SetupSignals();
			
			signals.Register(onExerciseFinished, removeExercise.Execute);
		}

		protected override void SetupMediators()
		{
			base.SetupMediators();
			
			mediators.Map<IAppLifeCycle, IAppLifeCycleMediator>();			
			mediators.Map<IFrontendReceiver, FrontendReceiverMediator>();			
		}

		protected override void SetupMisc()
		{
			base.SetupMisc();

			CheckScreen();
			CheckHotKeys();
		}

		private async Task CheckHotKeys()
		{
			var nextFrame = Awaiters.NextFrame;

			while (true)
			{
				await nextFrame;
				hotKeys.OnCheck();
			}
		}

		private async Task CheckScreen()
		{
			var timeout = Awaiters.Seconds(1f);
			
			while (true)
			{
				await timeout;
				screen.Update();
			}
		}
	}
}