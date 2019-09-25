using MinLibs.MVC;
using WebExercises.Shared;

namespace WebExercises.Runner
{
	public abstract class CreateRunner<T> : MediatingBehaviour where T: class, IApplyRegistrations, new()
	{
		private IContext context;

		private void Start()
		{
			context = new Context { Id = "Runner" };
			var registrations = context.Create<T>();
			ExecuteRegistrations(registrations);
			context.Get<ISetupContext>().Execute(this);
		}

		protected abstract void ExecuteRegistrations(IApplyRegistrations registrations);
	}
}