using MinLibs.Signals;

namespace MinLibs.MVC
{
	public abstract class Mediator<T> : IMediator where T : class, IMediated
	{
		protected T mediated;

		protected readonly ISignalsManager signals = new SignalsManager();

		public void Init (IMediated med)
		{
			mediated = (T)med;

			Register();
		}

		protected virtual void Register ()
		{
			signals.Register(mediated.OnRemove, OnRemove);
		}

		protected virtual void OnRemove ()
		{
			Unregister();
			signals.RemoveAll();
		}

		protected virtual void Unregister ()
		{

		}
	}
}
