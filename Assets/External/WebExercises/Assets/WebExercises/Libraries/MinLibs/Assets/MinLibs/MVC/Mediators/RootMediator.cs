using MinLibs.Signals;

namespace MinLibs.MVC
{
	public class RootMediator : Mediator<IMediating>
	{
		[Inject] public IMediators mediators;

		protected override void Register ()
		{
			var mediateSignal = new Signal<IMediated>();
			signals.Register(mediateSignal, mediators.Mediate);
			mediated.OnMediate = mediateSignal;
		}
	}
}
