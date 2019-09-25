using MinLibs.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace WebExercises.Shared.Components
{
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(Button))]
	public class LabelButton : InfoLabel
	{
		[SerializeField] private float alphaHitThreshold;
		[SerializeField] private Button button;
		
		public Signal OnClick { get; } = new Signal();

		protected virtual void Awake()
		{
			var nav = button.navigation;
			nav.mode = Navigation.Mode.None;
			button.navigation = nav;
			button.image.alphaHitTestMinimumThreshold = alphaHitThreshold;
			button.onClick.AddListener(() => OnClick.Dispatch());
		}

		public Color BackgroundColor
		{
			set => button.image.color = value;
		}
	}
}