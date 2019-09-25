using System;
using ExerciseEngine.View.Interfaces;
using SynaptikonFramework.Util.Math;

namespace WebExercises.Shared
{
	public class BackgroundViewAdapter : IBackgroundViewAdapter
	{
		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public void LevelUp(Action callback = null)
		{
			throw new NotImplementedException();
		}

		public void LevelDown(Action callback = null)
		{
			throw new NotImplementedException();
		}

		public void ShowCorrectIndicator()
		{
			throw new NotImplementedException();
		}

		public void ShowWrongIndicator()
		{
			throw new NotImplementedException();
		}

		public void ShowTimeoutIndicator()
		{
			throw new NotImplementedException();
		}

		public void ShowCorrectAnswer(Action callback = null)
		{
			throw new NotImplementedException();
		}

		public void ShowCorrectAnswer(float duration, Action callback = null)
		{
			throw new NotImplementedException();
		}

		public void ShowWrongAnswer(Action callback = null)
		{
			throw new NotImplementedException();
		}

		public void ShowWrongAnswer(float duration, Action callback = null)
		{
			throw new NotImplementedException();
		}

		public void SetMultiplier(int mulitiplierLevel)
		{
			throw new NotImplementedException();
		}

		public void Tap(Vector2D point)
		{
			throw new NotImplementedException();
		}

		public void EnableBackground(bool enabled)
		{
			throw new NotImplementedException();
		}

		public void ToggleGradient()
		{
			throw new NotImplementedException();
		}
	}

	public interface IBackgroundViewAdapter : IExerciseBackgroundView
	{
	}
}