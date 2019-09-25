using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MinLibs.MVC;
using MinLibs.Signals;
using WebExercises.Runner;

namespace WebExercises.HUD
{
	public interface IDialogue : IMediatedBehaviour
	{
		Signal<int> OnClick { get; }
		string Text { set; }
		float BackgroundAlpha { set; }
		void Cleanup();
		void AddButton(string label, int index);
		Task Show();
		Task Hide();
	}

	public class DialogueMediator : Mediator<IDialogue>
	{
		[Inject] public OnShowDialogue onShowDialogue;
		[Inject] public OnHideDialogue onHideDialogue;

		private readonly Stack<DialogueData> dataStack = new Stack<DialogueData>();
		
		protected override void Register()
		{
			base.Register();
			
			signals.Register(onShowDialogue, Show);
			signals.Register(onHideDialogue, Hide);
			signals.Register(mediated.OnClick, Close);
		}

		private void Hide()
		{
			Close(-1);
		}

		private void Show(DialogueData data)
		{
			dataStack.Push(data);
			Render(data);
		}

		private void Render(DialogueData data)
		{
			mediated.Cleanup();
			mediated.Text = data.Message;
			mediated.BackgroundAlpha = data.BackgroundAlpha;

			var index = 0;
			
			foreach (var actionData in data.Actions)
			{
				mediated.AddButton(actionData.Label, index);
				index++;
			}
			
			mediated.Show();
		}

		private async void Close(int index)
		{
			await mediated.Hide();

			if (dataStack.Count > 0)
			{
				var data = dataStack.Pop();

				if (index >= 0)
				{
					data.Actions[index].Handler(true);
				}

				if (dataStack.Count > 0)
				{
					Render(dataStack.Peek());
				}
			}
		}
	}

	public class DialogueData
	{
		public string Message { get; set; }
		public float BackgroundAlpha { get; set; }
		public IList<DialogueAction> Actions { get; } = new List<DialogueAction>();

		public void AddAction(string label, Action<bool> handler)
		{
			var action = new DialogueAction(label, handler);
			Actions.Add(action);
		}
	}
	
	public class DialogueAction
	{
		public string Label { get; }
		public Action<bool> Handler { get; }
		
		public DialogueAction(string label, Action<bool> handler)
		{
			Label = label;
			Handler = handler;
		}
	}
}