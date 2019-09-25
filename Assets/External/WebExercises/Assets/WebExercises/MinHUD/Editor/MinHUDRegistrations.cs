using MinLibs.MVC;
using WebExercises.Shared;
using WebExercises.Shared.Components;
using WebExercises.Shared.Editor;

namespace WebExercises.MinHUD
{
	public class MinHUDRegistrations : WebExercisesRegistrations
	{
		public override RegistrationInfo Create()
		{
			var info = base.Create();
			info.FilePath = "Modules/MinHUD/Init";

			info.Add<SetupMinHUDContext>().As<ISetupContext>();
			
			info.Add<FullscreenToggleMediator>().With(RegisterFlags.NoCache);

			return info;
		}
	}
}