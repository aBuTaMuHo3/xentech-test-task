using System;
using System.Collections.Generic;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.Tutorial.Interfaces;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.Interfaces
{
    public delegate void TutorialTriggerHandler(ITutorialTrigger vo);

    public interface IExerciseModel : IDisposable
    {
        event TutorialTriggerHandler OnTutorialTrigger;

        /// <summary>
        /// Creates the round for the exercise.
        /// </summary>
        /// <returns>The round.</returns>
        /// <param name="exerciseRoundConfiguration">Exercise round configuration.</param>
        void CreateRound(Action<IExerciseRoundDataVO> callback, IExerciseRoundConfigurationVO exerciseRoundConfiguration = null);

        /// <summary>
        /// Evaluates the answer.
        /// </summary>
        /// <returns>The answer.</returns>
        /// <param name="answer">Answer.</param>
        /// <param name="isTutorial">If set to <c>true</c> is tutorial.</param>
        void EvaluateAnswer(Action<IRoundEvaluationResultVO> callback, IAnswerVO answer, bool isTutorial = false);

        /// <summary>
        /// Updates model with some round independent data.
        /// </summary>
        /// <returns>The independent update.</returns>
        /// <param name="data">Data.</param>
        IRoundIndependentUpdateResultVO RoundIndependentUpdate(IRoundIndependentVO data);

        /// <summary>
        /// Gets the current round.
        /// </summary>
        /// <value>The current round.</value>
        IExerciseRoundDataVO CurrentRound { get; }

        /// <summary>
        /// Gets the current round result.
        /// </summary>
        /// <value>The current round result.</value>
        IRoundEvaluationResultVO CurrentRoundResult { get; }

        /// <summary>
        /// Gets the exercise configuration.
        /// </summary>
        /// <value>The exercise configuration.</value>
        IExerciseConfiguration ExerciseConfiguration { get; }

        /// <summary>
        /// Stop the exercise and returns the exercise resilt.
        /// </summary>
        /// <returns>The exercise resilt.</returns>
        IExerciseResultVO Stop();

        /// <summary>
        /// Gets the bot answer for testing purposes to limulate user giving answer.
        /// </summary>
        /// <value>The bot answer.</value>
        IExerciseViewUpdateVO BotAnswer { get; }

        /// <summary>
        /// Gets the bot round delay.
        /// </summary>
        /// <value>The bot round delay.</value>
        int BotRoundDelay { get; }

        /// <summary>
        /// Saves the input enabled at date.
        /// </summary>
        /// <param name="date">Date.</param>
        void StartMonitorReactionTime(DateTime date);

        ExerciseIdEnum ExerciseId { get; }

        Dictionary<ExerciseSettingsEnum, bool> ExerciseSettings { get; }

        int BonusScore { get; }
        
        bool IsTimeoutEnabled { get; }

        IExerciseInitVO ExerciseInitVO { get; }
    }
}