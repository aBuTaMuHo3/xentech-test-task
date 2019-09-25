using System;
using System.Collections.Generic;
using ExerciseEngine.Terminator.Interfaces;
using ExerciseEngine.Terminator.Triggers;
using ExerciseEngine.View.Interfaces;
using SynaptikonFramework.Interfaces.DebugLog;

namespace ExerciseEngine.Controller
{
    public class StateMachine : IDisposable
    {
        protected readonly ILogger _logger;
        protected readonly IExerciseTerminator _terminator;
        protected readonly IExerciseView _view;

        /// <summary>
        /// Holding current active state.
        /// </summary>
        private int _state;

        /// <summary>
        /// Holds functions executed on state entered.
        /// </summary>
        private readonly Dictionary<int, System.Action> _enterStateMapping;

        /// <summary>
        /// Holds functions executed on state left.
        /// </summary>
        private readonly Dictionary<int, System.Action> _exitStateMapping;

        protected bool _disposed { get; private set; }

        public StateMachine(ILogger logger, IExerciseTerminator terminator, IExerciseView exerciseView)
        {
            _disposed = false;
            _terminator = terminator;
            _view = exerciseView;
            _logger = logger;
            _state = States.UNDEFINED;
            _enterStateMapping = new Dictionary<int, System.Action>();
            _exitStateMapping = new Dictionary<int, System.Action>();
        }

        /// <summary>
        /// Maps in and out functions for given state.
        /// </summary>
        /// <param name="state">State.</param>
        /// <param name="enterAction">Enter action.</param>
        /// <param name="exitAction">Exit action.</param>
        protected void MapState(int state, System.Action enterAction, System.Action exitAction)
        {
            if (_enterStateMapping.ContainsKey(state))
            {
                _enterStateMapping[state] = enterAction;
            }
            else
            {
                _enterStateMapping.Add(state, enterAction);
            }
            if (_enterStateMapping.ContainsKey(state))
            {
                _exitStateMapping[state] = exitAction;
            }
            else
            {
                _exitStateMapping.Add(state, exitAction);
            }
        }

        protected int CurrentState
        {
            set
            {
                // Don't proceed if there is no state change
                if (_state == value){
					return;
                } 

                int prevState = _state;
                _state = value;

                // If left state is not undefined, and mapping is there, execute exit state function
                if (prevState != States.UNDEFINED && ValidateStateMapping(_exitStateMapping, prevState)){
                    _view.RunOnGameThread(() => _exitStateMapping[prevState]());
                    _terminator.HandelTermiantionTrigger(new StateTerminationTrigger(prevState, true));
                } 

                // If mapping is there execute enter state function
                if (ValidateStateMapping(_enterStateMapping, _state)){
                    _view.RunOnGameThread(() => _enterStateMapping[_state]());
                    _terminator.HandelTermiantionTrigger(new StateTerminationTrigger(_state, false));
                }
            }

            get
            {
                return _state;
            }
        }

        /// <summary>
        /// Function validating states mapping.
        /// </summary>
        /// <returns><c>true</c>, if state mapping was mapped, <c>false</c> otherwise.</returns>
        /// <param name="stateMapping">State mapping. enter or exit states</param>
        /// <param name="state">State.</param>
        private bool ValidateStateMapping(Dictionary<int, System.Action> stateMapping, int state)
        {
            if (state == States.UNDEFINED)
            {
                // UNDEFINED state shouldnt be mapped
                _logger.LogMessage(LogLevel.Error, "Error: State " + state + " is undefined state an shouldn't be used");
                return false;
            }
            if (stateMapping.ContainsKey(state))
            {
                return true;
            }
            else
            {
                // There are no mapped funtion to given state
                _logger.LogMessage(LogLevel.Error, "Error: Function mapping for state " + state + " is missing");
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // stop the GC clearing us up, 

        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose#implementing-the-dispose-pattern-for-a-base-class
        /// Releases all resource used by the <see cref="T:ExerciseEngine.Controller.BaseExerciseController"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:ExerciseEngine.Controller.BaseExerciseController"/>. The <see cref="Dispose"/> method leaves
        /// the <see cref="T:ExerciseEngine.Controller.BaseExerciseController"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:ExerciseEngine.Controller.BaseExerciseController"/> so the garbage collector can reclaim the
        /// memory that the <see cref="T:ExerciseEngine.Controller.BaseExerciseController"/> was occupying.</remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Free any unmanaged objects here.
                _disposed = true;
                // Free any other managed objects here.
                _logger.LogMessage(LogLevel.Informational, "Dispose called");
                _enterStateMapping.Clear();
                _exitStateMapping.Clear();
            }
        }
    }

    public class States
    {
        public const int UNDEFINED = -1;
        protected States(){}
    }
}