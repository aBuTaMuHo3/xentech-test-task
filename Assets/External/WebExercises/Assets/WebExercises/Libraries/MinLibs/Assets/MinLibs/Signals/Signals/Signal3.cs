using System;
using System.Collections.Generic;

namespace MinLibs.Signals
{
	public class Signal<T0, T1, T2> : IBaseSignal
	{
		Action<T0, T1, T2> fastInvocationListener;
		List<Action<T0, T1, T2>> listeners;

		int executionCount;
		bool didRemoveFromList;

		public bool HasListeners ()
		{
			if (fastInvocationListener != null) {
				return true;
			}

			if (listeners == null) {
				return false;
			}

			for (var i = 0; i < listeners.Count; i++) {
				if (listeners[i] != null) {
					return true;
				}
			}

			return false;
		}

		public void Dispatch (T0 t0, T1 t1, T2 t2)
		{
			executionCount++;

			var countBeforeExecution = (listeners != null) ? listeners.Count : 0;

			if (fastInvocationListener != null) {
				fastInvocationListener(t0, t1, t2);
			}

			if (listeners != null) {
				for (var i = 0; i < countBeforeExecution; i++) {
					var listener = listeners[i];
					if (listener != null) {
						listener(t0, t1, t2);
					}
				}
			}

			executionCount--;

			if (executionCount == 0 && didRemoveFromList) {
				if (listeners != null) {
					listeners.RemoveAll(c => c == null);
				}

				didRemoveFromList = false;
			}
		}

		public void AddListener (Action<T0, T1, T2> callback)
		{
			if (fastInvocationListener == null) {
				fastInvocationListener = callback;
				return;
			}

			if (fastInvocationListener == callback) {
				return;
			}

			if (listeners == null) {
				listeners = new List<Action<T0, T1, T2>>();
			}

			if (!listeners.Contains(callback)) {
				listeners.Add(callback);
			}
		}

		public void RemoveListener (Action<T0, T1, T2> callback)
		{
			if (fastInvocationListener == callback) {
				fastInvocationListener = null;
			}
			else if (listeners != null) {
				var index = listeners.IndexOf(callback);
				if (index != -1) {
					listeners[index] = null;
					didRemoveFromList = true;
				}
			}
		}

		public void RemoveAllListeners ()
		{
			fastInvocationListener = null;

			if (listeners != null) {
				for (var index = 0; index < listeners.Count; index++) {
					listeners[index] = null;
					didRemoveFromList = true;
				}
			}
		}
	}
}
