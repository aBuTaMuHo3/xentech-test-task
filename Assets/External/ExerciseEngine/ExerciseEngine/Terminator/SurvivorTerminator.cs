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
    public class SurvivorTerminator : IExerciseTerminator
    {
        private bool _disposed = false;
        private readonly ILogger _logger;
        private readonly SurvivorTerminatorInitVO _initVO;
        public IHudInintVO HudInintVO => new SurvivorHudInitVO();
		public event TerminatorUpdateHandler OnTerminate;
        private bool _waitForShowWrong = false;

        public SurvivorTerminator(ILogger logger, SurvivorTerminatorInitVO initVO)
        {
            _initVO = initVO;
            _logger = logger;
        }

        public void HandelTermiantionTrigger(ITerminatorTrigger trigger)
        {
            if(trigger is TotalBadRunsUpdateVO && ((TotalBadRunsUpdateVO)trigger).CurrentValue >= _initVO.NumBadRuns){
                _waitForShowWrong = true;
            }
            else if (_waitForShowWrong && trigger is StateTerminationTrigger && ((StateTerminationTrigger)trigger).IsRoundFinished)
            {
                //Todo verify conditional logic
                _logger.LogMessage(LogLevel.Informational, "Terminate by SurvivorTerminator");
                OnTerminate?.Invoke(new SurvivorTerminationVO());
            }
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:ExerciseEngine.Xamarin.Terminator.SurvivorTerminator"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:ExerciseEngine.Xamarin.Terminator.SurvivorTerminator"/>. The <see cref="Dispose"/> method
        /// leaves the <see cref="T:ExerciseEngine.Xamarin.Terminator.SurvivorTerminator"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:ExerciseEngine.Xamarin.Terminator.SurvivorTerminator"/> so the garbage collector can reclaim
        /// the memory that the <see cref="T:ExerciseEngine.Xamarin.Terminator.SurvivorTerminator"/> was occupying.</remarks>
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
