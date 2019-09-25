using UnityEngine;
using UnityEngine.UI;

namespace WebExercises.Shared.Components
{
	[RequireComponent(typeof(Image))]
	public class Indicator : MonoBehaviour
	{
		[SerializeField] private Image image;
		[SerializeField] private Sprite correct;
		[SerializeField] private Sprite wrong;
		
		public bool IsCorrect
		{
			set
			{
				gameObject.SetActive(true);
				image.sprite = value ? correct : wrong;
			}
		}

		public Vector2 Offset
		{
			set
			{
				var rect = (RectTransform) transform;
				rect.anchoredPosition = value;
			}
		}

		public void Reset()
		{
			gameObject.SetActive(false);
		}
	}
}