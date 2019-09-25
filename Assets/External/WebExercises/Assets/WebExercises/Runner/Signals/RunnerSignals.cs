using MinLibs.Signals;
using WebExercises.HUD;

namespace WebExercises.Runner
{
	public class OnStartExercise : Signal<string>
	{
		
	}

	public class OnShowDialogue: Signal<DialogueData>
	{
		
	}

	public class OnHideDialogue: Signal
	{
		
	}

	public class OnTick : Signal<float>
	{
		
	}
}