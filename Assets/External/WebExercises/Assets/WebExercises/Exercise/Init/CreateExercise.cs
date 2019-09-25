using MinLibs.MVC;
using WebExercises.Shared;

namespace WebExercises.Exercise
{
	public abstract class CreateExercise<T> : MediatingBehaviour, IContextInitializer where T: class, IApplyRegistrations, new()
	{
		public IContext InitContext(string id, IContext parent)
		{
			var context = new Context { Id = id, Parent = parent };
			
			var registrations = context.Create<T>();
			registrations.Execute();
			
			var setup = context.Get<ISetupContext>();
			setup.Execute(this);

			return context;
		}
	}
}