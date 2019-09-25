using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebExercises.Shared.Components;

namespace WebExercises.Memoflow
{
	[RequireComponent(typeof(CanvasGroup))]
	public class MemoflowItem : MonoBehaviour
	{
		[SerializeField] private TMP_Text label;
		[SerializeField] private Image blocker;
		[SerializeField] private Indicator indicator;
		[SerializeField] private CanvasGroup group;
		[SerializeField] private float duration;

		private RectTransform _rect;

		private RectTransform Rect
		{
			get
			{
				_rect = _rect ?? (RectTransform) transform;
				return _rect;
			}
		}

		public float X
		{
			private get { return transform.localPosition.x; }
			set
			{
				var pos = transform.localPosition;
				pos.x = value;
				transform.localPosition = pos;
			}
		}

		public float Width => Rect.rect.width;

		public void SetIndicator(bool isCorrect)
		{
			indicator.IsCorrect = isCorrect;
		}

		public void SetLabel(string text)
		{
			label.gameObject.SetActive(true);
			label.text = text;
		}

		public Tweener Appear()
		{
			return Fade(0f, 1f, duration);
		}

		public Tweener Disappear()
		{
			return group.DOFade(0f, duration);
		}

		public Sequence CoverIcon(bool cover)
		{
			var seq = DOTween.Sequence();
			seq.Join(blocker.DOFade(cover ? 1 : 0, duration));
			seq.Join(label.DOFade(cover ? 0 : 1, duration));

			return seq;
		}

		public Tweener MoveBy(float distance)
		{
			return transform.DOLocalMoveX(X + distance, duration);
		}

		public Tweener MoveTo(float target)
		{
			return transform.DOLocalMoveX(target, duration);
		}

		private Tweener Fade(float from, float to, float duration)
		{
			group.alpha = from;
			return FadeTo(to, duration);
		}

		private Tweener FadeTo(float alpha, float duration)
		{
			return group.DOFade(alpha, duration);
		}

		public void CleanUp()
		{
			label.DOFade(1f, 0f);
			blocker.DOFade(0f, 0f);
			indicator.Reset();
			gameObject.SetActive(false);
		}

		public void HideLabel()
		{
			label.gameObject.SetActive(false);
		}
	}
}