using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.Management;

namespace MG.Sccm.Api
{
    public class SmsSite : BaseWmiObject
    {
        #region PUBLIC PROPERTIES
        [SccmInteger]
        public int BuildNumber { get; set; }
        [SccmString]
        public string ContentLibraryLocation { get; set; }
        [SccmInteger]
        public int ContentLibraryMoveProgress { get; set; }
        [SccmInteger]
        public int ContentLibraryStatus { get; set; }
        public HealthStatus HealthStatus { get; set; }
        [SccmString("InstallDir")]
        public string InstallDirectory { get; set; }
        [SccmInteger]
        public SiteMode Mode { get; set; }
        [SccmString("ReportingSiteCode")]
        public string ParentSiteCode { get; set; }
        /// <summary>
        /// Value indicating a request for secondary site status.  This is actually an enumeration with long definitions.
        /// <see cref="https://docs.microsoft.com/en-us/sccm/develop/reference/core/servers/configure/sms_site-server-wmi-class#properties"/>
        /// </summary>
        [SccmInteger]
        public int SecondarySiteStatus { get; set; }
        [SccmInteger]
        public int SecondarySiteCMUpdateStatus { get; set; }
        /// <summary>
        /// The server name of the site SMS is installed on.
        /// </summary>
        [SccmString]
        public string ServerName { get; set; }
        [SccmString]
        public string SiteCode { get; set; }
        [SccmString]
        public string SiteName { get; set; }
        [SccmInteger]
        public SiteStatus Status { get; set; }
        [SccmInteger]
        public SiteType Type { get; set; }
        [SccmString]
        public string Version { get; set; }

        #endregion

        #region CONSTRUCTORS
        public SmsSite() : base() { }
        public SmsSite(ConnectionManagerBase connection)
            : base(connection) { }

        #endregion

        #region METHODS
        private void OnSccmDeserialized()
        {
            if (!string.IsNullOrEmpty(this.SiteCode) && !_isDisp)
            {
                using (IResultObject statusResults = this.Connection.QueryProcessor.ExecuteQuery(
                    string.Format("SELECT Status FROM SMS_SummarizerSiteStatus WHERE SiteCode = \"{0}\"", this.SiteCode)
                ))
                {
                    foreach (IResultObject stat in statusResults)
                    {
                        using (stat)
                        {
                            this.HealthStatus = (HealthStatus)stat["Status"].IntegerValue;
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}