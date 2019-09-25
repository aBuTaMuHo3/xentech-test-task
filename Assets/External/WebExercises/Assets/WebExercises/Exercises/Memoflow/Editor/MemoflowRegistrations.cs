using ExerciseEngine.Colors;
using MinLibs.MVC;
using WebExercises.Exercise;
using WebExercises.Exercise.Editor;
using WebExercises.Shared;

namespace WebExercises.Memoflow.Editor
{
	public class MemoflowRegistrations : ExerciseRegistrations
	{
		public override RegistrationInfo Create()
		{
			var info = base.Create();
			info.FilePath = "Modules/Exercises/Memoflow/Init";

			info.Add<SetupMemoflowContext>().As<ISetupContext>();
			info.Add<StartMemoflow>().As<IStartExercise>();
			
			info.Add<MemoflowItemPanelMediator>().With(RegisterFlags.NoCache);
			info.Add<MemoflowUIMediator>().With(RegisterFlags.NoCache);

            info.Add<BaseColorManagerInitializer>().As<IColorManagerInitializer>();

            return info;
		}
	}
}
