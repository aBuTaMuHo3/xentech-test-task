using MinLibs.MVC;
using MinLibs.Utils;
using WebExercises.Shared;

namespace WebExercises.Exercise
{
	public interface IEndExercise
	{
		void Execute(IExerciseResult result);
	}

	public class EndExercise : IEndExercise
	{
		[Inject] public IContext context;
		[Inject] public IParser parser;
		[Inject] public OnExerciseFinished onExerciseFinished;
		
		public void Execute(IExerciseResult result)
		{
			context.CleanUp();
			
			var resultString = parser.Serialize(result);
			onExerciseFinished.Dispatch(resultString);
		}
	}
}