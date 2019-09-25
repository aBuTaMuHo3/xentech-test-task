using System;

namespace MinLibs.Signals
{
	public partial interface ISignalVerifiers
	{
		void Add<T0, T1, T2> (Signal<T0, T1, T2> signal, int expTimesCalled = 1, Func<T0, T1, T2, bool> condition = null);
	}

	public partial class SignalVerifiers
	{
		public void Add<T0, T1, T2> (Signal<T0, T1, T2> signal, int expTimesCalled = 1, Func<T0, T1, T2, bool> condition = null)
		{
			var verifier = new SignalVerifier<T0, T1, T2>(signal, expTimesCalled, condition);
			Add(verifier);
		}
	}

	public class SignalVerifier<T0, T1, T2> : ASignalVerifier
	{
		readonly Func<T0, T1, T2, bool> defaultCondition = (t0, t1, t2) => true;

		public SignalVerifier (Signal<T0, T1, T2> signal, int expTimesCalled, Func<T0, T1, T2, bool> condition = null) : base(expTimesCalled)
		{
			StoreInfo(signal);
			condition = condition ?? defaultCondition;
			signal.AddListener((t0, t1, t2) => CheckCondition(condition(t0, t1, t2)));
		}
	}
}