using UnityEngine;

namespace WebExercises.Runner
{
	[CreateAssetMenu(menuName = "Exercises/Create AppSettings", order = 0)]
	public class AppSettings : ScriptableObject
	{
		public string url;
		public string basePath;
	}
}