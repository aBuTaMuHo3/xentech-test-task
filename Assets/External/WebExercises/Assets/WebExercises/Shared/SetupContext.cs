using MinLibs.MVC;
using MinLibs.Signals;

namespace WebExercises.Shared
{
	public interface ISetupContext
	{
		void Execute(IMediating view);
	}
	
	public abstract class SetupContext : ISetupContext
	{
		[Inject] public IMediators mediators;
		[Inject] public ISignalsManager signals;
		
		public virtual void Execute(IMediating view)
		{
			SetupSignals();
			SetupMediators();
			SetupMisc();
			mediators.Mediate(view);
		}

		protected virtual void SetupSignals()
		{
			
		}

		protected virtual void SetupMediators()
		{
			mediators.Map<IMediating, RootMediator>();
		}

		protected virtual void SetupMisc()
		{
			
		}
	}
}