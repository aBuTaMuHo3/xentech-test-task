using System;
using System.Collections.Generic;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using Memoflow.Model.ValueObjects;
using MinLibs.MVC;
using MinLibs.Signals;
using WebExercises.Exercise;
using WebExercises.Shared;

namespace WebExercises.Memoflow
{
	public interface IMemoflowUI : IMediatedBehaviour
	{
		Signal OnOk { get; }
		Signal OnYes { get; }
		Signal OnNo { get; }
		void ShowIntro(string infoText, string okText);
		void ShowRound(string infoText, string yesText, string noText);
	}
	
	public class MemoflowUIMediator : Mediator<IMemoflowUI>
	{
		[Inject] public IExerciseControllerProxy controller;
		[Inject] public OnFinishInitialRound onFinishInitialRound;
		[Inject] public IHotKeys hotKeys;
		[Inject] public ILocalization localization;

		protected override void Register()
		{
			base.Register();
			
			signals.Register(controller.OnCreateInitialRound, CreateInitialRound);
			
			signals.Register(mediated.OnOk, OnOk);
			signals.Register(mediated.OnYes, OnYes);
			signals.Register(mediated.OnNo, OnNo);
		}

		private void OnNo()
		{
			CheckAnswer(false);
		}

		private void OnYes()
		{
			CheckAnswer(true);
		}

		private void CheckAnswer(bool isCorrect)
		{
			var vo = new MemoFlowAnswerVO(isCorrect);
			controller.CheckAnswer(vo);
		}

		private void OnOk()
		{
			hotKeys.KeyActions = new Dictionary<string, Action>
			{
				{ GetHotKey(TextKeys.YES), OnYes },
				{ GetHotKey(TextKeys.NO), OnNo },
				{ "left", OnYes },
				{ "right", OnNo },
			};

			var question = localization.Get(TextKeys.MF_QUESTION);
			var yes = localization.Get(TextKeys.YES);
			var no = localization.Get(TextKeys.NO);
			mediated.ShowRound(question, yes, no);
			onFinishInitialRound.Dispatch();
			controller.RoundCreated();
		}

		private string GetHotKey(string key)
		{
			var text = localization.Get(key);
			var firstChar = text.Substring(0, 1).ToLower();

			return firstChar;
		}

		private void CreateInitialRound(IExerciseRoundDataVO dataVO)
		{
			hotKeys.KeyActions = new Dictionary<string, Action>
			{
				{ "space", OnOk },
			};
			
			var advice = localization.Get(TextKeys.MF_ADVICE);
			var ok = localization.Get(TextKeys.OK);
			mediated.ShowIntro(advice, ok);
		}
	}
}