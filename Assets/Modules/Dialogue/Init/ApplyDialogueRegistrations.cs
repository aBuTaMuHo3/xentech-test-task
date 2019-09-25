using MinLibs.MVC;

public class ApplyDialogueRegistrations : IApplyRegistrations
{
    [Inject] public IContext context;

    public void Execute (params object[] args)
    {
// >>> PARAMETERS

// <<< PARAMETERS
      
            
// >>> DECLARATIONS
		var minLibsMVCContextSignals = new MinLibs.MVC.ContextSignals();
		var minLibsMVCMediators = new MinLibs.MVC.Mediators();
		var webExercisesHUDSetupDialogueContext = new WebExercises.HUD.SetupDialogueContext();
// <<< DECLARATIONS
            
            
// >>> REGISTRATIONS
		context.RegisterInstance<MinLibs.Signals.ISignalsManager>(minLibsMVCContextSignals, RegisterFlags.PreventInjections);
		context.RegisterInstance<MinLibs.MVC.IMediators>(minLibsMVCMediators, RegisterFlags.PreventInjections);
		context.RegisterInstance<WebExercises.Shared.ISetupContext>(webExercisesHUDSetupDialogueContext, RegisterFlags.PreventInjections);
// <<< REGISTRATIONS
            
            
// >>> HANDLERS
		context.RegisterHandler<MinLibs.MVC.RootMediator>(host => {
			var minLibsMVCRootMediator = new MinLibs.MVC.RootMediator();
			minLibsMVCRootMediator.mediators = context.Get<MinLibs.MVC.IMediators>();

			return minLibsMVCRootMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

		context.RegisterHandler<WebExercises.HUD.DialogueMediator>(host => {
			var webExercisesHUDDialogueMediator = new WebExercises.HUD.DialogueMediator();
			webExercisesHUDDialogueMediator.onShowDialogue = context.Get<WebExercises.Runner.OnShowDialogue>();
			webExercisesHUDDialogueMediator.onHideDialogue = context.Get<WebExercises.Runner.OnHideDialogue>();

			return webExercisesHUDDialogueMediator;
		}, RegisterFlags.NoCache | RegisterFlags.PreventInjections);

// <<< HANDLERS
            
            
// >>> ASSIGNMENTS
		minLibsMVCMediators.context = context.Get<MinLibs.MVC.IContext>();

		webExercisesHUDSetupDialogueContext.mediators = context.Get<MinLibs.MVC.IMediators>();
		webExercisesHUDSetupDialogueContext.signals = context.Get<MinLibs.Signals.ISignalsManager>();

// <<< ASSIGNMENTS
            
            
// >>> CLEANUPS
		context.OnCleanUp.AddListener(minLibsMVCContextSignals.Cleanup);
// <<< CLEANUPS
            
            
// >>> POSTINJECTIONS

// <<< POSTINJECTIONS
    }
}
