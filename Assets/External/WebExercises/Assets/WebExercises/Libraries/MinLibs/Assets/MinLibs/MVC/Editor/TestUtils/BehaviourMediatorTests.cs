namespace MinLibs.MVC.TestUtils
{
	public class BehaviourMediatorTests<T, U> : MediatorTests<T, U>
		where T : IMediator where U : class, IMediatedBehaviour
	{
		protected override void SetUpView()
		{
			base.SetUpView();

			InitProperty(view.OnEnabled);
			InitProperty(view.OnDisabled);
		}
	}
}