using MinLibs.MVC;
using WebExercises.Shared;
using WebExercises.Shared.Editor;

namespace WebExercises.Exercise.Editor
{
	public class ExerciseRegistrations : WebExercisesRegistrations
	{
		public override RegistrationInfo Create()
		{
			var info = base.Create();

			info.Add<ExerciseSettings>().As<IExerciseSettings>();
			info.Add<ExerciseState>().As<IExerciseState>();
			info.Add<EndExercise>().As<IEndExercise>();
			info.Add<GenerateExerciseResult>().As<IGenerateExerciseResult>();
			info.Add<BlockApp>().As<IBlockApp>();

			info.Add<ExerciseControllerProxy>().As<IExerciseControllerProxy>();
			info.Add<ExerciseViewAdapter>().As<IExerciseViewAdapter>();
			info.Add<HUDViewAdapter>().As<IHUDViewAdapter>();
			info.Add<BackgroundViewAdapter>().As<IBackgroundViewAdapter>();

			info.Add<ExerciseRootMediator>().With(RegisterFlags.NoCache);
			
			info.Add<OnShowExercise>();
			info.Add<OnFinishInitialRound>();

			return info;
		}
	}
}