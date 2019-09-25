using UnityEngine;

namespace MinLibs.Utils
{
	public class FPSDisplay : MonoBehaviour
	{
		private float deltaTime;
		private Rect rect;
		private GUIStyle style;

		private void Start()
		{
			var w = Screen.width;
			var h = Screen.height;
			style = new GUIStyle();

			rect = new Rect(0, 0, w, h * 2 / 100);
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize =  2 *(h / 100);
			style.normal.textColor = Color.white;
		}

		private void Update()
		{
			deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		}

		private void OnGUI()
		{
			var msec = deltaTime * 1000.0f;
			var fps = 1.0f / deltaTime;
			var text = $"{msec:0.0} ms ({fps:0.} fps)";
			GUI.Label(rect, text, style);
		}
	}
}