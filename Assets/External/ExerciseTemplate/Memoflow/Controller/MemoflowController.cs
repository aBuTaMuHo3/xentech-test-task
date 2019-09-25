using ExerciseEngine.Controller;
using ExerciseEngine.HUD.Interfaces;
using ExerciseEngine.Model.Interfaces;
using ExerciseEngine.Model.Tutorial;
using ExerciseEngine.Model.Tutorial.Interfaces;
using ExerciseEngine.Sound.Interfaces;
using ExerciseEngine.Terminator.Interfaces;
using ExerciseEngine.View.Interfaces;
using SynaptikonFramework.Interfaces.DebugLog;
using SynaptikonFramework.Interfaces.Util.Timer;

namespace Memoflow.Controller
{
    public class MemoflowController : BaseExerciseController
    {
        public MemoflowController(IExerciseModel exerciseModel, IExerciseView exerciseView, IExerciseBackgroundView exerciseBackgroundView, IExerciseHUD hud, ITimerFactory timerFactory, IExerciseTerminator terminator, ILogger logger, ISoundManager soundManager) : base(exerciseModel, exerciseView, exerciseBackgroundView, hud, timerFactory, terminator, logger, soundManager)
        {
        }

        public override ITutorialVO GeneralTutorialText() {
            return new BaseTutorialVO("Memoflow_instruction_text");
        }
    }
}
