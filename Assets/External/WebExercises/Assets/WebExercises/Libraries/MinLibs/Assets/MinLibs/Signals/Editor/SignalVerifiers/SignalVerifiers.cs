using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MinLibs.Signals
{
	public partial interface ISignalVerifiers
	{
		void Add (ISignalVerifier verifier);
		void Verify ();
	}

	public partial class SignalVerifiers : ISignalVerifiers
	{
		readonly HashSet<ISignalVerifier> verifiers = new HashSet<ISignalVerifier>();

		public void Add (ISignalVerifier verifier)
		{
			verifiers.Add(verifier);
		}

		public void Verify ()
		{
			foreach (var verifier in verifiers) {
				verifier.Verify();
			}
		}
	}

	public interface ISignalVerifier
	{
		void Verify ();
	}

	public abstract class ASignalVerifier : ISignalVerifier
	{
		readonly int expectedTimesCalled;
		int timesCalled;
		int timesCorrectlyCalled;
		protected Type type;

		protected ASignalVerifier (int expTimesCalled)
		{
			expectedTimesCalled = expTimesCalled;
		}

		public void Verify ()
		{
			Assert.AreEqual(expectedTimesCalled, timesCalled, type.Name + " was not called the expected amount of times");
			Assert.AreEqual(timesCalled, timesCorrectlyCalled, type.Name + " was not always called successfully");
		}

		protected void StoreInfo (IBaseSignal signal)
		{
			type = signal.GetType();
		}
		protected void CheckCondition (bool condition)
		{
			timesCalled++;

			if (condition) {
				timesCorrectlyCalled++;
			}
		}
	}
}