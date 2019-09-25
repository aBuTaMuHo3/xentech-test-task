using WebExercises.Shared;
using WebExercises.Shared.Components;

namespace WebExercises.HUD
{
	public class SetupHUDContext : SetupContext
	{
		protected override void SetupMediators()
		{
			base.SetupMediators();
			
			mediators.Map<IHUDRoot, HUDRootMediator>();
			mediators.Map<IHUDLevel, HUDLevelMediator>();
			mediators.Map<IHUDScore, HUDScoreMediator>();
			mediators.Map<IHUDTime, HUDTimeMediator>();
			mediators.Map<IFullscreenToggle, FullscreenToggleMediator>();
		}
	}
}