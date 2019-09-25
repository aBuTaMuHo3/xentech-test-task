using System;
using System.Collections.Generic;
using MinLibs.Signals;

namespace MinLibs.MVC
{
	public interface ITimers
	{
		ITimer Create();
		void Update(float deltaTime);
	}

	public class Timers : ITimers
	{
		private readonly IList<ITimer> timers = new List<ITimer>();
		
		public ITimer Create()
		{
			var timer = new Timer();
			timers.Add(timer);

			return timer;
		}

		public void Update(float deltaTime)
		{
			for (var i = timers.Count - 1; i >= 0; i--)
			{
				var timer = timers[i];
				timer.Update(deltaTime);

				if (timer.State > TimerStates.Running)
				{
					timers.RemoveAt(i);
				}
			}
		}
	}

	public interface ITimer
	{
		Signal<int, float> OnTick { get; }
		Signal OnFinish { get; }
		TimerStates State { get; }
		bool IsPaused { get; set; }
		int Count { get; }
		float Elapsed { get; }
		float Duration { get; }
		float Interval { get; }
		void Start(float duration, float interval);
		void Start(float duration);
		void Stop();
		void Update(float deltaTime);
	}

	public class Timer : ITimer
	{
		public Signal<int, float> OnTick { get; } = new Signal<int, float>();
		public Signal OnFinish { get; } = new Signal();
		
		public TimerStates State { get; private set; }

		public bool IsPaused
		{
			get { return State == TimerStates.Paused; }

			set
			{
				if (!value && State == TimerStates.Paused)
				{
					State = TimerStates.Running;
				}
				else if (value && State == TimerStates.Running)
				{
					State = TimerStates.Paused;
				}
			}
		}
		
		public int Count { get; private set; }
		public float Elapsed { get; private set; }
		public float Duration { get; private set; }
		public float Interval { get; private set; }

		private float lastInterval;

		public void Start(float duration, float interval)
		{
			Duration = duration;
			lastInterval = Interval = interval;
			State = TimerStates.Running;
		}

		public void Start(float duration)
		{
			Start(duration, duration);
		}

		public void Stop()
		{
			State = TimerStates.Cancelled;
		}

		public void Update(float deltaTime)
		{
			if (State == TimerStates.Running)
			{
				Elapsed += deltaTime;

				if (Elapsed >= lastInterval)
				{
					OnTick.Dispatch(Count, deltaTime);
					Count++;
					lastInterval += Interval;
				}

				if (Elapsed >= Duration)
				{
					State = TimerStates.Finished;
					OnFinish.Dispatch();
				}
			}
		}
	}

	[Flags]
	public enum TimerStates
	{
		None = 0,
		Running = 1,
		Paused = 2,
		Cancelled = 4,
		Finished = 8
	}
}