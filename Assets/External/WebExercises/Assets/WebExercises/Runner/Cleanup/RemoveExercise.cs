using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Utils;

namespace WebExercises.Runner
{
	public interface IRemoveExercise
	{
		void Execute(string data);
	}

	public class RemoveExercise : IRemoveExercise
	{
		[Inject] public ILogging log;
		[Inject] public IPlatform platform;
		[Inject] public IRunnerSettings runnerSettings;
		[Inject] public IBundles bundles;
		[Inject] public IScenes scenes;
		[Inject] public IFrontendSender sender;
		
		public async void Execute(string data)
		{
			log.Trace(">>>>>>>>> REMOVE EXERCISE");
			
			await scenes.UnloadAll();
			
			bundles.Remove(runnerSettings.ExerciseInfo.Bundles);

			log.Trace(">>>>>>>>> ON COMPLETE EXERCISE " + data);

			platform.CaptureInput = false;
			
			sender.OnCompleteExercise(data);
		}
	}
}
