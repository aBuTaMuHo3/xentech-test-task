using System;
using ExerciseEngine.Controller;
using ExerciseEngine.HUD.ValueObjects;
using ExerciseEngine.HUD.ValueObjects.Interfaces;
using ExerciseEngine.Terminator.Interfaces;
using ExerciseEngine.Terminator.Triggers;
using ExerciseEngine.Terminator.Triggers.Interfaces;
using ExerciseEngine.Terminator.ValueObjects;
using SynaptikonFramework.Interfaces.DebugLog;

namespace ExerciseEngine.Terminator
{
	public class TimeBasedTerminator : IExerciseTerminator
	{
		public event TerminatorUpdateHandler OnTerminate;
		public IHudInintVO HudInintVO => new TimeBasedHudInitVO(_duration * 1000);

		private readonly ILogger _logger;
		private readonly int _duration;
		private bool _timeIsUp = false;
		private TimeTerminationTrigger _lastTimeTerminationTrigger;

		public TimeBasedTerminator(ILogger logger, TimeBasedTerminatorInitVO initVO)
		{
			_logger = logger;
			_duration = initVO.ExerciseDuration;
		}

		public void HandelTermiantionTrigger(ITerminatorTrigger trigger)
		{
			//_logger.LogMessage( LogLevel.Informational, "HandelTermiantionTrigger "+ trigger.ToString());
			var timeTerminationTrigger = trigger as TimeTerminationTrigger;

			if (timeTerminationTrigger != null)
			{
				_lastTimeTerminationTrigger = timeTerminationTrigger;
				if (timeTerminationTrigger.TimerUpdateVO.TimePassed >= (_duration * 1000))
				{
					_logger.LogMessage(LogLevel.Informational,
						"Exercise time is up (" + timeTerminationTrigger.TimerUpdateVO.TimePassed + ")");
					_timeIsUp = true;
				}
			}
			else
			{
				var stateTerminationTrigger = trigger as StateTerminationTrigger;
				if (stateTerminationTrigger?.IsRoundFinished == true && _timeIsUp)
				{
					_logger.LogMessage(LogLevel.Informational, string.Concat("Exercise terminated"));
					OnTerminate?.Invoke(new TimerBasedTerminationVO(_lastTimeTerminationTrigger.TimerUpdateVO));
				}
			}
		}

		private bool _disposed = false;


		/// <summary>
		/// Releases all resource used by the <see cref="T:ExerciseEngine.Xamarin.Terminator.TimeBasedTerminator"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:ExerciseEngine.Xamarin.Terminator.TimeBasedTerminator"/>. The <see cref="Dispose"/> method
		/// leaves the <see cref="T:ExerciseEngine.Xamarin.Terminator.TimeBasedTerminator"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:ExerciseEngine.Xamarin.Terminator.TimeBasedTerminator"/> so the garbage collector can reclaim
		/// the memory that the <see cref="T:ExerciseEngine.Xamarin.Terminator.TimeBasedTerminator"/> was occupying.</remarks>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); // stop the GC clearing us up, 
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				// someone called Dispose()
				_logger.LogMessage(LogLevel.Informational, "Dispose called");
				_disposed = true;
			}
		}
	}
}