using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using MinLibs.MVC;
using MinLibs.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebExercises.Shared.Components;
using WebExercises.Shared.Utils;

namespace WebExercises.HUD
{
	public class DialogueView : MediatedBehaviour, IDialogue
	{
		[SerializeField] private TMP_Text text;
		[SerializeField] private RectTransform buttons;
		[SerializeField] private LabelButton buttonPrefab;
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private Image background;
		[SerializeField] private float fadeTime;
		
		private readonly Queue<LabelButton> cache = new Queue<LabelButton>();

		public Signal<int> OnClick { get; } = new Signal<int>();

		protected override void Awake()
		{
			base.Awake();

			gameObject.SetActive(false);
		}
		
		public async Task Show()
		{
			gameObject.SetActive(true);

			canvasGroup.alpha = 0f;
			await canvasGroup.DOFade(1, fadeTime).IsCompleting();
		}

		public async Task Hide()
		{
			canvasGroup.alpha = 1f;
			await canvasGroup.DOFade(0f, fadeTime).IsCompleting();

			Cleanup();
			
			gameObject.SetActive(false);
		}

		public void Cleanup()
		{
			var trans = buttons.transform;
			var count = trans.childCount;

			for (var i = 0; i < count; i++)
			{
				var child = trans.GetChild(i);
				child.SetParent(null);
				child.gameObject.SetActive(false);
				var button = child.GetComponent<LabelButton>();
				button.OnClick.RemoveAllListeners();
				cache.Enqueue(button);
			}
		}

		public string Text
		{
			set { text.text = value; }
		}

		public float BackgroundAlpha
		{
			set
			{
				if (value >= 0)
				{
					var c = background.color;
					c.a = value;
					background.color = c;
				}
			}
		}

		public void AddButton(string label, int index)
		{
			var button = GetButton();
			button.Text = label;
			button.OnClick.AddListener(() => OnClick.Dispatch(index));
		}

		private LabelButton GetButton()
		{
			if (cache.Count == 0)
			{
				return Instantiate(buttonPrefab, buttons);
			}

			var button = cache.Dequeue();
			button.transform.SetParent(buttons);
			button.gameObject.SetActive(true);

			return button;
		}
	}
}