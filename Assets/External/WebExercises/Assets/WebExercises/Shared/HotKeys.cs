using System;
using System.Collections.Generic;
using MinLibs.MVC;
using MinLibs.Utils;

namespace WebExercises.Shared
{
	public interface IHotKeys
	{
		IDictionary<string, Action> KeyActions { get; set; }
		void OnCheck();
		void Add(string key, Action action);
		void Reset();
	}
	
	public class HotKeys : IHotKeys
	{
		public IDictionary<string, Action> KeyActions { get; set; } = new Dictionary<string, Action>();

		[Inject] public IInput input = new UnityInput();
		
		public void OnCheck()
		{
			foreach (var keyAction in KeyActions)
			{
				if (input.IsDown(keyAction.Key))
				{
					keyAction.Value();
				}
			}
		}

		public void Reset()
		{
			KeyActions.Clear();
		}

		public void Add(string key, Action action)
		{
			KeyActions[key] = action;
		}
	}
}