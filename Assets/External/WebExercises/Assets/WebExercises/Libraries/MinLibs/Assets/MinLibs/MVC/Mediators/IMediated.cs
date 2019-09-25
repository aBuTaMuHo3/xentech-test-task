using MinLibs.Signals;

namespace MinLibs.MVC
{
	public interface IMediated
	{
		Signal OnRemove { get; }

		void HandleMediation ();

		void Remove ();
	}
}
