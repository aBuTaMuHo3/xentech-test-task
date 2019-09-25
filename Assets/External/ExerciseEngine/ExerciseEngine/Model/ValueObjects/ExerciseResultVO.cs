using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using SynaptikonFramework.Util;

namespace ExerciseEngine.Model.ValueObjects
{
    public class ExerciseResultVO : IExerciseResultVO
    {
        protected IList<FullRoundVO> _history;

        public int GoodRuns { get; protected set; }

        public int BadRuns { get; protected set; }

        public int CurrentDifficulty { get; protected set; }

        public int StartDifficulty { get; protected set; }

        public float MeanDifficulty { get; protected set; }

        public int MaxDifficulty { get; protected set; } 

        public int MinDifficulty { get; protected set; }

        public int FirstErrorDifficulty { get; protected set; }

        public int ReactionTimeAverage { get; protected set; }

        public int ReactionTimeGoodRunAverage { get; protected set; }

        public int ReactionTimeBadRunAverage { get; protected set; }

        public int Score { get; protected set; }

        public float Accuracy { get; protected set; }

        public int BonusScore { get; protected set; }

        public int FinalScore => Score + BonusScore;

        public object RawData => _history;

        public string GameHistory { get; protected set; }

        public long FinishedTimestamp { get; protected set; }

        protected ExerciseResultVO()
        {
        }

            public ExerciseResultVO(int score, float accuracy, int bonusScore, IList<FullRoundVO> history, int currentDifficulty)
        {
            Accuracy = accuracy;
            Score = score;
            BonusScore = bonusScore;
            _history = history;
            ReactionTimeGoodRunAverage = 0;
            ReactionTimeBadRunAverage = 0;
            if(_history != null && history.Any()){
                GoodRuns = _history.Count(x => !x.RoundResult.Any(y => !y.AnswerCorrect));
                BadRuns = _history.Count(x => x.RoundResult.Any(y => !y.AnswerCorrect));
                CurrentDifficulty = currentDifficulty;
                StartDifficulty = _history[0].RoundDataVO.Difficulty;
                MeanDifficulty = (float)_history.Sum(x => x.RoundDataVO.Difficulty) / _history.Count;
                MaxDifficulty = Math.Max(_history.Max(x => x.RoundDataVO.Difficulty), currentDifficulty);
                MinDifficulty = Math.Min(_history.Min(x => x.RoundDataVO.Difficulty), currentDifficulty);
                FirstErrorDifficulty = _history.Any(x => x.RoundResult.Any(y => !y.AnswerCorrect)) ? _history.First(x => x.RoundResult.Any(y => !y.AnswerCorrect)).RoundDataVO.Difficulty : -1;
                ReactionTimeAverage = (int)(_history.Any(x => x.RoundResult != null) ? _history.Sum(x => x.RoundResult.Sum(y => y.ReactionTime.TotalMilliseconds)) / _history.Count : 0);
                if (_history.Any(x => x.RoundResult != null))
                {
                    var sum = (_history.Sum(x => x.RoundResult.Any(y => y.AnswerCorrect) ? x.RoundResult.Sum(y => y.ReactionTime.TotalMilliseconds) : 0));
                    if (sum > 0)
                        ReactionTimeGoodRunAverage = (int)(sum / _history.Count(x => x.RoundResult.Any(y => y.AnswerCorrect)));
                    sum = (_history.Sum(x => x.RoundResult.Any(y => !y.AnswerCorrect) ? x.RoundResult.Sum(y => y.ReactionTime.TotalMilliseconds) : 0));
                    if (sum > 0)
                        ReactionTimeBadRunAverage = (int)(sum / _history.Count(x => x.RoundResult.Any(y => !y.AnswerCorrect)));
                }
                var builder = new StringBuilder();
                foreach (var round in _history)
                {
                    builder.Append(round.RoundResult.Any(y => !y.AnswerCorrect) ? "1" : "0");
                }
                GameHistory = builder.ToString().EncryptZeroOne();
            }else{
                GameHistory = "";
            }
            FinishedTimestamp = TimestampUtil.NowUnixTimestamp();
        }       

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(RawData);
        }
    }
}