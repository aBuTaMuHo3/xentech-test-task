namespace MinLibs.MVC
{
	public abstract class BehaviourMediator<T> : Mediator<T> where T : class, IMediatedBehaviour
	{
		protected override void Register ()
		{
			base.Register();

			signals.Register(mediated.OnEnabled, OnEnabled);
			signals.Register(mediated.OnDisabled, OnDisabled);
		}

		protected virtual void OnEnabled ()
		{
		}

		protected virtual void OnDisabled ()
		{
		}
	}
}