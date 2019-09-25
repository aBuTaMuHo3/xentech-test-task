using System.Threading.Tasks;
using DG.Tweening;
using MinLibs.MVC;
using UnityEngine;
using WebExercises.Shared.Utils;

namespace WebExercises.Exercise
{
	[RequireComponent(typeof(CanvasGroup))]
	public class ExerciseFadeView : MediatedBehaviour, IExerciseFade
	{
		[SerializeField] private float fadeDuration = 0.3f;
		
		private CanvasGroup canvasGroup;

		protected override void Awake()
		{
			base.Awake();

			canvasGroup = this.EnsureComponent<CanvasGroup>();
			canvasGroup.alpha = 0;
		}

		public async Task Show()
		{
			await Fade(1);
		}
		
		public async Task Hide()
		{
			await Fade(0);
		}
		
		private Task Fade(float target)
		{
			return canvasGroup.DOFade(target, fadeDuration).IsCompleting();
		}
	}
}