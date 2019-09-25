using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Signals;
using UnityEngine;
using WebExercises.Shared.Components;

namespace WebExercises.Shared.Components
{
	public class FullscreenToggleView : MediatedBehaviour, IFullscreenToggle
	{
        public ILogging Log { get;  set; }
        [SerializeField] private ToggleButton fullScreenSwitch;
		
		public Signal<bool> OnToggleFullscreen { get; } = new Signal<bool>();

		protected void Awake()
		{
			fullScreenSwitch.OnToggle.AddListener(OnToggleFullscreen.Dispatch);
			
			base.Awake();
		}
		
		public void SetFullscreenState(bool isOn)
		{
			Log.Trace("set fullscreen to " + isOn);
			fullScreenSwitch.ActOnDown = !isOn;
			fullScreenSwitch.IsOn = isOn;
		}
	}
}
