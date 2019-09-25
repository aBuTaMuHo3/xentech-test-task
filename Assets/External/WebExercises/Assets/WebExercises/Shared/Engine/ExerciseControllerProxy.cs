using ExerciseEngine.Controller;
using ExerciseEngine.Controller.ValueObjects.Interfaces;
using ExerciseEngine.HUD.ValueObjects;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.View.ValueObjects;
using ExerciseEngine.View.ValueObjects.Interfaces;
using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Signals;
using SynaptikonFramework.Interfaces.Util.Timer;

namespace WebExercises.Shared
{
	public interface IExerciseControllerProxy
	{
		BaseExerciseController Controller { get; set; }
		
		Signal<IInitialViewDataVO> OnGameStart { get; }
		Signal OnStartExercise { get; }
		Signal<IExerciseRoundDataVO> OnCreateInitialRound { get; }
		Signal<IExerciseRoundDataVO> OnCreateRound { get; }
		Signal<IRoundEvaluationResultVO> OnShowCorrect { get; }
		Signal<IRoundEvaluationResultVO> OnShowWrong { get; }
		Signal<IExerciseResult> OnEndExercise { get; }
		
		Signal<StartTimeoutVO> OnStartTimeout { get; }
		Signal OnResetTimeout { get; }
		Signal OnMemorizeTimeOver { get; }
		
		Signal<int> OnUpdateRuns { get; }
		
		Signal<ITimerUpdateVO> OnUpdateTime { get; }

		Signal<HUDLevelInfo> OnInitLevelHUD { get; }
		Signal<int> OnUpdateScore { get; }
		
		bool IsPaused { get; set; }

		void RoundCreated();
		void CheckAnswer(IRoundItem roundItem);
		void FeedbackShown();
		void StartMemorize(int timeout);
		void StopMemorize();
	}

	public class ExerciseControllerProxy : IExerciseControllerProxy
	{
		[Inject] public IGenerateExerciseResult generateExerciseResult;

		private BaseExerciseController controller;

		public BaseExerciseController Controller
		{
			get => controller;
			set
			{
				controller = value;
				controller.OnUpdate += OnControllerUpdate;
			}
		}

		public Signal<IInitialViewDataVO> OnGameStart { get; } = new Signal<IInitialViewDataVO>();
		public Signal OnStartExercise { get; } = new Signal();
		public Signal<IExerciseRoundDataVO> OnCreateInitialRound { get; } = new Signal<IExerciseRoundDataVO>();
		public Signal<IExerciseRoundDataVO> OnCreateRound { get; } = new Signal<IExerciseRoundDataVO>();
		public Signal<IRoundEvaluationResultVO> OnShowCorrect { get; } = new Signal<IRoundEvaluationResultVO>();
		public Signal<IRoundEvaluationResultVO> OnShowWrong { get; } = new Signal<IRoundEvaluationResultVO>();
		public Signal<IExerciseResult> OnEndExercise { get; } = new Signal<IExerciseResult>();
		
		public Signal<StartTimeoutVO> OnStartTimeout { get; } = new Signal<StartTimeoutVO>();
		public Signal OnResetTimeout { get; } = new Signal();
		public Signal OnMemorizeTimeOver { get; } = new Signal();
		
		public Signal<int> OnUpdateRuns { get; } = new Signal<int>();
		
		public Signal<ITimerUpdateVO> OnUpdateTime { get; } = new Signal<ITimerUpdateVO>();

		public Signal<HUDLevelInfo> OnInitLevelHUD { get; } = new Signal<HUDLevelInfo>();
		public Signal<int> OnUpdateScore { get; } = new Signal<int>();

        private bool _isPaused;
        public bool IsPaused {
            get => _isPaused;
            set {
                var wasPaused = _isPaused;
                _isPaused = value; 

                if (wasPaused != _isPaused) {
                    if (_isPaused) {
                        controller?.Suspend();
                    } else {
                        controller?.Resume();
                    }
                }
            }
        }

        public void RoundCreated()
		{
			OnUpdate(new BaseRoundCreatedVO());
		}
		
		public void CheckAnswer(IRoundItem roundItem)
		{
			OnUpdate(new BaseAnswerVO(roundItem));
		}

		public void FeedbackShown()
		{
			OnUpdate(new BaseFeedbackShownVO());
		}

		public void StartMemorize(int timeout)
		{
			OnUpdate(new StartMemorizeVO(timeout));
		}

		public void StopMemorize()
		{
			OnUpdate(new StopMemorizeVO());
		}

		private void OnControllerUpdate(IExerciseControllerUpdateVO vo)
		{
			if (vo is IFinishExerciseVO finishExerciseVo)
			{
				var result = generateExerciseResult.Execute(finishExerciseVo.ExerciseResultVO);
				OnEndExercise.Dispatch(result);
			}
		}

		private void OnUpdate(IExerciseViewUpdateVO vo)
		{
			Controller.OnViewUpdate(vo);
		}
	}
}