using System;

namespace MinLibs.Signals
{
	public partial interface ISignalVerifiers
	{
		void Add (Signal signal, int expTimesCalled = 1, Func<bool> condition = null);
	}

	public partial class SignalVerifiers
	{
		public void Add (Signal signal, int expTimesCalled = 1, Func<bool> condition = null)
		{
			var verifier = new SignalVerifier(signal, expTimesCalled, condition);
			Add(verifier);
		}
	}

	public class SignalVerifier : ASignalVerifier
	{
		readonly Func<bool> defaultCondition = () => true;

		public SignalVerifier (Signal signal, int expTimesCalled, Func<bool> condition = null) : base(expTimesCalled)
		{
			StoreInfo(signal);
			condition = condition ?? defaultCondition;
			signal.AddListener(() => CheckCondition(condition()));
		}
	}
}