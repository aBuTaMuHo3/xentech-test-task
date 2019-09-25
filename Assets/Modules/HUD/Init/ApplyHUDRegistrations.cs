using MinLibs.MVC;

public class ApplyHUDRegistrations : IApplyRegistrations
{
    [Inject] public IContext context;

    public void Execute (params object[] args)
    {
// >>> PARAMETERS

// <<< PARAMETERS
      
            
// >>> DECLARATIONS
		var minLibsMVCContextSignals = new MinLibs.MVC.ContextSignals();
		var minLibsMVCMediators = new MinLibs.MVC.Mediators();
		var webExercisesHUDSetupHUDContext = new WebExercises.HUD.SetupHUDContext();
// <<< DECLARATIONS
            
            
// >>> REGISTRATIONS
		context.RegisterInstance<MinLibs.Signals.ISignalsManager>(minLibsMVCContextSignals, RegisterFlags.PreventInjections);
		context.RegisterInstance<MinLibs.MVC.IMediators>(minLibsMVCMediators, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.ISetupContext>(webExercisesHUDSetupHUDContext, RegisterFlags.PreventInjections);
// <<< REGISTRATIONS
            
            
// >>> HANDLERS
		context.RegisterHandler<MinLibs.MVC.RootMediator>(host => {
			var minLibsMVCRootMediator = new MinLibs.MVC.RootMediator();
			minLibsMVCRootMediator.mediators = context.Get<MinLibs.MVC.IMediators>();

			return minLibsMVCRootMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

		context.RegisterHandler<WebExercises.HUD.HUDRootMediator>(host => {
			var webExercisesHUDHUDRootMediator = new WebExercises.HUD.HUDRootMediator();
			webExercisesHUDHUDRootMediator.blockApp = context.Get<WebExercises.Exercise.IBlockApp>();
			webExercisesHUDHUDRootMediator.controller = context.Get<WebExercises.Shared.IExerciseControllerProxy>();

			return webExercisesHUDHUDRootMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

		context.RegisterHandler<WebExercises.HUD.HUDLevelMediator>(host => {
			var webExercisesHUDHUDLevelMediator = new WebExercises.HUD.HUDLevelMediator();
			webExercisesHUDHUDLevelMediator.localization = context.Get<WebExercises.Shared.ILocalization>();
			webExercisesHUDHUDLevelMediator.controller = context.Get<WebExercises.Shared.IExerciseControllerProxy>();

			return webExercisesHUDHUDLevelMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

		context.RegisterHandler<WebExercises.HUD.HUDScoreMediator>(host => {
			var webExercisesHUDHUDScoreMediator = new WebExercises.HUD.HUDScoreMediator();
			webExercisesHUDHUDScoreMediator.localization = context.Get<WebExercises.Shared.ILocalization>();
			webExercisesHUDHUDScoreMediator.controller = context.Get<WebExercises.Shared.IExerciseControllerProxy>();

			return webExercisesHUDHUDScoreMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

		context.RegisterHandler<WebExercises.HUD.HUDTimeMediator>(host => {
			var webExercisesHUDHUDTimeMediator = new WebExercises.HUD.HUDTimeMediator();
			webExercisesHUDHUDTimeMediator.exerciseState = context.Get<WebExercises.Exercise.IExerciseState>();
			webExercisesHUDHUDTimeMediator.localization = context.Get<WebExercises.Shared.ILocalization>();
			webExercisesHUDHUDTimeMediator.controller = context.Get<WebExercises.Shared.IExerciseControllerProxy>();

			return webExercisesHUDHUDTimeMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

		context.RegisterHandler<WebExercises.Shared.Components.FullscreenToggleMediator>(host => {
			var webExercisesSharedComponentsFullscreenToggleMediator = new WebExercises.Shared.Components.FullscreenToggleMediator();
			webExercisesSharedComponentsFullscreenToggleMediator.screen = context.Get<MinLibs.Utils.IScreen>();
			webExercisesSharedComponentsFullscreenToggleMediator.log = context.Get<MinLibs.Logging.ILogging>();

			return webExercisesSharedComponentsFullscreenToggleMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

// <<< HANDLERS
            
            
// >>> ASSIGNMENTS
		minLibsMVCMediators.context = context.Get<MinLibs.MVC.IContext>();

		webExercisesHUDSetupHUDContext.mediators = context.Get<MinLibs.MVC.IMediators>();
		webExercisesHUDSetupHUDContext.signals = context.Get<MinLibs.Signals.ISignalsManager>();

// <<< ASSIGNMENTS
            
            
// >>> CLEANUPS
		context.OnCleanUp.AddListener(minLibsMVCContextSignals.Cleanup);
// <<< CLEANUPS
            
            
// >>> POSTINJECTIONS

// <<< POSTINJECTIONS
    }
}
