using ExerciseEngine.Controller;
using ExerciseEngine.Model.Interfaces;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Terminator.Interfaces;
using ExerciseEngine.View.ValueObjects;
using Exercises.Exercises;
using Exercises.Utils;
using Memoflow.Controller;
using Memoflow.Model;
using MinLibs.MVC;
using SynaptikonFramework.Interfaces.DebugLog;
using WebExercises.Exercise;
using ExerciseEngine.Colors;

namespace WebExercises.Memoflow
{
	public class StartMemoflow : StartExercise<MemoflowConfiguration>
	{
        [Inject] public IColorManagerInitializer colorManager;

        protected override void SetInitialModelData(MemoflowConfiguration config)
		{
			SetInitialModelData(new NumAssetUpdate(config.MaxSymbols));
		}

		protected override IExerciseModel CreateModel(ExerciseInitVO initVO, IExerciseConfiguration config,
			ILogger logger)
		{
			return new MemoflowModel(config, initVO, logger, colorManager);
		}

		protected override BaseExerciseController CreateController(NNLogger logger, IExerciseModel model,
			IExerciseTerminator terminator, NNUnityTimerFactory timerFactory, ExerciseSoundManager soundManager)
		{
			return new MemoflowController(model, exerciseViewAdapter, backgroundViewAdapter, hudViewAdapter,
				timerFactory, terminator, logger, soundManager);
		}
	}
}