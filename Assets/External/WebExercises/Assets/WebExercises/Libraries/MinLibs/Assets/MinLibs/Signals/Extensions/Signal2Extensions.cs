using System;

namespace MinLibs.Signals
{
	public static partial class SignalExtensions
	{
		public static void ManageListener<T0, T1> (this Signal<T0, T1> signal, Action<T0, T1> callback, bool addListener)
		{
			if (addListener) {
				signal.AddListener(callback);
			}
			else {
				signal.RemoveListener(callback);
			}
		}
	}
}
