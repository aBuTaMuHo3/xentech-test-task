using UnityEngine;

namespace WebExercises.Shared.Components
{
	public class SpriteLibrary : MonoBehaviour
	{
		[SerializeField] private SpriteEntry[] sprites;

		public Sprite GetSprite(string indicatorName)
		{
			foreach (var spriteEntry in sprites)
			{
				if (indicatorName == spriteEntry.Name)
				{
					return spriteEntry.Sprite;
				}
			}

			return null;
		}
	}
}