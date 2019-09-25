﻿using System.Runtime.CompilerServices;

namespace SynaptikonFramework.Interfaces.DebugLog
{
    public interface ILogger
    {
        void LogMessage(LogLevel level, string message, [CallerFilePath] string caller = "", [CallerLineNumber] int sourceLineNumber = 0);

        bool SendToNNServer { set; }

        // the calling funtion name
        // [CallerMemberName] string memberName = ""
    }
}
