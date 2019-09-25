using MinLibs.MVC;
using WebExercises.Shared;
using WebExercises.Shared.Editor;

namespace WebExercises.HUD
{
	public class DialogueRegistrations : WebExercisesRegistrations
	{
		public override RegistrationInfo Create()
		{
			var info = base.Create();
			info.FilePath = "Modules/Dialogue/Init";

			info.Add<SetupDialogueContext>().As<ISetupContext>();
			
			info.Add<DialogueMediator>().With(RegisterFlags.NoCache);

			return info;
		}
	}
}