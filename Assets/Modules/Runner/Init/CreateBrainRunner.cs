using MinLibs.Logging;
using MinLibs.MVC;

namespace WebExercises.Runner
{
	public class CreateBrainRunner : CreateRunner<ApplyRunnerRegistrations>
	{
		protected override void ExecuteRegistrations(IApplyRegistrations registrations)
		{
			var logging = new Logging(new UnityTarget());
			registrations.Execute(logging);
		}
	}
}