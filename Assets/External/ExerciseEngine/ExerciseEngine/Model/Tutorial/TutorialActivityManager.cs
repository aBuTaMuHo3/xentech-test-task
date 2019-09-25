using System;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using SynaptikonFramework.Interfaces.DebugLog;

namespace ExerciseEngine.Model.Tutorial
{
    public class TutorialActivityManager
    {
        public bool TutorialActive { get; protected set; }
        public bool TutorialSystemEnabled { get; }

        protected readonly int _badRunsToActivate;
        protected readonly int _goodRunsToDisable;
        protected readonly ILogger _logger;
        protected int _badRunsInARow;
        protected int _goodRunsInARow;
        private bool _roundHasWrongStep = false;

        public TutorialActivityManager(ILogger logger, int badRunsToActivate, int goodRunsToDisable, bool tutorialActive, bool tutorialSystemEnabled = false)
        {
            _badRunsToActivate = badRunsToActivate;
            _goodRunsToDisable = goodRunsToDisable;
            _logger = logger;
            TutorialActive = tutorialActive && tutorialSystemEnabled;
            TutorialSystemEnabled = tutorialSystemEnabled;
            _badRunsInARow = 0;
            _goodRunsInARow = 0;
        }

        public void EvaluateResult(IRoundEvaluationResultVO result)
        {
            if (TutorialSystemEnabled)
            {
                if (result.RoundCompleted)

                {
                    if (result.AnswerCorrect && !_roundHasWrongStep)
                    {
                        _goodRunsInARow++;
                        _badRunsInARow = 0;
                        if (TutorialActive && _goodRunsInARow >= _goodRunsToDisable)
                        {

                            _goodRunsInARow = 0;
                            TutorialActive = false;
                            _logger.LogMessage(LogLevel.Informational, "Tutorial disactivated");
                        }
                    }
                    else
                    {
                        _badRunsInARow++;
                        _goodRunsInARow = 0;
                        if (!TutorialActive && _badRunsInARow >= _badRunsToActivate)
                        {
                            _badRunsInARow = 0;
                            TutorialActive = true;
                            _logger.LogMessage(LogLevel.Informational, "Tutorial activated");
                        }
                    }
                    _roundHasWrongStep = false;
                }
                else
                {
                    if (!result.AnswerCorrect)
                    {
                        _roundHasWrongStep = true;
                    }
                }
            }
        }

        public void ActivateTutorail(){
            if(TutorialSystemEnabled){
                TutorialActive = true;
                _badRunsInARow = 0;
                _goodRunsInARow = 0;
            }
        }
    }
}