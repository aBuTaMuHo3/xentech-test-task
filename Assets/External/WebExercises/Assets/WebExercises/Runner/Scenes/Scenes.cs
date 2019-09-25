using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace WebExercises.Runner
{
	public interface IScenes
	{
		Task<Scene> Load(string name, LoadSceneMode mode = LoadSceneMode.Additive, bool activate = true);
		Task UnloadAll();
	}

	public class Scenes : IScenes
	{
		private readonly HashSet<string> sceneNames = new HashSet<string>();
		
		public async Task<Scene> Load(string name, LoadSceneMode mode = LoadSceneMode.Additive, bool activate = true)
		{
			sceneNames.Add(name);
			
			var operation = SceneManager.LoadSceneAsync(name, mode);
			await Awaiters.Until(() => operation.isDone);

			var scene = SceneManager.GetSceneByName(name);

			if (activate)
			{
				SceneManager.SetActiveScene(scene);
			}

			return scene;
		}

		public async Task UnloadAll()
		{
			foreach (var name in sceneNames)
			{
				var scene = SceneManager.GetSceneByName(name);

				await SceneManager.UnloadSceneAsync(scene);
			}
			
			sceneNames.Clear();
		}
	}
}