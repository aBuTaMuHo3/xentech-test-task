using TMPro;
using UnityEngine;

namespace WebExercises.HUD
{
	public class HUDLevelView : HUDItemView, IHUDLevel
	{
		[SerializeField] private TMP_Text currentLabel;
		[SerializeField] private TMP_Text maxLabel;
		[SerializeField] private HUDItemScale scale;

		public void SetLevel(string current, string max)
		{
			currentLabel.text = current;
			maxLabel.text = max;
		}

		public void SetRuns(float percentage)
		{
			scale.SetScale(percentage);
		}
	}
}