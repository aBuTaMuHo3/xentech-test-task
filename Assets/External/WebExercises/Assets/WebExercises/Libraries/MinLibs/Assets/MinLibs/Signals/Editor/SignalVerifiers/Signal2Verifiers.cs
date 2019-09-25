using System;

namespace MinLibs.Signals
{
	public partial interface ISignalVerifiers
	{
		void Add<T0, T1> (Signal<T0, T1> signal, int expTimesCalled = 1, Func<T0, T1, bool> condition = null);
	}

	public partial class SignalVerifiers
	{
		public void Add<T0, T1> (Signal<T0, T1> signal, int expTimesCalled = 1, Func<T0, T1, bool> condition = null)
		{
			var verifier = new SignalVerifier<T0, T1>(signal, expTimesCalled, condition);
			Add(verifier);
		}
	}

	public class SignalVerifier<T0, T1> : ASignalVerifier
	{
		readonly Func<T0, T1, bool> defaultCondition = (t0, t1) => true;

		public SignalVerifier (Signal<T0, T1> signal, int expTimesCalled, Func<T0, T1, bool> condition = null) : base(expTimesCalled)
		{
			StoreInfo(signal);
			condition = condition ?? defaultCondition;
			signal.AddListener((t0, t1) => CheckCondition(condition(t0, t1)));
		}
	}
}