using MinLibs.MVC;
using MinLibs.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace WebExercises.HUD
{
	public class HUDRootView : MediatedBehaviour, IHUDRoot
	{
		[SerializeField] private Button pauseButton;
		
		public Signal<bool> OnToggleFullscreen { get; } = new Signal<bool>();
		public Signal OnPauseApp { get; } = new Signal();

		protected void Awake()
		{
			base.Awake();
			
			pauseButton.onClick.AddListener(OnPauseApp.Dispatch);
		}
	}
}
