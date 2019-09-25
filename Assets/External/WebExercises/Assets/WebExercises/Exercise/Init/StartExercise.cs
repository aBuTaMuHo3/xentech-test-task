using System.Collections.Generic;
using System.Threading.Tasks;
using ExerciseEngine.Controller;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.Interfaces;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Settings.Enums;
using ExerciseEngine.Terminator.Interfaces;
using Exercises.Exercises;
using Exercises.Utils;
using MinLibs.MVC;
using MinLibs.Utils;
using SynaptikonFramework.Interfaces.DebugLog;
using WebExercises.Runner;
using WebExercises.Shared;

namespace WebExercises.Exercise
{
	public interface IStartExercise
	{
		void Execute(string data);
	}

	public abstract class StartExercise<T> : IStartExercise where T : class, IExerciseConfiguration
	{
		[Inject] public IContext context;
		[Inject] public IPlatform platform;
		[Inject] public IParser parser;
		[Inject] public IRunnerSettings runnerSettings;
		[Inject] public IExerciseSettings exerciseSettings;
		[Inject] public IExerciseState exerciseState;
		[Inject] public IGetConfig getConfig;
		[Inject] public IExerciseControllerProxy exerciseController;
		[Inject] public IExerciseViewAdapter exerciseViewAdapter;
		[Inject] public IHUDViewAdapter hudViewAdapter;
		[Inject] public IBackgroundViewAdapter backgroundViewAdapter;
		[Inject] public OnShowExercise onShowExercise;

		public async void Execute(string data)
		{
			platform.CaptureInput = true;

			var options = GetOptions<ExerciseDetails>(data);
			exerciseSettings.Difficulty = options.Difficulty;
			exerciseSettings.Mode = options.Mode;
			exerciseSettings.Options = options.Options;

			await PrepareEngine();
			var config = await InitController();
			SetInitialModelData(config);

			exerciseController.Controller.Start();
			onShowExercise.Dispatch();
		}

		protected virtual Task PrepareEngine()
		{
			return Task.CompletedTask;
		}

		private async Task<T> InitController()
		{
			var config = await LoadConfig();
			var logger = new NNLogger(platform.EnableLogging == 0 ? LogLevel.Off : LogLevel.Verbose);
			var options = GetOptions<ExerciseOptions>();
			exerciseState.Init(options);
			var initVO = CreateModelInit();
			var model = CreateModel(initVO, config, logger);
			var terminator = context.Create<ExerciseTerminator>();
			var timerFactory = new NNUnityTimerFactory();
			var soundManager = new ExerciseSoundManager(logger);
			var controller = CreateController(logger, model, terminator, timerFactory, soundManager);
			exerciseController.Controller = controller;

			return config;
		}

		private async Task<T> LoadConfig()
		{
			var appConfig = await LoadConfig<AppConfig<T>>("config");

			return appConfig.Exercise.Config;
		}

		private ExerciseInitVO CreateModelInit()
		{
			var exSettings = new Dictionary<ExerciseSettingsEnum, bool>();

			return new ExerciseInitVO(exerciseSettings.Difficulty, ExerciseMode.Normal, exSettings, false, false);
		}

		protected abstract IExerciseModel CreateModel(ExerciseInitVO initVO, IExerciseConfiguration config,
			ILogger logger);

		protected virtual BaseExerciseController CreateController(NNLogger logger, IExerciseModel model,
			IExerciseTerminator terminator,
			NNUnityTimerFactory timerFactory, ExerciseSoundManager soundManager)
		{
			return new BaseExerciseController(model, exerciseViewAdapter, backgroundViewAdapter, hudViewAdapter,
				timerFactory, terminator, logger, soundManager);
		}

		private T GetOptions<T>(string data = null)
		{
			data = data ?? exerciseSettings.Options;
			return parser.Deserialize<T>(data);
		}

		protected virtual void SetInitialModelData(T config)
		{
		}

		protected void SetInitialModelData(IRoundIndependentVO data)
		{
			exerciseController.Controller.InitialModelData = data;
		}

		protected Task<T> LoadConfig<T>(string id) where T : class
		{
			var path = runnerSettings.ExerciseInfo.Configs[id];
			
			return getConfig.Execute<T>(path);
		}
	}
}