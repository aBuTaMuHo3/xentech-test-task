using DG.Tweening;
using UnityEngine;
using WebExercises.Shared.Utils;

namespace WebExercises.Exercise
{
	public class PathLine : PathObject
	{
		[SerializeField] private RectTransform rect;

		public float Size
		{
			set
			{
				var size = rect.sizeDelta;
				size.y = value;
				rect.sizeDelta = size;
			}
		}

		public void Init(Vector3 pos, Vector3 target)
		{
			Pos = pos;
			transform.LookAt2D(target);
			transform.SetAsFirstSibling();
			gameObject.SetActive(true);
		}

		public Tween Grow(float length, int speed)
		{
			var to = rect.sizeDelta;
			to.y = length;
			var duration = speed / 1000f;
			
			return rect.DOSizeDelta(to, duration);
		}

		public void Flip()
		{
			transform.Rotate(0f, 0f, 180f);
		}

		public override void Reset()
		{
			Destroy(gameObject);
		}
	}
}