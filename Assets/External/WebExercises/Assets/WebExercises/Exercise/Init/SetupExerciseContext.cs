using MinLibs.MVC;
using WebExercises.Runner;
using WebExercises.Shared;

namespace WebExercises.Exercise
{
	public class SetupExerciseContext : SetupContext
	{
		[Inject] public IExerciseControllerProxy controller;
		
		[Inject] public OnStartExercise onStartExercise;
		[Inject] public OnBlockApp onBlockApp;
		[Inject] public IStartExercise startExercise;
		[Inject] public IBlockApp blockApp;
		[Inject] public IEndExercise endExercise;

		protected override void SetupSignals()
		{
			base.SetupSignals();
			
			signals.Register(onStartExercise, startExercise.Execute);
			signals.Register(controller.OnEndExercise, endExercise.Execute);
			signals.Register(onBlockApp, blockApp.Execute);
		}

		protected override void SetupMediators()
		{
			base.SetupMediators();
			
			mediators.Map<IExerciseFade, ExerciseRootMediator>();
		}
	}
}