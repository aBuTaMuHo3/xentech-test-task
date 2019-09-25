using MinLibs.Signals;
using UnityEngine;

namespace MinLibs.Utils
{
	public interface IScreen
	{
		Signal<int, int> OnResize { get; }
		Signal<bool> OnFullscreen { get; }
		
		int Width { get; }
		int Height { get; }
		float AspectRatio { get; }
		bool IsFullscreen { get; set; }

		void ToggleFullscreen();
		void Update();
	}

	public class UnityScreen : IScreen
	{
		public Signal<int, int> OnResize { get; } = new Signal<int, int>();

		public Signal<bool> OnFullscreen { get; } = new Signal<bool>();

		private int screenWidth;
		private int screenHeight;
		private bool isFullscreen;

		public UnityScreen()
		{
			screenWidth = Width;
			screenHeight = Height;
			isFullscreen = IsFullscreen;
		}
		
		public int Width => Screen.width;
		public int Height => Screen.height;
		public float AspectRatio => Width / (float) Height;
		public bool IsFullscreen
		{
			get { return Screen.fullScreen; }
			set { Screen.fullScreen = value; }
		}

		public void ToggleFullscreen()
		{
			IsFullscreen = !IsFullscreen;
		}

		public void Update()
		{
			UpdateScreenSize();
			UpdateFullscreen();
		}

		private void UpdateFullscreen()
		{
			if (isFullscreen != IsFullscreen)
			{
				isFullscreen = IsFullscreen;
				OnFullscreen.Dispatch(isFullscreen);
			}
		}

		private void UpdateScreenSize()
		{
			if (screenWidth != Width || screenHeight != Height)
			{
				screenWidth = Width;
				screenHeight = Height;
				OnResize.Dispatch(screenWidth, screenHeight);
			}
		}
	}
}