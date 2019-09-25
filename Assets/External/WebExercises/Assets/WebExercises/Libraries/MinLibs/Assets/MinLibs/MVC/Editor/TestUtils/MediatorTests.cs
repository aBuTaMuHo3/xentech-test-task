using NSubstitute;

namespace MinLibs.MVC.TestUtils
{
	public class MediatorTests<T, U> : MinMVCTests<T>
		where T : IMediator where U : class, IMediated
	{
		protected U view;

		public override void Setup()
		{
			base.Setup();

			view = Substitute.For<U>();
			SetUpView();

			subject.Init(view);
			view.ClearReceivedCalls();
		}

		protected virtual void SetUpView()
		{
			InitProperty(view.OnRemove);
		}
	}
}