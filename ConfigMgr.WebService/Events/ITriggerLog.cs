using System;
using System.Diagnostics;
using System.Reflection;

namespace ConfigMgr.Events
{
    public interface ITriggerLog
    {
        event LogTriggerEventHandler LogTriggered;

        void MethodBegin(MethodBase methodBase);
        void MethodEnd(MethodBase methodBase);

        void StartTimer();
        TimeSpan StopTimer();
    }
}
