using TMPro;
using UnityEngine;

namespace WebExercises.HUD
{
	public class HUDTimeView : HUDItemView, IHUDTime
	{
		[SerializeField] private TMP_Text timeLabel;

		public void SetTime(string time)
		{
			timeLabel.gameObject.SetActive(true);
			timeLabel.text = time;
		}
	}
}