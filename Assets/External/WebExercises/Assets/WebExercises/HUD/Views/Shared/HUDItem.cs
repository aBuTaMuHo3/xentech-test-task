using MinLibs.MVC;
using WebExercises.Shared;

namespace WebExercises.HUD
{
	public interface IHUDItem : IMediated
	{
		void Init(string headerText);
	}

	public abstract class HUDItemMediator<T> : Mediator<T> where T: class, IHUDItem
	{
		[Inject] public ILocalization localization;
		[Inject] public IExerciseControllerProxy controller;

		protected override void Register()
		{
			base.Register();
			
			signals.Register(controller.OnInitLevelHUD, UpdateHUD);
		}

		protected virtual void UpdateHUD(HUDLevelInfo info)
		{
			
		}

		protected void InitView(string textKey)
		{
			mediated.Init(localization.Get(textKey));
		}
	}
}