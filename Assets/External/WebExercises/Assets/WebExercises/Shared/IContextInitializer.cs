using MinLibs.MVC;

namespace WebExercises.Shared
{
	public interface IContextInitializer
	{
		IContext InitContext(string id, IContext parent);
	}
}