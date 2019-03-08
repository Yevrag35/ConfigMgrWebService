using ConfigMgr.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace ConfigMgr
{
    public class Helper : ITriggerLog
    {
        private const string EVENT_LOG_SOURCE = "ConfigMgr Web Service";
        private const string EVENT_LOG_SOURCE_ACT = EVENT_LOG_SOURCE + " Activity";
        private const EventLogEntryType ID_DEF = EventLogEntryType.Information;
        private const int ID_INFO = 1000;
        private const int ID_WARN = 1001;
        private const int ID_ERR = 1002;

        private Stopwatch _timer;

        public Helper()
        {
            _timer = new Stopwatch();
        }

        #region ITriggerLog Methods
        public void StartTimer() => _timer.Start();

        #endregion

        public string ConvertFromSecureString(SecureString ss)
        {
            IntPtr pPoint = Marshal.SecureStringToBSTR(ss);
            string plain = Marshal.PtrToStringAuto(pPoint);
            Marshal.ZeroFreeBSTR(pPoint);
            return plain;
        }

        public int FindNextNumber(List<int> list)
        {
            list.Sort();
            int lastNumber = list.Last();
            int nextNumber = lastNumber + 1;

            return nextNumber;
        }

        public bool IsNullOrEmpty<T>(IEnumerable<T> enumerable) =>
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
