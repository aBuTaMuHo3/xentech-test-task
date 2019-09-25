using MinLibs.Signals;

namespace WebExercises.Shared
{
	public class OnBlockApp : Signal<bool>
	{
	}

	public class OnExerciseFinished : Signal<string>
	{
	}
}