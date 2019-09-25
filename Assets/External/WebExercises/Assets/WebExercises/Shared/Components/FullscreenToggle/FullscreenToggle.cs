using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Signals;
using MinLibs.Utils;
using UnityEngine;

namespace WebExercises.Shared.Components
{
	public interface IFullscreenToggle : IMediatedBehaviour
	{
		void SetFullscreenState(bool isOn);
		Signal<bool> OnToggleFullscreen { get; }
        ILogging Log { get; set; }
    }

	public class FullscreenToggleMediator : Mediator<IFullscreenToggle>
	{
		[Inject] public IScreen screen;
        [Inject] public ILogging log;

        protected override void Register()
		{
			base.Register();

            mediated.Log = log;
			signals.Register(mediated.OnToggleFullscreen, ToggleFullscreen);
			signals.Register(screen.OnFullscreen, OnFullscreen);
			
			OnFullscreen(screen.IsFullscreen);
		}

		private void OnFullscreen(bool isFullscreen)
		{
			mediated.SetFullscreenState(isFullscreen);
		}

		private void ToggleFullscreen(bool isFullscreen)
		{
			screen.ToggleFullscreen();
		}
	}
}
