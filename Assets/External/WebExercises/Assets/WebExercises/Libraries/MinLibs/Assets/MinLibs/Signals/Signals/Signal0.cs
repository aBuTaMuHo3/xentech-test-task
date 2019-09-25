using System;
using System.Collections.Generic;

namespace MinLibs.Signals
{
	public class Signal : IBaseSignal
	{
		Action fastInvocationListener;
		List<Action> listeners;

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

		public void Dispatch ()
		{
			executionCount++;

			var countBeforeExecution = (listeners != null) ? listeners.Count : 0;

			if (fastInvocationListener != null) {
				fastInvocationListener();
			}

			if (listeners != null) {
				for (var i = 0; i < countBeforeExecution; i++) {
					var listener = listeners[i];
					if (listener != null) {
						listener();
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

		public void AddListener (Action callback)
		{
			if (fastInvocationListener == null) {
				fastInvocationListener = callback;
				return;
			}

			if (fastInvocationListener == callback) {
				return;
			}

			if (listeners == null) {
				listeners = new List<Action>();
			}

			if (!listeners.Contains(callback)) {
				listeners.Add(callback);
			}
		}

		public void RemoveListener (Action callback)
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
