using UnityEngine;

namespace MinLibs.Utils
{
	public interface IResources
	{
		T Load<T> (string path) where T: Object;

		T LoadComponent<T> (string path);

		T[] LoadComponents<T> (string path, bool recursive = false);

		AsyncOperation UnloadUnusedAssets ();

		ResourceRequest LoadAsync<T> (string path) where T : Object;
	}

	public class UnityResources : IResources
	{
		public T Load<T> (string path) where T: Object
		{
			return Resources.Load<T>(path);
		}

		public T LoadComponent<T> (string path)
		{
			var go = Load<GameObject>(path);

			return go.GetComponent<T>();
		}

		public T[] LoadComponents<T> (string path, bool recursive = false)
		{
			var go = Load<GameObject>(path);

			return recursive ? go.GetComponentsInChildren<T>() : go.GetComponents<T>();
		}

		public ResourceRequest LoadAsync<T> (string path) where T : Object
		{
			return Resources.LoadAsync<T>(path);
		}

		public AsyncOperation UnloadUnusedAssets ()
		{
			return Resources.UnloadUnusedAssets();
		}
	}
}