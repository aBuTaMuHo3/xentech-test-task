using TMPro;
using UnityEngine;

namespace WebExercises.Shared.Components
{
	public class InfoLabel : MonoBehaviour
	{
		[SerializeField] protected TMP_Text label;
		
		public string Text
		{
			get { return label.text; }
			set { label.text = value; }
		}

		public bool IsVisible
		{
			set
			{
				gameObject.SetActive(value);
			}
		}
	}
}