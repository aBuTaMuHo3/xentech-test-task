using System;
using ExerciseEngine.Controller;
using ExerciseEngine.HUD.Interfaces;
using ExerciseEngine.Model.Interfaces;
using ExerciseEngine.Model.Tutorial;
using ExerciseEngine.Model.Tutorial.Interfaces;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Sound.Interfaces;
using ExerciseEngine.Terminator.Interfaces;
using ExerciseEngine.View.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;
using FlashGlance.Model;
using FlashGlance.Model.ValueObjects;
using SynaptikonFramework.Interfaces.DebugLog;
using SynaptikonFramework.Interfaces.Util.Timer;

namespace FlashGlance.Controller
{
    public class FlashGlanceController : BaseExerciseController
    {
        private readonly ITimer _spawnTimer;
        private readonly ITimer _moveTimer;
        private readonly ITimer _switchTimer;
        private readonly Random _random;

        public FlashGlanceController(IExerciseModel exerciseModel, IExerciseView exerciseView, IExerciseBackgroundView exerciseBackgroundView, IExerciseHUD hud, ITimerFactory timerFactory, IExerciseTerminator terminator, ILogger logger, ISoundManager soundManager) : base(exerciseModel, exerciseView, exerciseBackgroundView, hud, timerFactory, terminator, logger, soundManager)
        {
            _random = new Random();

            _spawnTimer = timerFactory.CreateTimer();
            _spawnTimer.OnComplete += RequestNewItem;

            _moveTimer = timerFactory.CreateTimer();
            _moveTimer.OnComplete += RequestMovement;

            _switchTimer = timerFactory.CreateTimer();
            _switchTimer.OnComplete += RequestSwap;
        }

        public override void CreateViewRound(IExerciseRoundDataVO roundData)
        {
            base.CreateViewRound(roundData);
            // Manage timers here.
            updateEvantTimer(FlashGlanceEventType.ItemSpawning);
            updateEvantTimer(FlashGlanceEventType.ItemMovement);
            updateEvantTimer(FlashGlanceEventType.ItemSwitching);
        }

        private void updateEvantTimer(FlashGlanceEventType eventType){
            FlashGlanceEventVo eventConfig = GetEventConfig(eventType);
            ITimer timer = null;
            switch (eventType)
            {
                case FlashGlanceEventType.ItemSpawning:
                    timer = _spawnTimer;
                    break;
                case FlashGlanceEventType.ItemMovement:
                    timer = _moveTimer;
                    break;
                case FlashGlanceEventType.ItemSwitching:
                    timer = _switchTimer;
                    break;
            }
            if(timer != null){
                if (eventConfig.Amount > 0 && eventConfig.MinDelay > 0 && eventConfig.MaxDelay > 0)
                {
                    // Action present
                    if (!timer.IsRunning) timer.Start(GetRandomDelay(eventConfig));
                }
                else
                {
                    // No action
                    if (timer.IsRunning) timer.Stop();
                }
            }
        }

        public override void OnViewUpdate(IExerciseViewUpdateVO vo)
        {
            if (vo is FlashGlanceItemUnlockedVO || vo is FlashGlanceItemHiddenVO)
            {
                _model.RoundIndependentUpdate((IRoundIndependentVO)vo);
            }
            else
            {
                base.OnViewUpdate(vo);
            }
        }

        private FlashGlanceEventVo GetEventConfig(FlashGlanceEventType eventType)
        {
            if (_model.ExerciseConfiguration is FlashGlanceConfiguration castedConfig)
            {
                switch (eventType)
                {
                    case FlashGlanceEventType.ItemSpawning:
                        return castedConfig.GetNewItemtemSpawningByLevel(_model.CurrentRound.Difficulty);
                    case FlashGlanceEventType.ItemMovement:
                        return castedConfig.GetItemMovingByLevel(_model.CurrentRound.Difficulty);
                    case FlashGlanceEventType.ItemSwitching:
                        return castedConfig.GetItemSwitchingByLevel(_model.CurrentRound.Difficulty);
                    default:
                        return new FlashGlanceEventVo() { Amount = 0, MaxDelay = 0, MinDelay = 0 };
                }
            }
            else
            {
                return new FlashGlanceEventVo() { Amount = 0, MaxDelay = 0, MinDelay = 0 };
            }
        }
        
        private void RequestNewItem(ITimerUpdateVO vo)
        {
            _view.Update(_model.RoundIndependentUpdate(new FlashGlanceEventRequestVO(FlashGlanceEventType.ItemSpawning)));
            FlashGlanceEventVo spawnConfig = GetEventConfig(FlashGlanceEventType.ItemSpawning);
            _spawnTimer.Start(GetRandomDelay(spawnConfig));
        }

        private void RequestMovement(ITimerUpdateVO vo)
        {
            _view.Update(_model.RoundIndependentUpdate(new FlashGlanceEventRequestVO(FlashGlanceEventType.ItemMovement)));
            FlashGlanceEventVo moveConfig = GetEventConfig(FlashGlanceEventType.ItemMovement);
            _moveTimer.Start(GetRandomDelay(moveConfig));
        }

        private void RequestSwap(ITimerUpdateVO vo)
        {
            _view.Update(_model.RoundIndependentUpdate(new FlashGlanceEventRequestVO(FlashGlanceEventType.ItemSwitching)));
            FlashGlanceEventVo swichConfig = GetEventConfig(FlashGlanceEventType.ItemSwitching);
            _switchTimer.Start(GetRandomDelay(swichConfig));
        }

        private double GetRandomDelay(FlashGlanceEventVo eventVo){
            var delay = ((_random.NextDouble() * (eventVo.MaxDelay - eventVo.MinDelay) + eventVo.MinDelay) * 1000);
            return delay;
        }

        public override void Suspend()
        {
            _spawnTimer.Pause();
            _moveTimer.Pause();
            _switchTimer.Pause();
            base.Suspend();
        }

        public override void Resume()
        {
            _spawnTimer.Resume();
            _moveTimer.Resume();
            _switchTimer.Resume();
            base.Resume();
        }

        public override void Stop(bool exerciseFinished = false)
        {
            _spawnTimer.Pause();
            _spawnTimer.OnComplete -= RequestNewItem;

            _moveTimer.Pause();
            _moveTimer.OnComplete -= RequestMovement;

            _switchTimer.Pause();
            _switchTimer.OnComplete -= RequestSwap;

            base.Stop(exerciseFinished);
        }

        public override ITutorialVO GeneralTutorialText()
        {
            return new BaseTutorialVO("FlashGlance_instruction_text");
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // someone called Dispose()
                _spawnTimer.Stop();
                _spawnTimer.Dispose();
                _moveTimer.Stop();
                _moveTimer.Dispose();
                _switchTimer.Stop();
                _switchTimer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}