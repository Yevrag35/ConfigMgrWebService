using ConfigMgr.Enums;
using ConfigMgr.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace ConfigMgr.WebApi
{
    public abstract class EventController : ApiController
    {
        protected private static Stopwatch _timer = new Stopwatch();

        private const string EVENT_BEG_TRIGGER_MSG = "Web API {0} was triggered from {1} by {2}";
        private const string EVENT_END_TRIGGER_MSG = "Web API {0} completed.  Elapsed time: {1}";
        public const string EVENT_LOG_SOURCE = "ConfigMgr Web Service";
        public const string EVENT_LOG_SOURCE_ACT = EVENT_LOG_SOURCE + " Activity";
        private const EventLogEntryType ID_DEF = EventLogEntryType.Information;
        private const int ID_INFO = 1000;
        private const int ID_WARN = 1001;
        private const int ID_ERR = 1002;
        //private const string ELAPSED_FORMAT = "{0:00}:{1:00}:{2:00}";

        public event LogTriggerEventHandler LogTriggered;

        protected private void TriggerLog(object sender, LogTriggerEventArgs e)
        {
            if (e.Action == LogTriggerAction.Begin)
                this.MethodBegin(e.Method);

            else
                this.MethodEnd(e.Method);
        }

        private void OnLogTriggered(LogTriggerEventArgs e)
        {
            if (this.LogTriggered != null)
                this.LogTriggered(this, e);
        }
        protected private void OnLogTriggered(LogTriggerAction action, MethodBase method)
        {
            this.OnLogTriggered(new LogTriggerEventArgs(action, method));
        }

        public void MethodBegin(MethodBase methodBase)
        {
            _timer.Start();
            string userName = HttpContext.Current.User.Identity.Name;
            string hostAddress = HttpContext.Current.Request.UserHostAddress;
            string msg = string.Format(EVENT_BEG_TRIGGER_MSG, methodBase.Name, hostAddress, userName);
            this.WriteEventLog(msg);
        }
        public void MethodEnd(MethodBase methodBase)
        {
            TimeSpan ts = this.StopTimer();
            string elapsedTime = ts.ToString();
            //string elapsedTime = string.Format(ELAPSED_FORMAT, ts.Hours, ts.Minutes, ts.Seconds);
            string endMsg = string.Format(EVENT_END_TRIGGER_MSG, methodBase.Name, elapsedTime);
            this.WriteEventLog(endMsg);
        }

        private TimeSpan StopTimer()
        {
            _timer.Stop();
            TimeSpan timeSpan = _timer.Elapsed;
            _timer.Reset();

            return timeSpan;
        }

        private void WriteEventLog(string logEntry, EventLogEntryType entryType = ID_DEF)
        {
            using (var eventLog = new EventLog
            {
                Source = EVENT_LOG_SOURCE,
                Log = EVENT_LOG_SOURCE_ACT
            })
            {
                if (!EventLog.SourceExists(EVENT_LOG_SOURCE))
                    EventLog.CreateEventSource(EVENT_LOG_SOURCE, EVENT_LOG_SOURCE_ACT);

                int eventNumber;
                switch (entryType)
                {
                    case EventLogEntryType.Warning:
                        eventNumber = ID_WARN;
                        break;
                    case EventLogEntryType.Error:
                        eventNumber = ID_ERR;
                        break;
                    default:
                        eventNumber = ID_INFO;
                        break;
                }
                eventLog.WriteEntry(logEntry, entryType, eventNumber);
            }
        }
    }
}
