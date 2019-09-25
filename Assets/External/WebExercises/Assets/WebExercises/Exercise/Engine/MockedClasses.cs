using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exercises.ExerciseA;
using ExerciseEngine.HUD.Interfaces;
using ExerciseEngine.HUD.ValueObjects.Interfaces;
using ExerciseEngine.Sound;
using ExerciseEngine.Sound.Interfaces;
using ExerciseEngine.View.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;
using SynaptikonFramework.Interfaces.DebugLog;
using SynaptikonFramework.Interfaces.Language;
using SynaptikonFramework.Interfaces.Util.Timer;
using SynaptikonFramework.Util.Math;
using UnityEngine;
using ILogger = SynaptikonFramework.Interfaces.DebugLog.ILogger;

namespace Exercises.Exercises
{
//    public class FallbackBackground : IExerciseBackgroundView
//    {
//        private readonly ILogger _logger;
//
//        public FallbackBackground(ILogger logger)
//        {
//            _logger = logger;
//        }
//
//        public void Dispose()
//        {
//            _logger.LogMessage(LogLevel.Warning, "Background should Dispose now");
//        }
//
//        public void LevelUp(Action callback = null)
//        {
//            throw new NotImplementedException();
//        }
//
//        public void LevelDown(Action callback = null)
//        {
//            throw new NotImplementedException();
//        }
//
//        public void ShowCorrectAnswer(Action callback = null)
//        {
//            throw new NotImplementedException();
//        }
//
//        public void ShowWrongAnswer(Action callback = null)
//        {
//            throw new NotImplementedException();
//        }
//
//        public void SetMultiplier(int mulitiplierLevel)
//        {
//            throw new NotImplementedException();
//        }
//
//        public void Tap(Vector2D point)
//        {
//            throw new NotImplementedException();
//        }
//
//        public void EnableBackground(bool enabled)
//        {
//            throw new NotImplementedException();
//        }
//
//        public void ToggleGradient()
//        {
//            throw new NotImplementedException();
//        }
//    }
//
//    public class FallbackHUD : IExerciseHUD
//    {
//        private readonly ILogger _logger;
//
//        public FallbackHUD(ILogger logger)
//        {
//            _logger = logger;
//        }
//
//        public IExercisePaddings Paddings
//        {
//            set { throw new NotImplementedException(); }
//        }
//
//        public IExercisePaddings Insets
//        {
//            get { throw new NotImplementedException(); }
//        }
//
//        public IHudView View
//        {
//            get { throw new NotImplementedException(); }
//        }
//
//        public event HudUpdateHandler OnUpdate;
//
//        public void Dispose()
//        {
//            _logger.LogMessage(LogLevel.Warning, "HUD should Dispose now");
//        }
//
//        public void Update(List<IExerciseHudVO> data)
//        {
//            //string str = "HUD should Update now ";
//            //if(data != null){
//            //    foreach(var u in data){
//            //        str += u.ToString() +" ";
//            //    }
//            //}
//            //_logger.LogMessage(LogLevel.Warning, str);
//        }
//    }
    
    public class LanguageManager : ILanguage
    {
        public string GetString(string id)
        {
            return id;
        }

        public string GetString(string id, Dictionary<string, string> variables)
        {
            return id + " " + variables.ToString();
        }
    }

    public class ExerciseSoundManager : ISoundManager
    {
        protected readonly ILogger _logger;
        public bool Mute { get; set; }

        public ExerciseSoundManager(ILogger logger)
        {
            _logger = logger;
        }

        protected virtual string GlobalSoundBaseName
        {
            get { return "ExerciseEngine.Xamarin.Resources"; }
        }

        protected virtual string[] GlobalSoundNames
        {
            get
            {
                return new string[]
                {
                    AudioEffect.Correct1,
                    AudioEffect.Correct2,
                    AudioEffect.LevelDown,
                    AudioEffect.LevelUp,
                    AudioEffect.StartGame,
                    AudioEffect.StopGame,
                    AudioEffect.Timeout,
                    AudioEffect.Wrong1,
                    AudioEffect.Wrong2,
                    AudioEffect.Wrong3
                };
            }
        }


        public async Task InitAsync()
        {
        }

        public virtual void PlaySound(string soundID)
        {
            //_logger.LogMessage(LogLevel.Informational, "Playing sound " + soundID);
        }

        public virtual void StopSound(string soundID)
        {
            //_logger.LogMessage(LogLevel.Informational, "Stopping sound " + soundID);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // stop the GC clearing us up, 
        }

        protected virtual void Dispose(bool disposing)
        {
            //_logger.LogMessage(LogLevel.Informational, "Disposing soundmanager " + disposing);
        }
    }
    
    public class NNUnityTimerFactory : ITimerFactory
    {
        private int count;
        
        public ITimer CreateTimer()
        {
            var go = new GameObject {name = "timer_" + count++};

            return go.AddComponent<NNUnityTimer>();
        }
    }

}