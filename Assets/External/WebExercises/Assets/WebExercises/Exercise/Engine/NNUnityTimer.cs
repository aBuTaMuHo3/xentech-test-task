using SynaptikonFramework.Interfaces.Util.Timer;
using UnityEngine;

namespace Exercises.ExerciseA
{
    
	public class NNUnityTimer : MonoBehaviour, ITimer
    {
        float _elapsedTime = 0f;
        double _tickCount;

        private void Update()
        {
            if(IsRunning && !IsPaused)
            {
                _elapsedTime += (Time.deltaTime*1000);
                if(_elapsedTime > IntervalTime)
                {
                    _tickCount++;
                    OnComplete?.Invoke(new TimerUpdateVO(_tickCount * IntervalTime));
                    _elapsedTime = 0f;
                }
            }
        }

        public double TimePassed
        {
            get
            {
                if(IsRunning)
                {
                    return IntervalTime * _tickCount + _elapsedTime;
                }
                return 0;
            }
        }

        public double IntervalTime
        {
            get;
            private set;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        public bool IsPaused { get; private set; }

        public event TimerUpdateHandler OnComplete;

        void ITimer.Start(double duration, bool repeat)
        {
            if (IsRunning)
            {
                _elapsedTime = 0f;
            }

            IsPaused = false;

            IsRunning = true;
            _tickCount = 0;
            IntervalTime = duration;
        }

        void ITimer.Stop()
        {
            if (IsRunning)
            {
                _elapsedTime = 0f;
            }

            IsRunning = false;
            IsPaused = false;
            _tickCount = 0;
        }

        void ITimer.Pause()
        {
            if (IsRunning)
            {
                IsPaused = true;
            }
        }

        void ITimer.Resume()
        {
            if (IsRunning && IsPaused)
            {
                IsPaused = false;
            }
        }

        public void Dispose()
        {
            if (IsRunning)
            {
                IsRunning = false;

            }
        }

        private class TimerUpdateVO : ITimerUpdateVO
        {
            public double TimePassed { get; }

            public TimerUpdateVO(double timePassed)
            {
                TimePassed = timePassed;
            }
        }
    }
}