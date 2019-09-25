using System;
using System.Collections.Generic;
using ExerciseEngine.Controller.ValueObjects.Interfaces;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.Controller.Interfaces
{
    public delegate void ControllerUpdateHandler( IExerciseControllerUpdateVO vo );
	
    public interface IExerciseController : IDisposable
	{
		void Start();
		void Suspend();
		void Resume();
        void EnableTutorial();
        void Stop(bool exerciseFinished = false);
		event ControllerUpdateHandler OnUpdate;
        Dictionary<ExerciseSettingsEnum, bool> ExerciseSettings { set; }
        void SwitchBot();
        IRoundIndependentVO InitialModelData { set; }
    }
}
