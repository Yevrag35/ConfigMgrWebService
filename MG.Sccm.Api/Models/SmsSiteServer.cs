using Microsoft.ConfigurationManagement.ManagementProvider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace MG.Sccm.Api.Models
{
    public class SmsSiteServer : BaseWmiObject
    {
        #region FIELDS/CONSTANTS
        //private const string TZ_QUERY = "SELECT StandardName FROM Win32_TimeZone";

        #endregion

        #region PROPERTIES
        [SccmBool]
        public bool InternetEnabled { get; set; }
        [SccmBool]
        public bool InternetShared { get; set; }
        [JsonIgnore]
        [SccmString]
        public string NALPath { get; set; }
        [SccmString]
        public string ResourceType { get; set; }
        [JsonIgnore]
        [SccmString("ServerName")]
        public string Name { get; set; }
        [SccmString]
        public string RoleName { get; set; }
        [SccmString("ServerRemoteName")]
        public string RemoteName { get; set; }
        [SccmString]
        public string SiteCode { get; set; }
        [SccmInteger("SslState")]
        public int SSLState { get; set; }
        //public string TimeZone { get; private set; }

        #endregion

        #region CONSTRUCTORS
        public SmsSiteServer()
            : base()
        {
        }

        public SmsSiteServer(ConnectionManagerBase wql)
            : base(wql)
        {
        }

        #endregion

        #region PUBLIC METHODS


        #endregion

        #region BACKEND/PRIVATE METHODS
        //private void OnSccmDeserialized(SmsConnection connection)
        //{
        //    var opts = new ConnectionOptions();
        //    if (connection.HasCredentials)
        //    {
        //        var creds = connection.GetConnectingCredential();
        //        opts.Username = creds.UserName;
        //        opts.Password = creds.Password;
        //    }

        //    var scope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", this.Name), opts);
        //    TimeZoneInfo tz = null;
        //    try
        //    {
        //        scope.Connect();
        //    }
        //    catch
        //    {
        //        return;
        //    }

        //    var query = new ObjectQuery(TZ_QUERY);

        //    using (var searcher = new ManagementObjectSearcher(scope, query))
        //    {
        //        ManagementObjectCollection objCol = searcher.Get();
        //        foreach (ManagementBaseObject mbo in objCol)
        //        {
        //            using (mbo)
        //            {
        //                tz = TimeZoneInfo.FindSystemTimeZoneById(mbo["StandardName"] as string);
        //            }
        //            break;
        //        }
        //    }
        //    this.TimeZone = tz.Id;
        //}

        #endregion
    }

    internal class SiteServerEquality : IEqualityComparer<SmsSiteServer>
    {
        public bool Equals(SmsSiteServer x, SmsSiteServer y) => x.Name.Equals(y.Name);
        public int GetHashCode(SmsSiteServer x) => x.GetHashCode();
    }
}