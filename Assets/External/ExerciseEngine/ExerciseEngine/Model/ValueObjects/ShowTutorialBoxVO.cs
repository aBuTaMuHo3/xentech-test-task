using System;
using ExerciseEngine.Controller.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    public class ShowTutorialBoxVO : IExerciseControllerUpdateVO
    {
        private string _text;
        private bool _animated;
        private bool _useHtml;
        private float _bottomPos;

        public ShowTutorialBoxVO(string text, bool animated = true, bool useHtml = false, float bottomPos = float.NaN)
        {
            this._bottomPos = bottomPos;
            this._useHtml = useHtml;
            this._animated = animated;
            this._text = text;
        }

        public string text
        {
            get
            {
                return _text;
            }
        }

        public bool animated
        {
            get
            {
                return _animated;
            }
        }

        public bool useHtml
        {
            get
            {
                return _useHtml;
            }
        }

        public float bottomPos
        {
            get
            {
                return _bottomPos;
            }
        }
    }
}