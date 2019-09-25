using NSubstitute;
using NUnit.Framework;
using MinLibs.Signals;

namespace MinLibs.MVC.TestUtils
{
	public class MinMVCTests<TSubject>
	{
		protected IContext context;
		protected SignalVerifiers verifiers;
		protected TSubject subject;

		[SetUp]
		public virtual void Setup ()
		{
			verifiers = new SignalVerifiers();
			context = new MockingContext();
			SetUpContext();

			subject = context.Get<TSubject>();
			context.Inject(this);
			SetUpSubject();
		}

		protected virtual void SetUpContext ()
		{
		}

		protected virtual void SetUpSubject ()
		{
		}

		[TearDown]
		public virtual void TearDown ()
		{
			verifiers.Verify();
		}

		protected static void InitProperty<T> (T property) where T : class, new()
		{
			property.Returns(new T());
		}
	}
}