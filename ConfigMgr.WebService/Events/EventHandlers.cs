using ConfigMgr.Enums;
using System;
using System.Reflection;

namespace ConfigMgr.Events
{
    public delegate void LogTriggerEventHandler(object sender, LogTriggerEventArgs e);

    public class LogTriggerEventArgs : EventArgs
    {
        public LogTriggerAction Action { get; }
        public MethodBase Method { get; }
        public string UserHostAddress { get; }
        public string UserName { get; }

        public LogTriggerEventArgs(LogTriggerAction action, MethodBase method, string userAddress, string userName)
            : this(action, method)
        {
            UserHostAddress = userAddress;
            UserName = userName;
        }

        public LogTriggerEventArgs(LogTriggerAction action, MethodBase method)
        {
            Action = action;
            Method = method;
        }
    }
}
