using MinLibs.MVC;

public class ApplyRunnerRegistrations : IApplyRegistrations
{
    [Inject] public IContext context;

    public void Execute (params object[] args)
    {
// >>> PARAMETERS
		var minLibsLoggingILogging = (MinLibs.Logging.ILogging) args[0];
// <<< PARAMETERS
      
            
// >>> DECLARATIONS
		var minLibsMVCContextSignals = new MinLibs.MVC.ContextSignals();
		var minLibsMVCMediators = new MinLibs.MVC.Mediators();
		var minLibsUtilsUnityPlatform = new MinLibs.Utils.UnityPlatform();
		var minLibsUtilsUnityScreen = new MinLibs.Utils.UnityScreen();
		var webExercisesSharedJsonParser = new WebExercises.Shared.JsonParser();
		var minLibsUtilsUnityResources = new MinLibs.Utils.UnityResources();
		var minLibsUtilsUnityInput = new MinLibs.Utils.UnityInput();
		var webExercisesRunnerAppState = new WebExercises.Runner.AppState();
		var webExercisesRunnerSetupRunnerContext = new WebExercises.Runner.SetupRunnerContext();
		var webExercisesRunnerRunnerSettings = new WebExercises.Runner.RunnerSettings();
		var minLibsUtilsSystemRandom = new MinLibs.Utils.SystemRandom();
		var minLibsMVCTimers = new MinLibs.MVC.Timers();
		var webExercisesRunnerFrontendSender = new WebExercises.Runner.FrontendSender();
		var webExercisesRunnerBundles = new WebExercises.Runner.Bundles();
		var webExercisesRunnerScenes = new WebExercises.Runner.Scenes();
		var webExercisesSharedHotKeys = new WebExercises.Shared.HotKeys();
		var webExercisesSharedLocalization = new WebExercises.Shared.Localization();
		var webExercisesRunnerGetConfig = new WebExercises.Runner.GetConfig();
		var webExercisesRunnerLoadBundle = new WebExercises.Runner.LoadBundle();
		var webExercisesRunnerInitApp = new WebExercises.Runner.InitApp();
		var webExercisesRunnerLoadExercise = new WebExercises.Runner.LoadExercise();
		var webExercisesRunnerRemoveExercise = new WebExercises.Runner.RemoveExercise();
		var webExercisesRunnerOnStartExercise = new WebExercises.Runner.OnStartExercise();
		var webExercisesRunnerOnShowDialogue = new WebExercises.Runner.OnShowDialogue();
		var webExercisesRunnerOnHideDialogue = new WebExercises.Runner.OnHideDialogue();
		var webExercisesSharedOnBlockApp = new WebExercises.Shared.OnBlockApp();
		var webExercisesSharedOnExerciseFinished = new WebExercises.Shared.OnExerciseFinished();
// <<< DECLARATIONS
            
            
// >>> REGISTRATIONS
		context.RegisterInstance<MinLibs.Signals.ISignalsManager>(minLibsMVCContextSignals, RegisterFlags.PreventInjections);
		context.RegisterInstance<MinLibs.MVC.IMediators>(minLibsMVCMediators, RegisterFlags.PreventInjections);
		context.RegisterInstance(minLibsLoggingILogging, RegisterFlags.None);
		context.RegisterInstance<MinLibs.Utils.IPlatform>(minLibsUtilsUnityPlatform, RegisterFlags.PreventInjections);
		context.RegisterInstance<MinLibs.Utils.IScreen>(minLibsUtilsUnityScreen, RegisterFlags.PreventInjections);
		context.RegisterInstance<MinLibs.Utils.IParser>(webExercisesSharedJsonParser, RegisterFlags.PreventInjections);
		context.RegisterInstance<MinLibs.Utils.IResources>(minLibsUtilsUnityResources, RegisterFlags.PreventInjections);
		context.RegisterInstance<MinLibs.Utils.IInput>(minLibsUtilsUnityInput, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Runner.IAppState>(webExercisesRunnerAppState, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.ISetupContext>(webExercisesRunnerSetupRunnerContext, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Runner.IRunnerSettings>(webExercisesRunnerRunnerSettings, RegisterFlags.PreventInjections);
		context.RegisterInstance<MinLibs.Utils.IRandom>(minLibsUtilsSystemRandom, RegisterFlags.PreventInjections);
		context.RegisterInstance<MinLibs.MVC.ITimers>(minLibsMVCTimers, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Runner.IFrontendSender>(webExercisesRunnerFrontendSender, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Runner.IBundles>(webExercisesRunnerBundles, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Runner.IScenes>(webExercisesRunnerScenes, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.IHotKeys>(webExercisesSharedHotKeys, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.ILocalization>(webExercisesSharedLocalization, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Runner.IGetConfig>(webExercisesRunnerGetConfig, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Runner.ILoadBundle>(webExercisesRunnerLoadBundle, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Runner.IInitApp>(webExercisesRunnerInitApp, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Runner.ILoadExercise>(webExercisesRunnerLoadExercise, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Runner.IRemoveExercise>(webExercisesRunnerRemoveExercise, RegisterFlags.PreventInjections);
		context.RegisterInstance(webExercisesRunnerOnStartExercise, RegisterFlags.PreventInjections);
		context.RegisterInstance(webExercisesRunnerOnShowDialogue, RegisterFlags.PreventInjections);
		context.RegisterInstance(webExercisesRunnerOnHideDialogue, RegisterFlags.PreventInjections);
		context.RegisterInstance(webExercisesSharedOnBlockApp, RegisterFlags.PreventInjections);
		context.RegisterInstance(webExercisesSharedOnExerciseFinished, RegisterFlags.PreventInjections);
// <<< REGISTRATIONS
            
            
// >>> HANDLERS
		context.RegisterHandler<MinLibs.MVC.RootMediator>(host => {
			var minLibsMVCRootMediator = new MinLibs.MVC.RootMediator();
			minLibsMVCRootMediator.mediators = context.Get<MinLibs.MVC.IMediators>();

			return minLibsMVCRootMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

		context.RegisterHandler<WebExercises.Runner.IAppLifeCycleMediator>(host => {
			var webExercisesRunnerAppLifeCycleMediator = new WebExercises.Runner.AppLifeCycleMediator();
			webExercisesRunnerAppLifeCycleMediator.appState = context.Get<WebExercises.Runner.IAppState>();
			webExercisesRunnerAppLifeCycleMediator.onBlockApp = context.Get<WebExercises.Shared.OnBlockApp>();

			return webExercisesRunnerAppLifeCycleMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

		context.RegisterHandler<WebExercises.Runner.FrontendReceiverMediator>(host => {
			var webExercisesRunnerFrontendReceiverMediator = new WebExercises.Runner.FrontendReceiverMediator();
			webExercisesRunnerFrontendReceiverMediator.initApp = context.Get<WebExercises.Runner.IInitApp>();
			webExercisesRunnerFrontendReceiverMediator.loadExercise = context.Get<WebExercises.Runner.ILoadExercise>();
			webExercisesRunnerFrontendReceiverMediator.onStartExercise = context.Get<WebExercises.Runner.OnStartExercise>();

			return webExercisesRunnerFrontendReceiverMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

// <<< HANDLERS
            
            
// >>> ASSIGNMENTS
		minLibsMVCMediators.context = context.Get<MinLibs.MVC.IContext>();

		webExercisesRunnerSetupRunnerContext.sender = context.Get<WebExercises.Runner.IFrontendSender>();
		webExercisesRunnerSetupRunnerContext.onExerciseFinished = webExercisesSharedOnExerciseFinished;
		webExercisesRunnerSetupRunnerContext.removeExercise = context.Get<WebExercises.Runner.IRemoveExercise>();
		webExercisesRunnerSetupRunnerContext.screen = context.Get<MinLibs.Utils.IScreen>();
		webExercisesRunnerSetupRunnerContext.hotKeys = context.Get<WebExercises.Shared.IHotKeys>();
		webExercisesRunnerSetupRunnerContext.mediators = context.Get<MinLibs.MVC.IMediators>();
		webExercisesRunnerSetupRunnerContext.signals = context.Get<MinLibs.Signals.ISignalsManager>();

		webExercisesSharedHotKeys.input = context.Get<MinLibs.Utils.IInput>();

		webExercisesRunnerGetConfig.settings = context.Get<WebExercises.Runner.IRunnerSettings>();
		webExercisesRunnerGetConfig.logger = minLibsLoggingILogging;
		webExercisesRunnerGetConfig.parser = context.Get<MinLibs.Utils.IParser>();

		webExercisesRunnerLoadBundle.log = minLibsLoggingILogging;
		webExercisesRunnerLoadBundle.settings = context.Get<WebExercises.Runner.IRunnerSettings>();
		webExercisesRunnerLoadBundle.bundles = context.Get<WebExercises.Runner.IBundles>();
		webExercisesRunnerLoadBundle.scenes = context.Get<WebExercises.Runner.IScenes>();

		webExercisesRunnerInitApp.platform = context.Get<MinLibs.Utils.IPlatform>();
		webExercisesRunnerInitApp.log = minLibsLoggingILogging;
		webExercisesRunnerInitApp.parser = context.Get<MinLibs.Utils.IParser>();
		webExercisesRunnerInitApp.settings = context.Get<WebExercises.Runner.IRunnerSettings>();
		webExercisesRunnerInitApp.localization = context.Get<WebExercises.Shared.ILocalization>();
		webExercisesRunnerInitApp.sender = context.Get<WebExercises.Runner.IFrontendSender>();
		webExercisesRunnerInitApp.loadBundle = context.Get<WebExercises.Runner.ILoadBundle>();

		webExercisesRunnerLoadExercise.context = context.Get<MinLibs.MVC.IContext>();
		webExercisesRunnerLoadExercise.log = minLibsLoggingILogging;
		webExercisesRunnerLoadExercise.parser = context.Get<MinLibs.Utils.IParser>();
		webExercisesRunnerLoadExercise.settings = context.Get<WebExercises.Runner.IRunnerSettings>();
		webExercisesRunnerLoadExercise.localization = context.Get<WebExercises.Shared.ILocalization>();
		webExercisesRunnerLoadExercise.loadBundle = context.Get<WebExercises.Runner.ILoadBundle>();
		webExercisesRunnerLoadExercise.sender = context.Get<WebExercises.Runner.IFrontendSender>();
		webExercisesRunnerLoadExercise.scenes = context.Get<WebExercises.Runner.IScenes>();

		webExercisesRunnerRemoveExercise.log = minLibsLoggingILogging;
		webExercisesRunnerRemoveExercise.platform = context.Get<MinLibs.Utils.IPlatform>();
		webExercisesRunnerRemoveExercise.runnerSettings = context.Get<WebExercises.Runner.IRunnerSettings>();
		webExercisesRunnerRemoveExercise.bundles = context.Get<WebExercises.Runner.IBundles>();
		webExercisesRunnerRemoveExercise.scenes = context.Get<WebExercises.Runner.IScenes>();
		webExercisesRunnerRemoveExercise.sender = context.Get<WebExercises.Runner.IFrontendSender>();

// <<< ASSIGNMENTS
            
            
// >>> CLEANUPS
		context.OnCleanUp.AddListener(minLibsMVCContextSignals.Cleanup);
// <<< CLEANUPS
            
            
// >>> POSTINJECTIONS

// <<< POSTINJECTIONS
    }
}
