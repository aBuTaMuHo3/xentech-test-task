using WebExercises.Shared;
using WebExercises.Shared.Components;

namespace WebExercises.MinHUD
{
	public class SetupMinHUDContext : SetupContext
	{
		protected override void SetupMediators()
		{
			base.SetupMediators();
			
			mediators.Map<IFullscreenToggle, FullscreenToggleMediator>();
		}
	}
}