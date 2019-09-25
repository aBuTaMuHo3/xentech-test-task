using System;
using System.Collections.Generic;
using ExerciseEngine.Model.Interfaces;
using ExerciseEngine.Model.Tutorial.Interfaces;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.View.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.Tutorial
{
    public class TutorialManager : ITutorialManager
    {
        private IExerciseModel _model;
        private IExerciseView _view;

        /// <summary>
        /// Exercise configuration for getting tutorial data.
        /// </summary>
        protected IExerciseConfiguration _exerciseConfiguration;

        /// <summary>
        /// Dictionary keeping track of number for finished tutorials during run.
        /// </summary>
        protected Dictionary<Type, int> _tutorialRuns;

        /// <summary>
        /// For tracking number of loops for tutorial.
        /// </summary>
        protected int _tutorialLoops;

        /// <summary>
        /// Holds current running tutorial configuration.
        /// </summary>
        protected ITutorialConfiguration _currentTutorialConfig;

        /// <summary>
        /// Occurs when on update. For dispatching changes in TutorialManager.
        /// </summary>
        public event TutorialManagerUpdateHandler OnUpdate;

        protected ITutorialStateHandler _currentTutorialHandler;

        public const ITutorialStateHandler TYPE_NO_TUTORIAL = null;

        private List<ITutorialTrigger> _currentRoundTriggers;  

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExerciseEngine.Model.Tutorial.TutorialManager"/> class.
        /// </summary>
        /// <param name="model">Model. For listening for level changes.</param>
        /// <param name="view">View. For triggers fired from the view.</param>
        /// <param name="exerciseConfig">Exercise config. For getting tutorial configurations.</param>
        /// <param name="currentDifficulty">Current difficulty. Starting difficulty of exercises</param>
        public TutorialManager(IExerciseModel model, IExerciseView view, IExerciseConfiguration exerciseConfig)
        {
            _model = model;
            _model.OnTutorialTrigger += OnModelUpdate;
            _view = view;
            _view.OnUpdate += OnViewUpdate;
            _exerciseConfiguration = exerciseConfig;
            _tutorialRuns = new Dictionary<Type, int>();
            _tutorialLoops = 0;
            _currentRoundTriggers = new List<ITutorialTrigger>();
        }


        /// <summary>
        /// Listen to model update signal to track difficulty changes.
        /// </summary>
        /// <returns>The model update.</returns>
        /// <param name="vo">.</param>
        protected void OnModelUpdate(ITutorialTrigger vo){
            _currentRoundTriggers.Add((ITutorialTrigger)vo);
       
        }

        protected void OnViewUpdate(IExerciseViewUpdateVO vo){
            if(vo is ITutorialTrigger){
                _currentRoundTriggers.Add((ITutorialTrigger)vo);
            }
        }

        /// <summary>
        /// Increases tutorial loop and checks if tutorial should be ended.
        /// </summary>
        /// <returns>The tutorial loop.</returns>
        /// <param name="amount">.</param>
        protected bool IncreaseTutorialLoop(int amount){
            _tutorialLoops += amount;
            return _tutorialLoops > _currentTutorialHandler.Loops;
        }

        /// <summary>
        /// Checking mount of finished tutorial type this round.
        /// </summary>
        /// <returns>The tutorial runs.</returns>
        /// <param name="type">.</param>
        protected int GetTutorialRuns(Type type){
            return _tutorialRuns.ContainsKey(type) ? _tutorialRuns[type] : 0;
        }


        public ITutorialStateHandler GetTutorialTypeAndUpdate(){
            // if currently tutorial is running update its loop
            if(_currentTutorialConfig != null){
                var tutFinished = IncreaseTutorialLoop(_currentTutorialHandler.TutorialLoopIncrease);
                if(tutFinished){
                    var tutorialType = _currentTutorialConfig.HandlerType;
                    //finish tutorial
                    _currentTutorialConfig = null;
                    _currentTutorialHandler.Dispose();
                    _currentTutorialHandler = null;
                    // mark how may times this type of tutorial finished for running it again
                    if(_tutorialRuns.ContainsKey(tutorialType)){
                        _tutorialRuns[tutorialType]++;
                    }else{
                        _tutorialRuns.Add(tutorialType, 1); 
                    }
                    // dispatch tutorial finish signal
                    OnUpdate?.Invoke(new GuidedTourUpdateVO(false, tutorialType.Name));               
                }else{
                    //tutorial still running
                    _currentRoundTriggers = new List<ITutorialTrigger>();
                    return _currentTutorialHandler;
                }
            }
            //if there is no tutorial or it was finished check if new one should be started
            // and there is no current running tutorial -> start a new one
            if(_currentRoundTriggers.Count > 0  && _currentTutorialConfig == null){
                ITutorialConfiguration newTutorialConfig = null;
                ITutorialTrigger newTutorialTrigger = null;
                foreach(var trigger in _currentRoundTriggers)
                {
                    foreach(var config in _exerciseConfiguration.TutorialConfigurations){
                        if (config.TriggerType != null && trigger.GetType() == config.TriggerType)
                        {
                            // if there is a new tutorial config with still defined repeats
                            // and this tutorial wasn't been shown already enough times
                            if (config != null && config.Repeats > 0 && GetTutorialRuns(config.HandlerType) == 0)
                            {
                                if (newTutorialConfig == null)
                                {
                                    newTutorialConfig = config;
                                    newTutorialTrigger = trigger;
                                }
                                else if (config.Priority > newTutorialConfig.Priority)
                                {
                                    newTutorialConfig = config;
                                    newTutorialTrigger = trigger;
                                }
                            }
                        }
                    }
                }

                if(newTutorialConfig != null){
                    // assign new tutorial as currently running
                    _currentTutorialConfig = newTutorialConfig;
                    // set tutorial first loop
                    _tutorialLoops = 1;
                    // dispatch tutorial started signal
                    OnUpdate?.Invoke(new GuidedTourUpdateVO(true, _currentTutorialConfig.HandlerType.Name));  

                    // indicate new tutorial
                    _currentTutorialHandler = (ITutorialStateHandler)Activator.CreateInstance(_currentTutorialConfig.HandlerType);
                    _currentTutorialHandler.Trigger = newTutorialTrigger;
                    _currentRoundTriggers = new List<ITutorialTrigger>();
                    return _currentTutorialHandler;
                }
            }
            _currentRoundTriggers = new List<ITutorialTrigger>();

            //there is no tutorial right now
            return TYPE_NO_TUTORIAL;
        }

        public bool IsTutorialRunning {
            get{
                return _currentTutorialConfig != null;
            }
        }

        public int CurrentTutorialLoop{
            get{
                return _tutorialLoops;
            }
        }

        public void Dispose(){
            _model.OnTutorialTrigger -= OnModelUpdate;
            _view.OnUpdate -= OnViewUpdate;
            //_updateSignal.removeAll();
            if (_currentTutorialHandler != null) {
                _currentTutorialHandler.Dispose();
            }
        }
    }
}
