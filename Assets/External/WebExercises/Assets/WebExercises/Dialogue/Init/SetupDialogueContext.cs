using WebExercises.Shared;

namespace WebExercises.HUD
{
	public class SetupDialogueContext : SetupContext
	{
		protected override void SetupMediators()
		{
			base.SetupMediators();
			
			mediators.Map<IDialogue, DialogueMediator>();
		}
	}
}