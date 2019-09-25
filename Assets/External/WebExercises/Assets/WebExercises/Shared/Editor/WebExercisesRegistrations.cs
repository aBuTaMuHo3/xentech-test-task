using MinLibs.MVC;
using MinLibs.Signals;

namespace WebExercises.Shared.Editor
{
	public abstract class WebExercisesRegistrations : BaseRegistrations
	{
		public override RegistrationInfo Create()
		{
			var info = CreateInfo();
			
			info.Add<ContextSignals>().As<ISignalsManager>();
			info.Add<Mediators>().As<IMediators>();

			info.Add<RootMediator>().With(RegisterFlags.NoCache);
			
			return info;
		}
	}
}