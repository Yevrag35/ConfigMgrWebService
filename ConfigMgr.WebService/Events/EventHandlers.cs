using ConfigMgr.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigMgr.Events
{
    public delegate void LogTriggerEventHandler(object sender, LogTriggerEventArgs e);

    public class LogTriggerEventArgs : EventArgs
    {
        public LogTriggerAction Action { get; }
        public string Method { get; }
        public string UserHostAddress { get; }

        public LogTriggerEventArgs(LogTriggerAction action, string method, string userAddress)
        {
            Action = action;
            Method = method;
            UserHostAddress = userAddress;
        }
    }
}
