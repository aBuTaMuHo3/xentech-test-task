using System.Threading.Tasks;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using MinLibs.MVC;
using WebExercises.Shared;

namespace WebExercises.Exercise
{
	public interface IExerciseFade : IMediatedBehaviour
	{
		Task Show();
		Task Hide();
	}
	
	public class ExerciseRootMediator : Mediator<IExerciseFade>
	{
		[Inject] public OnShowExercise onShowExercise;

		protected override void Register()
		{
			base.Register();
			
			signals.Register(onShowExercise, OnShow);
		}

		private async void OnShow()
		{
			await mediated.Show();
		}
	}
}