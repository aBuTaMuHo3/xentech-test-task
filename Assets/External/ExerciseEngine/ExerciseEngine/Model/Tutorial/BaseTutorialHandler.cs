using System;
using ExerciseEngine.Controller;
using ExerciseEngine.Model.Interfaces;
using ExerciseEngine.Model.Tutorial.Interfaces;
using ExerciseEngine.View.Interfaces;

namespace ExerciseEngine.Model.Tutorial
{
    public class BaseTutorialHandler : ITutorialStateHandler
    {
        protected BaseExerciseController _controller;
        protected IExerciseView _view;
        //protected IMessageBox _messageBox;
        protected IExerciseModel _model;
        private ITutorialTrigger _trigger;
        public ITutorialTrigger Trigger
        {
            get { return _trigger; }
            set
            {
                _trigger = value;
            }
        }
        public virtual int TutorialLoopIncrease => 1;
        public virtual int Loops => 1;
        public bool Initialised { get; private set; }

        public BaseTutorialHandler()
        {
        }

        
        public virtual void Init(IExerciseModel model, BaseExerciseController controller, IExerciseView view)
        {
            _controller = controller;
            //_messageBox = messageBox;
            _view = view;
            _model = model;
            Initialised = true;
        }

        public virtual void AfterStateCorrectAnswer()
        {
            _controller.AfterStateCorrectAnswer();
        }

        public virtual void AfterStateCorrectStep()
        {
            _controller.AfterStateCorrectStep();
        }

        public virtual void AfterStateInput()
        {
            _controller.AfterStateInput();
        }

        public virtual void AfterStateMemorize()
        {
            _controller.AfterStateMemorize();
        }

        public virtual void AfterStateShowRound()
        {
            _controller.AfterStateShowRound();
        }

        public virtual void AfterStateWrongAnswer()
        {
            _controller.AfterStateWrongAnswer();
        }

        public virtual void AfterStateWrongStep()
        {
            _controller.AfterStateWrongStep();
        }

        public virtual void OnStateCorrectAnswer()
        {
            _controller.OnStateCorrectAnswer();
        }

        public virtual void OnStateCorrectStep()
        {
            _controller.OnStateCorrectStep();
        }

        public virtual void OnStateInput()
        {
            _controller.OnStateInput();
        }

        public virtual void OnStateMemorize()
        {
            _controller.OnStateMemorize();
        }

        public virtual void OnStateShowRound()
        {
            _controller.OnStateShowRound();
        }

        public virtual void OnStateWrongAnswer()
        {
            _controller.OnStateWrongAnswer();
        }

        public virtual void OnStateWrongStep()
        {
            _controller.OnStateWrongStep();
        }
        
        public virtual void Dispose()
        {
            _controller = null;
            //_messageBox = null;
            _view = null;
            _model = null;
        }
    }
}
