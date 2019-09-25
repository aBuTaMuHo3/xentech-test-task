using MinLibs.MVC;

public class ApplyMemoflowRegistrations : IApplyRegistrations
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
		var webExercisesMemoflowSetupMemoflowContext = new WebExercises.Memoflow.SetupMemoflowContext();
		var webExercisesMemoflowStartMemoflow = new WebExercises.Memoflow.StartMemoflow();
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
		context.RegisterInstance<WebExercises.Shared.ISetupContext>(webExercisesMemoflowSetupMemoflowContext, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Exercise.IStartExercise>(webExercisesMemoflowStartMemoflow, RegisterFlags.PreventInjections);
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

		context.RegisterHandler<WebExercises.Memoflow.MemoflowItemPanelMediator>(host => {
			var webExercisesMemoflowMemoflowItemPanelMediator = new WebExercises.Memoflow.MemoflowItemPanelMediator();
			webExercisesMemoflowMemoflowItemPanelMediator.controller = context.Get<WebExercises.Shared.IExerciseControllerProxy>();
			webExercisesMemoflowMemoflowItemPanelMediator.settings = context.Get<WebExercises.Exercise.IExerciseSettings>();
			webExercisesMemoflowMemoflowItemPanelMediator.onFinishInitialRound = webExercisesExerciseOnFinishInitialRound;

			return webExercisesMemoflowMemoflowItemPanelMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

		context.RegisterHandler<WebExercises.Memoflow.MemoflowUIMediator>(host => {
			var webExercisesMemoflowMemoflowUIMediator = new WebExercises.Memoflow.MemoflowUIMediator();
			webExercisesMemoflowMemoflowUIMediator.controller = context.Get<WebExercises.Shared.IExerciseControllerProxy>();
			webExercisesMemoflowMemoflowUIMediator.onFinishInitialRound = webExercisesExerciseOnFinishInitialRound;
			webExercisesMemoflowMemoflowUIMediator.hotKeys = context.Get<WebExercises.Shared.IHotKeys>();
			webExercisesMemoflowMemoflowUIMediator.localization = context.Get<WebExercises.Shared.ILocalization>();

			return webExercisesMemoflowMemoflowUIMediator;
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

		webExercisesMemoflowSetupMemoflowContext.controller = context.Get<WebExercises.Shared.IExerciseControllerProxy>();
		webExercisesMemoflowSetupMemoflowContext.onStartExercise = context.Get<WebExercises.Runner.OnStartExercise>();
		webExercisesMemoflowSetupMemoflowContext.onBlockApp = context.Get<WebExercises.Shared.OnBlockApp>();
		webExercisesMemoflowSetupMemoflowContext.startExercise = context.Get<WebExercises.Exercise.IStartExercise>();
		webExercisesMemoflowSetupMemoflowContext.blockApp = context.Get<WebExercises.Exercise.IBlockApp>();
		webExercisesMemoflowSetupMemoflowContext.endExercise = context.Get<WebExercises.Exercise.IEndExercise>();
		webExercisesMemoflowSetupMemoflowContext.mediators = context.Get<MinLibs.MVC.IMediators>();
		webExercisesMemoflowSetupMemoflowContext.signals = context.Get<MinLibs.Signals.ISignalsManager>();

		webExercisesMemoflowStartMemoflow.colorManager = context.Get<ExerciseEngine.Colors.IColorManagerInitializer>();
		webExercisesMemoflowStartMemoflow.context = context.Get<MinLibs.MVC.IContext>();
		webExercisesMemoflowStartMemoflow.platform = context.Get<MinLibs.Utils.IPlatform>();
		webExercisesMemoflowStartMemoflow.parser = context.Get<MinLibs.Utils.IParser>();
		webExercisesMemoflowStartMemoflow.runnerSettings = context.Get<WebExercises.Runner.IRunnerSettings>();
		webExercisesMemoflowStartMemoflow.exerciseSettings = context.Get<WebExercises.Exercise.IExerciseSettings>();
		webExercisesMemoflowStartMemoflow.exerciseState = context.Get<WebExercises.Exercise.IExerciseState>();
		webExercisesMemoflowStartMemoflow.getConfig = context.Get<WebExercises.Runner.IGetConfig>();
		webExercisesMemoflowStartMemoflow.exerciseController = context.Get<WebExercises.Shared.IExerciseControllerProxy>();
		webExercisesMemoflowStartMemoflow.exerciseViewAdapter = context.Get<WebExercises.Shared.IExerciseViewAdapter>();
		webExercisesMemoflowStartMemoflow.hudViewAdapter = context.Get<WebExercises.Shared.IHUDViewAdapter>();
		webExercisesMemoflowStartMemoflow.backgroundViewAdapter = context.Get<WebExercises.Shared.IBackgroundViewAdapter>();
		webExercisesMemoflowStartMemoflow.onShowExercise = webExercisesExerciseOnShowExercise;

// <<< ASSIGNMENTS
            
            
// >>> CLEANUPS
		context.OnCleanUp.AddListener(minLibsMVCContextSignals.Cleanup);
// <<< CLEANUPS
            
            
// >>> POSTINJECTIONS

// <<< POSTINJECTIONS
    }
}
