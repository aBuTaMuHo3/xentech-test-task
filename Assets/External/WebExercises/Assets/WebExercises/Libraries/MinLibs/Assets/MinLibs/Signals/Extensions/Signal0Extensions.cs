using System;

namespace MinLibs.Signals
{
	public static partial class SignalExtensions
	{
		public static void ManageListener (this Signal signal, Action callback, bool addListener)
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
