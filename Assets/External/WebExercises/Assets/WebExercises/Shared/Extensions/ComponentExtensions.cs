using UnityEngine;

namespace WebExercises.Shared.Utils
{
	public static class ComponentExtensions
	{
		public static T EnsureComponent<T>(this Component component) where T: Component
		{
			return component.GetComponent<T>() ?? component.gameObject.AddComponent<T>();
		}
	}
}