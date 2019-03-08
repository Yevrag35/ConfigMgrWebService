using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;

namespace ConfigMgr
{
    public class SmsProvider
    {
        public WqlConnectionManager Connect(string serverName)
        {
            try
            {
                //' Connect to SMS Provider
                var namedValues = new SmsNamedValuesDictionary();
                var connection = new WqlConnectionManager(namedValues);
                connection.Connect(serverName);

                return connection;
            }
            catch (SmsException ex)
            {
                ConfigMgr.WriteEventLog(string.Format("Unhandled expection thrown by SMS Provider: {0}", ex.Message), EventLogEntryType.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                ConfigMgr.WriteEventLog(string.Format("Unathorized access exception thrown: {0}", ex.Message), EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                ConfigMgr.WriteEventLog(string.Format("Unhandled expection thrown: {0}", ex.Message), EventLogEntryType.Error);
            }

            return null;
        }
    }
}