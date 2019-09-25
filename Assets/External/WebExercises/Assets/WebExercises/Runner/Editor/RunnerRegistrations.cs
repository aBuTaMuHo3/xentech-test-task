using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Utils;
using WebExercises.Exercise;
using WebExercises.Shared;
using WebExercises.Shared.Editor;

namespace WebExercises.Runner.Editor
{
	public class RunnerRegistrations : WebExercisesRegistrations
	{
		public override RegistrationInfo Create()
		{
			var info = base.Create();
			info.FilePath = "Modules/Runner/Init";

			info.Add<ILogging>().With(RegisterFlags.Parameter);
			
			info.Add<UnityPlatform>().As<IPlatform>();
			info.Add<UnityScreen>().As<IScreen>();
			info.Add<JsonParser>().As<IParser>();
			info.Add<UnityResources>().As<IResources>();
			info.Add<UnityInput>().As<IInput>();
			info.Add<AppState>().As<IAppState>();

			info.Add<SetupRunnerContext>().As<ISetupContext>();
			info.Add<RunnerSettings>().As<IRunnerSettings>();
			info.Add<SystemRandom>().As<IRandom>();
			info.Add<Timers>().As<ITimers>();
			info.Add<FrontendSender>().As<IFrontendSender>().In(EditorFlags.IsNotEditor);
			info.Add<FakeFrontendSender>().As<IFrontendSender>().In(EditorFlags.IsEditor);
			info.Add<Bundles>().As<IBundles>();
			info.Add<Scenes>().As<IScenes>();
			info.Add<HotKeys>().As<IHotKeys>();
			info.Add<Localization>().As<ILocalization>();

			info.Add<GetConfig>().As<IGetConfig>();
			info.Add<LoadBundle>().As<ILoadBundle>();
			info.Add<InitApp>().As<IInitApp>();
			info.Add<LoadExercise>().As<ILoadExercise>();
			info.Add<RemoveExercise>().As<IRemoveExercise>();

			info.Add<AppLifeCycleMediator>().As<IAppLifeCycleMediator>().With(RegisterFlags.NoCache).In(EditorFlags.IsNotEditor);
			info.Add<DummyAppLifeCycleMediator>().As<IAppLifeCycleMediator>().With(RegisterFlags.NoCache).In(EditorFlags.IsEditor);
			info.Add<FrontendReceiverMediator>().With(RegisterFlags.NoCache);
			
			info.Add<OnStartExercise>();
			info.Add<OnShowDialogue>();
			info.Add<OnHideDialogue>();
			info.Add<OnBlockApp>();
			info.Add<OnExerciseFinished>();

			return info;
		}
	}
}