using System;
using System.Diagnostics;
using System.Reflection;
using System.Web;

namespace ConfigMgr.Events
{
    public interface ITriggerLog
    {
        event LogTriggerEventHandler LogTriggered;

        void MethodBegin(MethodBase methodBase, string userHostAddress, string userName);
        void MethodEnd(MethodBase methodBase);
    }
}
