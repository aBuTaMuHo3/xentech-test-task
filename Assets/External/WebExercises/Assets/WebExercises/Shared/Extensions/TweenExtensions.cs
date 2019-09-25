using System.Threading.Tasks;
using DG.Tweening;

namespace WebExercises.Shared.Utils
{
	public static class TweenExtensions
	{
		public static async Task<T> IsCompleting<T>(this T tween) where T : Tween {
			var completionSource = new TaskCompletionSource<T>();
			tween.OnComplete(() => completionSource.SetResult(tween));
			return await completionSource.Task;
		}
	}
}