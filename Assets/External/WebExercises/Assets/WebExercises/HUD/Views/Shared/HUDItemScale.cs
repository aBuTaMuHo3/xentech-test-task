using System;
using UnityEngine;
using UnityEngine.UI;

namespace WebExercises.HUD
{
	public class HUDItemScale : MonoBehaviour
	{
		[SerializeField] private RectTransform background;
		[SerializeField] private Image scale;
		[SerializeField] private Color goodRuns;
		[SerializeField] private Color badRuns;

		public void SetScale(float percentage)
		{
			var width = background.sizeDelta.x;
			var value = Math.Abs(percentage);
			var size = width - width * value;

			if (percentage >= 0)
			{
				UpdateScale(goodRuns, 0, -size);
			}
			else
			{
				UpdateScale(badRuns, size, 0);
			}
		}

		public void UpdateScale(Color color, float min, float max)
		{
			scale.color = color;
			var rectTransform = scale.rectTransform;
			rectTransform.offsetMin = new Vector2(min, 0);
			rectTransform.offsetMax = new Vector2(max, 0);
		}
	}
}