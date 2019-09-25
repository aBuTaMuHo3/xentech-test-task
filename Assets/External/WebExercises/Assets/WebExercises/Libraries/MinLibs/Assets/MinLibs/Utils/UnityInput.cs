using UnityEngine;

namespace MinLibs.Utils
{
	public class UnityInput : IInput
	{
		public bool IsDown(string key)
		{
			return Input.GetKeyDown(key);
		}
	}
}