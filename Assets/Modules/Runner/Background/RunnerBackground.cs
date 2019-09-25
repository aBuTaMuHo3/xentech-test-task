using UnityEngine;

namespace WebExercises.Runner
{
	public class RunnerBackground : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}
