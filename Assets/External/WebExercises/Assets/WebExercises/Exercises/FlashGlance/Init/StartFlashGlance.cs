using ExerciseEngine.Colors;
using ExerciseEngine.Controller;
using ExerciseEngine.Model.Interfaces;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Terminator.Interfaces;
using Exercises.Exercises;
using Exercises.Utils;
using FlashGlance.Controller;
using FlashGlance.Model;
using MinLibs.MVC;
using WebExercises.Exercise;
using ILogger = SynaptikonFramework.Interfaces.DebugLog.ILogger;

namespace WebExercises.FlashGlance
{
    public class StartFlashGlance : StartExercise<FlashGlanceConfiguration>
    {
        [Inject] public IColorManagerInitializer colorManager;

        protected override IExerciseModel CreateModel(ExerciseInitVO initVO, IExerciseConfiguration config, ILogger logger)
        {
            return new FlashGlanceModel(config, initVO, logger, colorManager);
        }

        protected override BaseExerciseController CreateController(NNLogger logger, IExerciseModel model, IExerciseTerminator terminator,
            NNUnityTimerFactory timerFactory, ExerciseSoundManager soundManager)
        {
            return new FlashGlanceController(model, exerciseViewAdapter, backgroundViewAdapter, hudViewAdapter, timerFactory, terminator, logger, soundManager);
        }
    }
}