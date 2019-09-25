using System;
using System.Collections.Generic;
using MinLibs.Utils;

namespace MinLibs.Signals
{
	public interface ISignalsManager
	{
		void Register (Signal signal, Action listener);
		void Register<U> (Signal<U> signal, Action<U> listener);
		void Register<U, V> (Signal<U, V> signal, Action<U, V> listener);
		void Register<U, V, W> (Signal<U, V, W> signal, Action<U, V, W> listener);
		void RemoveAll();
	}

	public class SignalsManager : ISignalsManager
	{
		readonly IList<Action> removeHandlers = new List<Action>();
		
		public void Register (Signal signal, Action listener)
		{
			signal.AddListener(listener);
			AddRemoveAction(() => signal.RemoveListener(listener));
		}

		public void Register<U> (Signal<U> signal, Action<U> listener)
		{
			signal.AddListener(listener);
			AddRemoveAction(() => signal.RemoveListener(listener));
		}

		public void Register<U, V> (Signal<U, V> signal, Action<U, V> listener)
		{
			signal.AddListener(listener);
			AddRemoveAction(() => signal.RemoveListener(listener));
		}

		public void Register<U, V, W> (Signal<U, V, W> signal, Action<U, V, W> listener)
		{
			signal.AddListener(listener);
			AddRemoveAction(() => signal.RemoveListener(listener));
		}

		public void RemoveAll ()
		{
			removeHandlers.Each(s => s());
			removeHandlers.Clear();
		}

		private void AddRemoveAction (Action action)
		{
			removeHandlers.Add(action);
		}
	}
}