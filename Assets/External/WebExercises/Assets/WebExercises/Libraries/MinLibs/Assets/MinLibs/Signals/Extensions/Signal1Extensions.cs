using System;

namespace MinLibs.Signals
{
	public static partial class SignalExtensions
	{
		public static void ManageListener<T0> (this Signal<T0> signal, Action<T0> callback, bool addListener)
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
