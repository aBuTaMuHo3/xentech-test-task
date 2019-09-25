using MinLibs.MVC;
using WebExercises.Shared;
using WebExercises.Shared.Components;
using WebExercises.Shared.Editor;

namespace WebExercises.HUD
{
	public class HUDRegistrations : WebExercisesRegistrations
	{
		public override RegistrationInfo Create()
		{
			var info = base.Create();
			info.FilePath = "Modules/HUD/Init";

			info.Add<SetupHUDContext>().As<ISetupContext>();
			
			info.Add<HUDRootMediator>().With(RegisterFlags.NoCache);
			info.Add<HUDLevelMediator>().With(RegisterFlags.NoCache);
			info.Add<HUDScoreMediator>().With(RegisterFlags.NoCache);
			info.Add<HUDTimeMediator>().With(RegisterFlags.NoCache);
			info.Add<FullscreenToggleMediator>().With(RegisterFlags.NoCache);
			
			return info;
		}
	}
}