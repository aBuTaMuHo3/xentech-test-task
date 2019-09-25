using MinLibs.MVC;

public class ApplyFlashGlanceRegistrations : IApplyRegistrations
{
    [Inject] public IContext context;

    public void Execute (params object[] args)
    {
// >>> PARAMETERS

// <<< PARAMETERS
      
            
// >>> DECLARATIONS
		var minLibsMVCContextSignals = new MinLibs.MVC.ContextSignals();
		var minLibsMVCMediators = new MinLibs.MVC.Mediators();
		var webExercisesExerciseExerciseSettings = new WebExercises.Exercise.ExerciseSettings();
		var webExercisesExerciseExerciseState = new WebExercises.Exercise.ExerciseState();
		var webExercisesExerciseEndExercise = new WebExercises.Exercise.EndExercise();
		var webExercisesSharedGenerateExerciseResult = new WebExercises.Shared.GenerateExerciseResult();
		var webExercisesExerciseBlockApp = new WebExercises.Exercise.BlockApp();
		var webExercisesSharedExerciseControllerProxy = new WebExercises.Shared.ExerciseControllerProxy();
		var webExercisesSharedExerciseViewAdapter = new WebExercises.Shared.ExerciseViewAdapter();
		var webExercisesSharedHUDViewAdapter = new WebExercises.Shared.HUDViewAdapter();
		var webExercisesSharedBackgroundViewAdapter = new WebExercises.Shared.BackgroundViewAdapter();
		var webExercisesExerciseOnShowExercise = new WebExercises.Exercise.OnShowExercise();
		var webExercisesExerciseOnFinishInitialRound = new WebExercises.Exercise.OnFinishInitialRound();
		var webExercisesFlashGlanceSetupFlashGlanceContext = new WebExercises.FlashGlance.SetupFlashGlanceContext();
		var webExercisesFlashGlanceStartFlashGlance = new WebExercises.FlashGlance.StartFlashGlance();
		var exerciseEngineColorsBaseColorManagerInitializer = new ExerciseEngine.Colors.BaseColorManagerInitializer();
// <<< DECLARATIONS
            
            
// >>> REGISTRATIONS
		context.RegisterInstance<MinLibs.Signals.ISignalsManager>(minLibsMVCContextSignals, RegisterFlags.PreventInjections);
		context.RegisterInstance<MinLibs.MVC.IMediators>(minLibsMVCMediators, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Exercise.IExerciseSettings>(webExercisesExerciseExerciseSettings, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Exercise.IExerciseState>(webExercisesExerciseExerciseState, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Exercise.IEndExercise>(webExercisesExerciseEndExercise, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.IGenerateExerciseResult>(webExercisesSharedGenerateExerciseResult, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Exercise.IBlockApp>(webExercisesExerciseBlockApp, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.IExerciseControllerProxy>(webExercisesSharedExerciseControllerProxy, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.IExerciseViewAdapter>(webExercisesSharedExerciseViewAdapter, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.IHUDViewAdapter>(webExercisesSharedHUDViewAdapter, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.IBackgroundViewAdapter>(webExercisesSharedBackgroundViewAdapter, RegisterFlags.PreventInjections);
		context.RegisterInstance(webExercisesExerciseOnShowExercise, RegisterFlags.PreventInjections);
		context.RegisterInstance(webExercisesExerciseOnFinishInitialRound, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.ISetupContext>(webExercisesFlashGlanceSetupFlashGlanceContext, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Exercise.IStartExercise>(webExercisesFlashGlanceStartFlashGlance, RegisterFlags.PreventInjections);
		context.RegisterInstance<ExerciseEngine.Colors.IColorManagerInitializer>(exerciseEngineColorsBaseColorManagerInitializer, RegisterFlags.PreventInjections);
// <<< REGISTRATIONS
            
            
// >>> HANDLERS
		context.RegisterHandler<MinLibs.MVC.RootMediator>(host => {
			var minLibsMVCRootMediator = new MinLibs.MVC.RootMediator();
			minLibsMVCRootMediator.mediators = context.Get<MinLibs.MVC.IMediators>();

			return minLibsMVCRootMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

		context.RegisterHandler<WebExercises.Exercise.ExerciseRootMediator>(host => {
			var webExercisesExerciseExerciseRootMediator = new WebExercises.Exercise.ExerciseRootMediator();
			webExercisesExerciseExerciseRootMediator.onShowExercise = context.Get<WebExercises.Exercise.OnShowExercise>();

			return webExercisesExerciseExerciseRootMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

		context.RegisterHandler<WebExercises.FlashGlance.FlashGlanceViewMediator>(host => {
			var webExercisesFlashGlanceFlashGlanceViewMediator = new WebExercises.FlashGlance.FlashGlanceViewMediator();
			webExercisesFlashGlanceFlashGlanceViewMediator.controller = context.Get<WebExercises.Shared.IExerciseControllerProxy>();

			return webExercisesFlashGlanceFlashGlanceViewMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

// <<< HANDLERS
            
            
// >>> ASSIGNMENTS
		minLibsMVCMediators.context = context.Get<MinLibs.MVC.IContext>();

		webExercisesExerciseEndExercise.context = context.Get<MinLibs.MVC.IContext>();
		webExercisesExerciseEndExercise.parser = context.Get<MinLibs.Utils.IParser>();
		webExercisesExerciseEndExercise.onExerciseFinished = context.Get<WebExercises.Shared.OnExerciseFinished>();

		webExercisesSharedGenerateExerciseResult.state = context.Get<WebExercises.Exercise.IExerciseState>();

		webExercisesExerciseBlockApp.controller = context.Get<WebExercises.Shared.IExerciseControllerProxy>();
		webExercisesExerciseBlockApp.appState = context.Get<WebExercises.Runner.IAppState>();
		webExercisesExerciseBlockApp.localization = context.Get<WebExercises.Shared.ILocalization>();
		webExercisesExerciseBlockApp.onShowDialogue = context.Get<WebExercises.Runner.OnShowDialogue>();
		webExercisesExerciseBlockApp.onHideDialogue = context.Get<WebExercises.Runner.OnHideDialogue>();

		webExercisesSharedExerciseControllerProxy.generateExerciseResult = context.Get<WebExercises.Shared.IGenerateExerciseResult>();

		webExercisesSharedExerciseViewAdapter.parser = context.Get<MinLibs.Utils.IParser>();
		webExercisesSharedExerciseViewAdapter.logger = context.Get<MinLibs.Logging.ILogging>();
		webExercisesSharedExerciseViewAdapter.controllerProxy = context.Get<WebExercises.Shared.IExerciseControllerProxy>();

		webExercisesSharedHUDViewAdapter.logger = context.Get<MinLibs.Logging.ILogging>();
		webExercisesSharedHUDViewAdapter.parser = context.Get<MinLibs.Utils.IParser>();
		webExercisesSharedHUDViewAdapter.controllerProxy = context.Get<WebExercises.Shared.IExerciseControllerProxy>();

		webExercisesFlashGlanceSetupFlashGlanceContext.controller = context.Get<WebExercises.Shared.IExerciseControllerProxy>();
		webExercisesFlashGlanceSetupFlashGlanceContext.onStartExercise = context.Get<WebExercises.Runner.OnStartExercise>();
		webExercisesFlashGlanceSetupFlashGlanceContext.onBlockApp = context.Get<WebExercises.Shared.OnBlockApp>();
		webExercisesFlashGlanceSetupFlashGlanceContext.startExercise = context.Get<WebExercises.Exercise.IStartExercise>();
		webExercisesFlashGlanceSetupFlashGlanceContext.blockApp = context.Get<WebExercises.Exercise.IBlockApp>();
		webExercisesFlashGlanceSetupFlashGlanceContext.endExercise = context.Get<WebExercises.Exercise.IEndExercise>();
		webExercisesFlashGlanceSetupFlashGlanceContext.mediators = context.Get<MinLibs.MVC.IMediators>();
		webExercisesFlashGlanceSetupFlashGlanceContext.signals = context.Get<MinLibs.Signals.ISignalsManager>();

		webExercisesFlashGlanceStartFlashGlance.colorManager = context.Get<ExerciseEngine.Colors.IColorManagerInitializer>();
		webExercisesFlashGlanceStartFlashGlance.context = context.Get<MinLibs.MVC.IContext>();
		webExercisesFlashGlanceStartFlashGlance.platform = context.Get<MinLibs.Utils.IPlatform>();
		webExercisesFlashGlanceStartFlashGlance.parser = context.Get<MinLibs.Utils.IParser>();
		webExercisesFlashGlanceStartFlashGlance.runnerSettings = context.Get<WebExercises.Runner.IRunnerSettings>();
		webExercisesFlashGlanceStartFlashGlance.exerciseSettings = context.Get<WebExercises.Exercise.IExerciseSettings>();
		webExercisesFlashGlanceStartFlashGlance.exerciseState = context.Get<WebExercises.Exercise.IExerciseState>();
		webExercisesFlashGlanceStartFlashGlance.getConfig = context.Get<WebExercises.Runner.IGetConfig>();
		webExercisesFlashGlanceStartFlashGlance.exerciseController = context.Get<WebExercises.Shared.IExerciseControllerProxy>();
		webExercisesFlashGlanceStartFlashGlance.exerciseViewAdapter = context.Get<WebExercises.Shared.IExerciseViewAdapter>();
		webExercisesFlashGlanceStartFlashGlance.hudViewAdapter = context.Get<WebExercises.Shared.IHUDViewAdapter>();
		webExercisesFlashGlanceStartFlashGlance.backgroundViewAdapter = context.Get<WebExercises.Shared.IBackgroundViewAdapter>();
		webExercisesFlashGlanceStartFlashGlance.onShowExercise = webExercisesExerciseOnShowExercise;

// <<< ASSIGNMENTS
            
            
// >>> CLEANUPS
		context.OnCleanUp.AddListener(minLibsMVCContextSignals.Cleanup);
// <<< CLEANUPS
            
            
// >>> POSTINJECTIONS

// <<< POSTINJECTIONS
    }
}
