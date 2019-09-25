using MinLibs.MVC;
using MinLibs.Signals;
using UnityEngine;
using WebExercises.Shared.Components;

namespace WebExercises.Memoflow
{
	public class MemoflowUIView : MediatedBehaviour, IMemoflowUI
	{
		[SerializeField] private LabelButton okButton;
		[SerializeField] private LabelButton yesButton;
		[SerializeField] private LabelButton noButton;
		[SerializeField] private InfoLabel infoLabel;

		public Signal OnOk { get; } = new Signal();
		public Signal OnYes { get; } = new Signal();
		public Signal OnNo { get; } = new Signal();

		public override void HandleMediation()
		{
			base.HandleMediation();

			okButton.OnClick.AddListener(() => OnOk.Dispatch());
			yesButton.OnClick.AddListener(() => OnYes.Dispatch());
			noButton.OnClick.AddListener(() => OnNo.Dispatch());
		}

		public void ShowIntro(string infoText, string okText)
		{
			infoLabel.Text = infoText;
			HideButton(yesButton);
			HideButton(noButton);
			ShowButton(okButton, okText);
		}

		public void ShowRound(string infoText, string yesText, string noText)
		{
			infoLabel.Text = infoText;
			HideButton(okButton);
			ShowButton(yesButton, yesText);
			ShowButton(noButton, noText);
		}

		private static void ShowButton(InfoLabel button, string text)
		{
			button.gameObject.SetActive(true);
			button.Text = text;
		}

		private static void HideButton(Component button)
		{
			button.gameObject.SetActive(false);
		}
	}
}