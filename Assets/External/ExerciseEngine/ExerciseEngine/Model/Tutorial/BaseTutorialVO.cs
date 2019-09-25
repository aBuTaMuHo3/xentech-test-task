using System;
using System.Collections.Generic;
using ExerciseEngine.Model.Tutorial.Interfaces;

namespace ExerciseEngine.Model.Tutorial
{
    public class BaseTutorialVO : ITutorialVO
    {
        public string Message { get; }
        public Dictionary<string, string> MessageVariables { get; }
        public bool Confirm { get; }

        public BaseTutorialVO(string message, Dictionary<string, string> messageVariables, bool confirm)
        {
            Message = message;
            MessageVariables = messageVariables;
            Confirm = confirm;
        }

        public BaseTutorialVO(string message, bool confirm = false): this (message, default(Dictionary<string, string>), confirm)
        {
    
        }

        public BaseTutorialVO(string message, Dictionary<string, string> messageVariables) : this(message, messageVariables, false)
        {

        }

    }
}
