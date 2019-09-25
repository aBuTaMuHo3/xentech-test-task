using MinLibs.MVC;

public class ApplyMinHUDRegistrations : IApplyRegistrations
{
    [Inject] public IContext context;

    public void Execute (params object[] args)
    {
// >>> PARAMETERS

// <<< PARAMETERS
      
            
// >>> DECLARATIONS
		var minLibsMVCContextSignals = new MinLibs.MVC.ContextSignals();
		var minLibsMVCMediators = new MinLibs.MVC.Mediators();
		var webExercisesMinHUDSetupMinHUDContext = new WebExercises.MinHUD.SetupMinHUDContext();
// <<< DECLARATIONS
            
            
// >>> REGISTRATIONS
		context.RegisterInstance<MinLibs.Signals.ISignalsManager>(minLibsMVCContextSignals, RegisterFlags.PreventInjections);
		context.RegisterInstance<MinLibs.MVC.IMediators>(minLibsMVCMediators, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.ISetupContext>(webExercisesMinHUDSetupMinHUDContext, RegisterFlags.PreventInjections);
// <<< REGISTRATIONS
            
            
// >>> HANDLERS
		context.RegisterHandler<MinLibs.MVC.RootMediator>(host => {
			var minLibsMVCRootMediator = new MinLibs.MVC.RootMediator();
			minLibsMVCRootMediator.mediators = context.Get<MinLibs.MVC.IMediators>();

			return minLibsMVCRootMediator;
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

		webExercisesMinHUDSetupMinHUDContext.mediators = context.Get<MinLibs.MVC.IMediators>();
		webExercisesMinHUDSetupMinHUDContext.signals = context.Get<MinLibs.Signals.ISignalsManager>();

// <<< ASSIGNMENTS
            
            
// >>> CLEANUPS
		context.OnCleanUp.AddListener(minLibsMVCContextSignals.Cleanup);
// <<< CLEANUPS
            
            
// >>> POSTINJECTIONS

// <<< POSTINJECTIONS
    }
}
