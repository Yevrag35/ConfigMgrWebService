using ConfigMgr.Enums;
using ConfigMgr.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Web;

namespace ConfigMgr
{
    public class Helper : ITriggerLog
    {
        private const string EVENT_BEG_TRIGGER_MSG = "Web service method {0} was triggered from {1} by {2}";
        private const string EVENT_END_TRIGGER_MSG = "Web service method {0} completed.  Elapsed time: {1}";
        public const string EVENT_LOG_SOURCE = "ConfigMgr Web Service";
        public const string EVENT_LOG_SOURCE_ACT = EVENT_LOG_SOURCE + " Activity";
        private const EventLogEntryType ID_DEF = EventLogEntryType.Information;
        private const int ID_INFO = 1000;
        private const int ID_WARN = 1001;
        private const int ID_ERR = 1002;
        private const string ELAPSED_FORMAT = "{0:00}:{1:00}:{2:00}";

        private Stopwatch _timer;

        public event LogTriggerEventHandler LogTriggered;

        public Helper() => _timer = new Stopwatch();

        #region ITriggerLog Methods
        private void OnLogTriggered(LogTriggerEventArgs e)
        {
            if (this.LogTriggered != null)
                this.LogTriggered(this, e);
        }
        public void OnLogTriggered(LogTriggerAction action, MethodBase method, string address, string userName)
        {
            this.OnLogTriggered(new LogTriggerEventArgs(action, method, address, userName));
        }
        public void OnLogTriggered(LogTriggerAction action, MethodBase method)
        {
            this.OnLogTriggered(new LogTriggerEventArgs(action, method));
        }

        private void StartTimer() => _timer.Start();

        private TimeSpan StopTimer()
        {
            var timeSpan = _timer.Elapsed;
            _timer.Reset();

            return timeSpan;
        }

        public void MethodBegin(MethodBase methodBase, string userHostAddress, string userName)
        {
            this.StartTimer();
            string msg = string.Format(EVENT_BEG_TRIGGER_MSG, methodBase.Name, userHostAddress, userName);
            this.WriteEventLog(msg);
        }

        public void MethodEnd(MethodBase methodBase)
        {
            TimeSpan ts = this.StopTimer();
            string elapsedTime = string.Format(ELAPSED_FORMAT, ts.Hours, ts.Minutes, ts.Seconds);
            string endMsg = string.Format(EVENT_END_TRIGGER_MSG, methodBase.Name, elapsedTime);
            this.WriteEventLog(endMsg);
        }

        #endregion

        public static string ConvertFromSecureString(SecureString ss)
        {
            IntPtr pPoint = Marshal.SecureStringToBSTR(ss);
            string plain = Marshal.PtrToStringAuto(pPoint);
            Marshal.ZeroFreeBSTR(pPoint);
            return plain;
        }

        public static int? FindMissingNumber(List<int> list)
        {
            int? firstMissingNumber = null;

            list.Sort();

            int firstNumber = list.First();
            int lastNumber = list.Last();

            if (firstNumber == 1 && lastNumber == 1)
                firstMissingNumber = 2;

            else
            {
                IEnumerable<int> range = Enumerable.Range(firstNumber, lastNumber - firstNumber);

                if (!range.Contains(1))
                    firstMissingNumber = 1;

                else if (range != null)
                {
                    IEnumerable<int> setDifferences = range.Except(list);
                    firstMissingNumber = !IsNullOrEmpty(setDifferences) ? range.Except(list).First() : FindNextNumber(list);
                }
            }
            return firstMissingNumber;
        }

        public static int FindNextNumber(List<int> list)
        {
            list.Sort();
            int lastNumber = list.Last();
            int nextNumber = lastNumber + 1;

            return nextNumber;
        }

        public static bool IsNullOrEmpty<T>(IEnumerable<T> enumerable) =>
            enumerable == null || !enumerable.Any();

        public void WriteEventLog(string logEntry, EventLogEntryType entryType = ID_DEF)
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
