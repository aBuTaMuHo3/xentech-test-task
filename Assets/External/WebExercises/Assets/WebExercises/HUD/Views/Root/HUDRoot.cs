using MinLibs.MVC;
using MinLibs.Signals;
using WebExercises.Exercise;
using WebExercises.Shared;

namespace WebExercises.HUD
{
	public interface IHUDRoot : IMediatedBehaviour
	{
		Signal OnPauseApp { get; }
	}

	public class HUDRootMediator : Mediator<IHUDRoot>
	{
		[Inject] public IBlockApp blockApp;
		[Inject] public IExerciseControllerProxy controller;

		protected override void Register()
		{
			base.Register();

			signals.Register(controller.OnInitLevelHUD, OnInitLevelHUD);
			
			signals.Register(mediated.OnPauseApp, OnPauseApp);
		}

		private void OnInitLevelHUD(HUDLevelInfo info)
		{
			
		}

		private void OnPauseApp()
		{
			blockApp.Execute(true, TextKeys.CONTINUE);
		}
	}
}
