using SynaptikonFramework.Interfaces.DebugLog;
using UnityEngine;
using ILogger = SynaptikonFramework.Interfaces.DebugLog.ILogger;

namespace Exercises.Utils
{
	public class DummyLogger : ILogger
	{
		public bool SendToNNServer { get; set; }

		public void LogMessage(LogLevel level, string message, string caller = "", int sourceLineNumber = 0)
		{
		}
	}

	public class NNLogger : ILogger
	{
		private LogLevel _maxLevel;
		public bool SendToNNServer { get; set; }

		public NNLogger(LogLevel maxLevel)
		{
			_maxLevel = maxLevel;
		}

		public void LogMessage(LogLevel level, string message, string caller = "", int sourceLineNumber = 0)
		{
			if (level > _maxLevel)
			{
				return;
			}

			switch (level)
			{
				case LogLevel.Verbose:
					Trace(message, caller, sourceLineNumber);
					break;
				case LogLevel.Informational:
					Info(message, caller, sourceLineNumber);
					break;
				case LogLevel.Warning:
					Warn(message, caller, sourceLineNumber);
					break;
				case LogLevel.Error:
					Error(message, caller, sourceLineNumber);
					break;
				default:
					var output = CreateOutput(level, message, caller, sourceLineNumber);
					Debug.Log(output);
					break;
			}
		}

		private static void Trace(object message, string caller = "", int sourceLineNumber = 0)
		{
			var output = CreateOutput(LogLevel.Verbose, message, caller, sourceLineNumber);

			Debug.Log(output);
		}

		private static void Info(object message, string caller = "", int sourceLineNumber = 0)
		{
			var output = CreateOutput(LogLevel.Informational, message, caller, sourceLineNumber);

			Debug.Log(output);
		}

		private static void Warn(object message, string caller = "", int sourceLineNumber = 0)
		{
			var output = CreateOutput(LogLevel.Warning, message, caller, sourceLineNumber);

			Debug.LogWarning(output);
		}

		private static void Error(object message, string caller = "", int sourceLineNumber = 0)
		{
			var output = CreateOutput(LogLevel.Error, message, caller, sourceLineNumber);

			Debug.LogError(output);
		}

		private static string CreateOutput(LogLevel level, object message, string caller, int sourceLineNumber)
		{
			var temp = caller.Split('/');
			var callingClassName = temp.Length > 0 ? temp[temp.Length - 1] : string.Empty;
			var output = "[" + level + "]" + " " + callingClassName + "(" + sourceLineNumber + "): " + message;

			return output;
		}
	}
}