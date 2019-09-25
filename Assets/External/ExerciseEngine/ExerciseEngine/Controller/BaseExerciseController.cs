using System;
using System.Collections.Generic;
using System.Linq;
using ExerciseEngine.Controller.Interfaces;
using ExerciseEngine.Controller.ValueObjects;
using ExerciseEngine.Controller.ValueObjects.Interfaces;
using ExerciseEngine.Goal;
using ExerciseEngine.HUD.Enum;
using ExerciseEngine.HUD.Interfaces;
using ExerciseEngine.HUD.ValueObjects;
using ExerciseEngine.HUD.ValueObjects.Interfaces;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.Interfaces;
using ExerciseEngine.Model.Tutorial;
using ExerciseEngine.Model.Tutorial.Interfaces;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Settings.Enums;
using ExerciseEngine.Sound;
using ExerciseEngine.Sound.Interfaces;
using ExerciseEngine.Terminator.Interfaces;
using ExerciseEngine.Terminator.Triggers;
using ExerciseEngine.Terminator.Triggers.Interfaces;
using ExerciseEngine.Terminator.ValueObjects.Interfaces;
using ExerciseEngine.View.Interfaces;
using ExerciseEngine.View.ValueObjects;
using ExerciseEngine.View.ValueObjects.Interfaces;
using SynaptikonFramework.Interfaces.DebugLog;
using SynaptikonFramework.Interfaces.Util.Timer;

namespace ExerciseEngine.Controller
{
    public class BaseExerciseController: StateMachine, IExerciseController, IExerciseStateHandler {
        protected readonly IExerciseModel _model;
        protected readonly IExerciseBackgroundView _backgroundView;
        protected readonly ISoundManager _soundManager;
        protected readonly IExerciseHUD _hud;

        public event ControllerUpdateHandler OnUpdate;

        public bool InputEnabled;
        protected string _nextWrongSound = AudioEffect.Wrong1;
        protected string _nextCorrectSound = AudioEffect.Correct1;

        protected ITimer _timeoutTimer;
        protected ITimer _exerciseTimer;
        protected ITimer _botTimer;

        protected bool _botActived;
        protected ITutorialManager _tutorialManager;
        protected TutorialActivityManager _tutorialActivityManager;
        protected IExerciseRoundDataVO _currentRound;
        private bool _stopped;

        public BaseExerciseController(IExerciseModel exerciseModel , IExerciseView exerciseView, IExerciseBackgroundView exerciseBackgroundView, IExerciseHUD hud, ITimerFactory timerFactory,
                                      IExerciseTerminator terminator, ILogger logger, ISoundManager soundManager) : base(logger, terminator, exerciseView) {
            _soundManager = soundManager;
            _model = exerciseModel;
            _backgroundView = exerciseBackgroundView;
            _hud = hud;

            _timeoutTimer = timerFactory.CreateTimer();
            _exerciseTimer = timerFactory.CreateTimer();
            _botTimer = timerFactory.CreateTimer();
            _botTimer.OnComplete += ProcessBotAnswer;

            _tutorialManager = new TutorialManager(_model, _view, _model.ExerciseConfiguration);
            _tutorialManager.OnUpdate += OnTutorialUpdate;

            _tutorialActivityManager = new TutorialActivityManager(logger, 3, 2, exerciseModel.ExerciseInitVO.StartWithTutorial, exerciseModel.ExerciseInitVO.TutorialSystemEnabled);

            var exerciseSettings = _model.ExerciseSettings;

            _view.Settings = exerciseSettings;
            _soundManager.Mute = exerciseSettings.ContainsKey(ExerciseSettingsEnum.SoundsEnabled) && !exerciseSettings[ExerciseSettingsEnum.SoundsEnabled];

            MapState(BaseStates.INIT, OnStateInit, AfterStateInit);

            _botActived = false;

            _stopped = false;
        }

        /// <summary>
        /// Maps the states.
        /// </summary>
        public virtual void MapStates() {
            _logger.LogMessage(LogLevel.Informational, "Mapping normal states");
            MapState(BaseStates.SHOW_ROUND, OnStateShowRound, AfterStateShowRound);
            MapState(BaseStates.INPUT, OnStateInput, AfterStateInput);
            MapState(BaseStates.CORRECT_ANSWER, OnStateCorrectAnswer, AfterStateCorrectAnswer);
            MapState(BaseStates.CORRECT_STEP, OnStateCorrectStep, AfterStateCorrectStep);
            MapState(BaseStates.WRONG_ANSWER, OnStateWrongAnswer, AfterStateWrongAnswer);
            MapState(BaseStates.WRONG_STEP, OnStateWrongStep, AfterStateWrongStep);
            MapState(BaseStates.MEMORIZE_STEP, OnStateMemorize, AfterStateMemorize);
        }

        protected virtual void MapTutorialStates(ITutorialStateHandler tutorialStateHandler) {
            if(!tutorialStateHandler.Initialised) tutorialStateHandler.Init(_model, this, _view);
            _logger.LogMessage(LogLevel.Informational, "Mapping  tutorial states type: " + tutorialStateHandler.GetType().ToString());
            MapState(BaseStates.SHOW_ROUND, tutorialStateHandler.OnStateShowRound, tutorialStateHandler.AfterStateShowRound);
            MapState(BaseStates.INPUT, tutorialStateHandler.OnStateInput, tutorialStateHandler.AfterStateInput);
            MapState(BaseStates.CORRECT_ANSWER, tutorialStateHandler.OnStateCorrectAnswer, tutorialStateHandler.AfterStateCorrectAnswer);
            MapState(BaseStates.CORRECT_STEP, tutorialStateHandler.OnStateCorrectStep, tutorialStateHandler.AfterStateCorrectStep);
            MapState(BaseStates.WRONG_ANSWER, tutorialStateHandler.OnStateWrongAnswer, tutorialStateHandler.AfterStateWrongAnswer);
            MapState(BaseStates.WRONG_STEP, tutorialStateHandler.OnStateWrongStep, tutorialStateHandler.AfterStateWrongStep);
            MapState(BaseStates.MEMORIZE_STEP, tutorialStateHandler.OnStateMemorize, tutorialStateHandler.AfterStateMemorize);
        }

        public virtual void OnStateInit() {
            if(!_stopped){
                try {
                    _model.CreateRound((round) => {
                        _currentRound = round;
                        var tutorialStateHandler = _tutorialManager.GetTutorialTypeAndUpdate();
                        if(tutorialStateHandler == TutorialManager.TYPE_NO_TUTORIAL) {
                            MapStates();
                        } else {
                            MapTutorialStates(tutorialStateHandler);
                        }
                        CurrentState = BaseStates.SHOW_ROUND;
                    });
                } catch(Exception e) {
                    _logger.LogMessage(LogLevel.Error, "OnStateInit " + e.Message);
                }
            }
        }

        protected virtual IInitialViewDataVO InitialViewData => null;

        public virtual IRoundIndependentVO InitialModelData {
            set;
            protected get;
        }

        /// <summary>
        /// Pauses the timer.
        /// </summary>
        /// <param name="pause">If set to <c>true</c> pause.</param>
        protected virtual void PauseTimer(bool pause) {

            if(pause) {
                if(_timeoutTimer != null) {
                    _timeoutTimer.Pause();
                }
                if(_exerciseTimer != null) {
                    _exerciseTimer.Pause();
                }

                _hud.Update(new List<IExerciseHudVO>() { new PauseHudAnimationsVO() });
            } else {
                if(_timeoutTimer != null && _timeoutTimer.IsPaused) {
                    _timeoutTimer.Resume();
                }
                if(_exerciseTimer != null && _exerciseTimer.IsPaused) {
                    _exerciseTimer.Resume();
                }
                _hud.Update(new List<IExerciseHudVO>() { new ResumeHudAnimationsVO() });
            }
        }

        private void OnTimerUpdate(ITimerUpdateVO updateVO) {
            if(_model.CurrentRound?.WarmUpState != WarmUpState.Enabled) {
                _terminator.HandelTermiantionTrigger(new TimeTerminationTrigger(updateVO));
                _hud.Update(new List<IExerciseHudVO>() { new UpdateExerciseTimeVO(updateVO) });
            }
        }

        public virtual void AfterStateInit() {
        }

        public virtual void OnStateShowRound() {

            if(_currentRound.WarmUpState == WarmUpState.JustCompleted) {
                // reset the current time 
                // todo: make a function for that
                _exerciseTimer.Stop();
                _exerciseTimer.Start(1000, true);
                _hud.Update(new List<IExerciseHudVO>() { new WarmupFinishedVO() });
            }
            CreateViewRound(_currentRound);
        }

        public virtual void CreateViewRound(IExerciseRoundDataVO roundData) {
            if(_botActived)
            {
                UpdateBotTimeouts(roundData);
            }
            
            UpdateTutorialForNewRound();
            if(roundData.LevelState == LevelState.NEW) {
                _view.CreateInitialRound(roundData);
            } else if(roundData.LevelState == LevelState.UP) {
                _view.CreateRoundLevelUp(roundData);
            } else if(roundData.LevelState == LevelState.DOWN) {
                _view.CreateRoundLevelDown(roundData);
            } else {
                _view.CreateRound(roundData);
            }
        }

        protected virtual void UpdateBotTimeouts(IExerciseRoundDataVO roundData)
        {
            _logger.LogMessage(LogLevel.Warning, "bot active shorter timeout");
            roundData.MemorizeTimeout = 1000;
        }

        public virtual void UpdateTutorialForNewRound()
        {
            if(_tutorialActivityManager.TutorialSystemEnabled && _tutorialActivityManager.TutorialActive)
            {
                _view.ShowTutorial(GeneralTutorialText());
            }
        }

        public virtual void DisableTutorialAfterAnswer()
        {
            if (_tutorialActivityManager.TutorialSystemEnabled && !_tutorialActivityManager.TutorialActive)
            {
                _view.HideTutorial();
            }
        }

        public virtual ITutorialVO GeneralTutorialText()
        {
            return new BaseTutorialVO("");
        }

        public virtual IRoundIndependentUpdateResultVO MemorizePhaseText() {
            return new MemorizeTimeStartedVO("");
        }

        public virtual void EnableTutorial()
        {
            _tutorialActivityManager.ActivateTutorail();
            _view.ShowTutorial(GeneralTutorialText());
        }

        public virtual void AfterStateShowRound() {
        }

        public virtual void OnStateInput() {
            StartTimeout();
            InputEnabled = true;
            RunBot();
            // store start moment input can be given
            _model.StartMonitorReactionTime(DateTime.Now);
        }

        protected virtual void StartTimeout() {
            if(_model.IsTimeoutEnabled && !_tutorialActivityManager.TutorialActive) {
                _logger.LogMessage(LogLevel.Informational, "Start timeout: " + _model.CurrentRound.Timeout);
                _timeoutTimer.OnComplete += OnTimeOutComplete;
                _timeoutTimer.Start(_model.CurrentRound.Timeout);

                _hud.Update(new List<IExerciseHudVO>() { new StartTimeoutVO(_model.CurrentRound.Timeout, TimeoutType.Input) });
            }
        }

        protected virtual void OnTimeOutComplete(ITimerUpdateVO timerUpdateVO) {
            _logger.LogMessage(LogLevel.Informational, "OnTimeOutComplete");
            InputEnabled = false;
            ResetTimeoutTimer();
            _hud.Update(new List<IExerciseHudVO>() { new ShowTimeoutVO(TimeoutType.Input) });
            _soundManager.PlaySound(AudioEffect.Timeout);
            _model.EvaluateAnswer(HandleAnswerResult, null);
        }

        protected void StartMemorizeTimeout(int timeout) {
            _logger.LogMessage(LogLevel.Informational, "Start memorize timeout: " + timeout);
            _timeoutTimer.OnComplete += OnMemorizeTimeoutComplete;
            _timeoutTimer.Start(timeout);
            _hud.Update(new List<IExerciseHudVO>() { new StartTimeoutVO(timeout, TimeoutType.Memorized) });
        }

        protected virtual void OnMemorizeTimeoutComplete(ITimerUpdateVO timerUpdateVO = null) {
            ResetTimeoutTimer();
            _hud.Update(new List<IExerciseHudVO>() { new ShowTimeoutVO(TimeoutType.Memorized) });
            _view.Update(new MemorizeTimeOverVO());
        }

        protected void StopTimeout() {
            if(_timeoutTimer != null) {
                _timeoutTimer.Pause();
            }
        }

        protected void ResetTimeoutTimer() {
            if(_timeoutTimer != null) {
                _timeoutTimer.Stop();
                _timeoutTimer.OnComplete -= OnMemorizeTimeoutComplete;
                _timeoutTimer.OnComplete -= OnTimeOutComplete;
            }
            _hud.Update(new List<IExerciseHudVO>() { new ResetTimeoutVO() });
        }

        public virtual void AfterStateInput() {
            InputEnabled = false;
        }

        public virtual void OnStateWrongAnswer() {
            PlayWrongSound(_model.CurrentRoundResult);
            ResetTimeoutTimer();
            _view.ShowWrong(_model.CurrentRoundResult);
        }

        public virtual void AfterStateWrongAnswer() {
        }

        public virtual void OnStateCorrectAnswer() {
            PlayCorrectSound(_model.CurrentRoundResult);
            ResetTimeoutTimer();      
            _view.ShowCorrect(_model.CurrentRoundResult);
        }

        public virtual void AfterStateCorrectAnswer() {
        }

        public virtual void OnStateCorrectStep() {
            OnStateCorrectAnswer();
        }

        public virtual void AfterStateCorrectStep() {
        }

        public virtual void OnStateWrongStep() {
            OnStateWrongAnswer();
        }

        public virtual void AfterStateWrongStep() {
        }

        public virtual void OnStateMemorize() {
            if(_model.CurrentRound.MemorizeTimeout != 0) {
                _view.Update(MemorizePhaseText());
                if (_model.CurrentRound.MemorizeTimeout > 0) {
                    StartMemorizeTimeout(_model.CurrentRound.MemorizeTimeout);
                }  
            }
        }

        public virtual void AfterStateMemorize() {
        }

        public virtual void OnViewUpdate(IExerciseViewUpdateVO vo)
        {
            if (vo is IAnswerVO)
            {
                if (InputEnabled)
                {
                    ResetTimeoutTimer();
                    InputEnabled = false;
                    _model.EvaluateAnswer(HandleAnswerResult, vo as IAnswerVO, _tutorialManager.IsTutorialRunning);
                }
            }
            else if (vo is IFeedbackShownVO)
            {
                _logger.LogMessage(LogLevel.Informational, "Current round result: " + _model.CurrentRoundResult);
                if (_model.CurrentRoundResult != null && _model.CurrentRoundResult.RoundCompleted)
                {
                    CurrentState = BaseStates.INIT;
                }
                else
                {
                    CurrentState = BaseStates.INPUT;
                }
            }
            else if (vo is IRoundCreatedVO)
            {
                CurrentState = BaseStates.INPUT;
            }
            else if (vo as IRoundIndependentVO != null)
            {
                IRoundIndependentUpdateResultVO result = _model.RoundIndependentUpdate((IRoundIndependentVO)vo);
                _hud.Update(result.Updates.OfType<IExerciseHudVO>().ToList());
                var terminatorTriggers = result.Updates.OfType<ITerminatorTrigger>();
                foreach (var t in terminatorTriggers)
                {
                    _terminator.HandelTermiantionTrigger(t);
                }
                _view.Update(result);
            }
            else if (vo.GetType() == typeof(StartMemorizeVO))
            {
                CurrentState = BaseStates.MEMORIZE_STEP;
            }
            else if (vo.GetType() == typeof(StopMemorizeVO))
            {
                OnMemorizeTimeoutComplete();
            }
            else if (vo.GetType() == typeof(ExerciseStageTapedVO))
            {
                _backgroundView.Tap((vo as ExerciseStageTapedVO).Position);
            }
            else if (vo is ShowScoreAnimationVO)
            {
                _hud.Update(new List<IExerciseHudVO>() { vo as IExerciseHudVO });
            }
            else if (vo is ExerciseSettingsChangedVO)
            {
                // Notify Mediator about settings change.
                NotifyMediatorUpdate((ExerciseSettingsChangedVO)vo);
            }
        }

        protected void HandleAnswerResult(IRoundEvaluationResultVO result) {
            _tutorialActivityManager.EvaluateResult(result);
            DisableTutorialAfterAnswer();
            _hud.Update(result.Updates.OfType<IExerciseHudVO>().ToList());
            var terminatorTriggers = result.Updates.OfType<ITerminatorTrigger>();
            foreach(var t in terminatorTriggers) {
                _terminator.HandelTermiantionTrigger(t);
            }
            if(result.AnswerCorrect)
                CurrentState = result.RoundCompleted ? BaseStates.CORRECT_ANSWER : BaseStates.CORRECT_STEP;
            else
                CurrentState = result.RoundCompleted ? BaseStates.WRONG_ANSWER : BaseStates.WRONG_STEP;
        }

        protected virtual void OnHudUpdate(IExerciseHUDUpdateVO vo) {
            var pauseVO = vo as PauseExerciseRequestVO;
            if (pauseVO != null) {
                OnUpdate?.Invoke(pauseVO);
                Suspend();
            }
        }

        protected virtual void OnTerminatorUpdate(IExerciseTerminatorUpdateVO vo) {
            _logger.LogMessage(LogLevel.Informational, "terminate because: " + vo?.ToString());
            Stop(true);
        }

        /* allow to override function and send message from the child classes */
        public void NotifyMediatorUpdate(IExerciseControllerUpdateVO updateVO) {
            OnUpdate?.Invoke(updateVO);
        }

        public void Start() {
            _view.OnUpdate += OnViewUpdate;
            _view.OnGameStart(InitialViewData);
            _terminator.OnTerminate += OnTerminatorUpdate;

            var initialState = _model.RoundIndependentUpdate(InitialModelData);
            var updates = initialState.Updates.OfType<IExerciseHudVO>().ToList();
            updates.Add(new StartExerciseTimeVO());
            _hud.Update(updates);
            _hud.OnUpdate += OnHudUpdate;


            _exerciseTimer.OnComplete += OnTimerUpdate;
            _exerciseTimer.Start(1000, true);
            _soundManager.PlaySound(AudioEffect.StartGame);

            CurrentState = BaseStates.INIT;
        }

        public virtual void Suspend() {
            PauseTimer(true);
            _view.Suspend();
        }

        public virtual void Resume() {
            PauseTimer(false);
            _view.Resume();
        }

        public void PlayCorrectSound(IRoundEvaluationResultVO result) {
            // Level up.
            if(result.Updates.Any((updateVO) => { return (updateVO is DifficultyUpdateVO && updateVO.ValueChange > 0); })) {
                _soundManager.PlaySound(AudioEffect.LevelUp);
            } else {
                _soundManager.PlaySound(_nextCorrectSound);
                if(_nextCorrectSound == AudioEffect.Correct1) {
                    _nextCorrectSound = AudioEffect.Correct2;
                } else {
                    _nextCorrectSound = AudioEffect.Correct1;
                }
            }
        }

        public void PlayWrongSound(IRoundEvaluationResultVO result) {
            // Level down.
            if(result.Updates.Any((updateVO) => { return (updateVO is DifficultyUpdateVO && updateVO.ValueChange < 0); })) {
                _soundManager.PlaySound(AudioEffect.LevelDown);
            } else {
                _soundManager.PlaySound(_nextWrongSound);
                if(_nextWrongSound == AudioEffect.Wrong1) {
                    _nextWrongSound = AudioEffect.Wrong2;
                } else {
                    _nextWrongSound = AudioEffect.Wrong1;
                }
            }
        }

        /// <summary>
        /// Runs the bot for system play alone.
        /// </summary>
        protected void RunBot() {
            //try to run the bot
            if(_botActived && _model.BotAnswer != null) {
                _botTimer.Start(_model.BotRoundDelay);
            }
        }

        private void ProcessBotAnswer(ITimerUpdateVO vo) {
            OnViewUpdate(_model.BotAnswer);
        }

        public virtual void Stop(bool exerciseFinished = false) {
            _stopped = true;
#pragma warning disable CS0168
            _logger.LogMessage(LogLevel.Informational, "Stop");

            PauseTimer(true);

            // remove listeners
            try {
                _exerciseTimer.OnComplete -= OnTimerUpdate;
            } catch(Exception e) {
                // no handler to remove
            }
            try {
                _timeoutTimer.OnComplete -= OnTimeOutComplete;
            } catch(Exception e) {
                // no handler to remove
            }
            try {
                _timeoutTimer.OnComplete -= OnMemorizeTimeoutComplete;
            } catch(Exception e) {
                // no handler to remove
            }
            try {
                _view.OnUpdate -= OnViewUpdate;
            } catch(Exception e) {
                // no handler to remove
            }
            try {
                _terminator.OnTerminate -= OnTerminatorUpdate;
            } catch(Exception e) {
                // no handler to remove
            }
            try {
                _botTimer.OnComplete -= ProcessBotAnswer;
            } catch(Exception e) {
                // no handler to remove
            }
            try {
                _tutorialManager.OnUpdate -= OnTutorialUpdate;
            } catch(Exception e) {
                // no handler to remove
            }
#pragma warning restore CS0168


            var exerciseResult = _model.Stop();


            _exerciseTimer.Pause();
            _timeoutTimer.Pause();
            _view.EndExercise();
            InputEnabled = false;
            _soundManager.PlaySound(AudioEffect.StopGame);

            NotifyMediatorUpdate(new FinishExerciseVO(exerciseResult, exerciseFinished));
        }

        public int CurrentTutorialLoop {
            get {
                return _tutorialManager.CurrentTutorialLoop;
            }
        }

        public Dictionary<ExerciseSettingsEnum, bool> ExerciseSettings { 
            set {
                // Notify view about settings change.
                _view.Settings = value;

                // Handle sounds manager.
                if(value.ContainsKey(ExerciseSettingsEnum.SoundsEnabled))
                {
                    _soundManager.Mute = !value[ExerciseSettingsEnum.SoundsEnabled];
                }
            }
        }

        private void OnTutorialUpdate(ITutorialManagerUpdateVO vo) {
            if(vo is IExerciseControllerUpdateVO) {
                //controller dispatches signal on update signal from tutorial manager
                OnUpdate?.Invoke((IExerciseControllerUpdateVO)vo);
            }
        }

        protected override void Dispose(bool disposing) {
            if(!_disposed)
            {
                // someone called Dispose()
                _exerciseTimer.Stop();
                _exerciseTimer.Dispose();
                _timeoutTimer.Stop();
                _timeoutTimer.Dispose();
                _tutorialManager.Dispose();
            }
            base.Dispose(disposing);
        }

        public void SwitchBot()
        {
            _botActived = !_botActived;
            if (CurrentState == BaseStates.INPUT)
            {
                RunBot();
            }
        }
    }

    public class BaseStates: States {
        public const int INIT = 1;
        public const int SHOW_ROUND = 2;
        public const int INPUT = 3;
        public const int WRONG_ANSWER = 4;
        public const int CORRECT_ANSWER = 5;
        public const int CORRECT_STEP = 6;
        public const int WRONG_STEP = 7;
        public const int MEMORIZE_STEP = 8;
    }
}