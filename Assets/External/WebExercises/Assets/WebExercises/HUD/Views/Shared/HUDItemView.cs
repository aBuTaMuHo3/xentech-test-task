using MinLibs.MVC;
using TMPro;
using UnityEngine;

namespace WebExercises.HUD
{
	public class HUDItemView : MediatedBehaviour
	{
		[SerializeField] private TMP_Text header;

		public virtual void Init(string headerText)
		{
			header.text = headerText;
		}
	}
}