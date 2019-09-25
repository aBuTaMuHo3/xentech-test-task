using DG.Tweening;
using UnityEngine;

namespace WebExercises.Exercise
{
	public class PathObject : MonoBehaviour
	{
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private float fadeTime;
		
		public Vector3 Pos
		{
			get => transform.localPosition;

			set => transform.localPosition = value;
		}
		
		public Tween Fade(float target)
		{
			return canvasGroup.DOFade(target, fadeTime);
		}

		public virtual void Reset()
		{
			canvasGroup.alpha = 0f;
			gameObject.SetActive(false);
		}
	}
}