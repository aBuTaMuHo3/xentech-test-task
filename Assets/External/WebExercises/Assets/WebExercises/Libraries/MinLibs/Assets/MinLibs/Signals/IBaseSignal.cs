namespace MinLibs.Signals
{
	/// <summary>
	/// Interface for methods that are shared by all templated signal classes
	/// </summary>
	public interface IBaseSignal
	{
		bool HasListeners ();

		void RemoveAllListeners ();
	}
}
