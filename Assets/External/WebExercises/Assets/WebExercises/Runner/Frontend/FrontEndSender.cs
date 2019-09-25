using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Utils;

namespace WebExercises.Runner
{
	public class FrontendSender : IFrontendSender
	{
		[DllImport("__Internal")]
		private static extern void AppReady();

		[DllImport("__Internal")]
		private static extern void EngineReady();

		[DllImport("__Internal")]
		private static extern void ExerciseReady();

		[DllImport("__Internal")]
		private static extern void CompleteExercise(string data);

		[DllImport("__Internal")]
		private static extern void CancelExercise(string data);

		public void OnAppReady()
		{
			AppReady();
		}

		public void OnEngineReady()
		{
			EngineReady();
		}

		public void OnExerciseReady()
		{
			ExerciseReady();
		}

		public void OnCompleteExercise(string data)
		{
			CompleteExercise(data);
		}

		public void OnCancelExercise(string data)
		{
			CancelExercise(data);
		}
	}

	public class FakeFrontendSender : IFrontendSender
	{
		[Inject] public ILogging logger;
		[Inject] public IRunnerSettings settings;
		[Inject] public IResources resources;
		[Inject] public IGetConfig getConfig;
		[Inject] public IInitApp initApp;
		[Inject] public ILoadExercise loadExercise;
		[Inject] public OnStartExercise onStartExercise;
		
		public async void OnAppReady()
		{
			var appSettings = resources.Load<AppSettings>(RunnerConsts.APP_SETTINGS);
			settings.BasePath = Path.Combine(appSettings.url, appSettings.basePath);
			var config = await Load("Configs/AppConfig.json");
			initApp.Execute(config);
		}

		public async void OnEngineReady()
		{
			var config = await Load("Configs/" + settings.ExerciseId + "/Settings.json");
			loadExercise.Execute(config);
		}

		public async void OnExerciseReady()
		{
			var path = "Configs/" + settings.ExerciseId + "/Options_" + settings.Mode + ".json";
			var config = await Load(path);
			onStartExercise.Dispatch(config);
		}

		public async void OnCompleteExercise(string data)
		{
			OnEngineReady();
		}

		public async void OnCancelExercise(string data)
		{
			OnEngineReady();
		}

		private async Task<string> Load(string path)
		{
			var config = await getConfig.Execute<string>(path);
			
			return config;
		}
	}

	public interface IFrontendSender
	{
		void OnAppReady();
		void OnEngineReady();
		void OnExerciseReady();
		void OnCompleteExercise(string data);
		void OnCancelExercise(string data);
	}
}