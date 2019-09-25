using MinLibs.Signals;

namespace MinLibs.MVC
{
	public class ContextSignals : SignalsManager
	{
		[Cleanup]
		public void Cleanup()
		{
			RemoveAll();
		}
	}
}
