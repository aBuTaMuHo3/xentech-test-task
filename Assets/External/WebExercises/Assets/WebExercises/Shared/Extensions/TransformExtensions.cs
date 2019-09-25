using UnityEngine;

namespace WebExercises.Shared.Utils
{
	public static class TransformExtensions
	{
		public static void LookAt2D(this Transform origin, Vector3 target)
		{
			var relative = origin.InverseTransformPoint(target);
			var angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
			origin.rotation = Quaternion.Euler(0, 0, -angle);
		}
	}
}