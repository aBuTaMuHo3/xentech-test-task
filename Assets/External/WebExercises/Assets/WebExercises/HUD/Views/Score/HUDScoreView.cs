using TMPro;
using UnityEngine;

namespace WebExercises.HUD
{
	public class HUDScoreView : HUDItemView, IHUDScore
	{
		[SerializeField] private TMP_Text scoreLabel;
		
		public void SetScore(string score)
		{
			scoreLabel.text = score;
		}
	}
}