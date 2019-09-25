using System;

namespace SynaptikonFramework.Interfaces.Util.Timer
{
    public delegate void TimerUpdateHandler(ITimerUpdateVO vo);

    public interface ITimer : IDisposable
    {
        /// <summary>
        /// Timer passed since the timer was started
        /// </summary>
        /// <value>Timer passed in milliseconds.</value>
        double TimePassed { get; }

        /// <summary>
        /// Gets the timer interval time.
        /// </summary>
        /// <value>The timer interval in milliseconds.</value>
        double IntervalTime { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:SynaptikonFramework.Interfaces.Util.IExerciseTimer"/> is running.
        /// </summary>
        /// <value><c>true</c> if is running; otherwise, <c>false</c>.</value>
        bool IsRunning { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:SynaptikonFramework.Interfaces.Util.IExerciseTimer"/> is paused.
        /// </summary>
        /// <value><c>true</c> if is paused; otherwise, <c>false</c>.</value>
        bool IsPaused { get; }

        /// <summary>
        /// Start the specified duration timer.
        /// </summary>
        /// <returns>The start.</returns>
        /// <param name="duration">Duration.</param>
        /// <param name="repeat">If set to <c>true</c> Timer repeats.</param>
        void Start(double duration, bool repeat = false);

        /// <summary>
        /// Pause this instance.
        /// </summary>
        void Pause();

        /// <summary>
        /// Resume this instance.
        /// </summary>
        void Resume();

        /// <summary>
        /// Stop this instance.
        /// </summary>
        void Stop();

        /// <summary>
        /// Occurs every time when interval completes.
        /// </summary>
        event TimerUpdateHandler OnComplete;

    }
}
