using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Serialization;

namespace MG.Sccm.Api
{
    [Serializable]
    public sealed class SmsConnection : IDisposable
    {
        #region FIELDS/CONSTANTS
        private bool _isDisp;

        #endregion

        #region PROPERTIES
        [JsonIgnore]
        public ConnectionManagerBase Connection { get; }
        [JsonExtensionData]
        public ExtensionData Data { get; set; }
        [JsonIgnore]
        public Guid SessionId { get; }
        [JsonIgnore]
        public string UserHostAddress { get; set; }

        #endregion

        #region CONSTRUCTORS
        public SmsConnection()
        {
            this.Data = new ExtensionData();
            this.SessionId = Guid.NewGuid();
        }
        public SmsConnection(ConnectionManagerBase wql)
            : this() => this.Connection = wql;

        #endregion

        #region PUBLIC METHODS
        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            this.Data.Clear();
            var dict = new Dictionary<string, JToken>
            {
                { "allProviderLocations", JToken.FromObject(this.Connection.NamedValueDictionary["AllProviderLocations"]) },
                { "buildNumber", JToken.FromObject(this.Connection.NamedValueDictionary["BuildNumber"]) },
                { "connectedServerVersion", JToken.FromObject(this.Connection.NamedValueDictionary["ConnectedServerVersion"]) },
                { "connectedSiteCode", JToken.FromObject(this.Connection.NamedValueDictionary["ConnectedSiteCode"]) },
                { "connectedSiteVersion", JToken.FromObject(this.Connection.NamedValueDictionary["ConnectedSiteVersion"]) },
                { "connectedPath", JToken.FromObject(this.Connection.NamedValueDictionary["Connection"]) },
                { "providerLocation", JToken.FromObject(((ManagementBaseObject)this.Connection.NamedValueDictionary["ProviderLocation"])["__PATH"]) },
                { "serverName", JToken.FromObject(this.Connection.NamedValueDictionary["ServerName"]) },
                { "sessionId", JToken.FromObject(this.SessionId) },
                { "siteName", JToken.FromObject(this.Connection.NamedValueDictionary["SiteName"]) },
                { "supportId", JToken.FromObject(this.Connection.NamedValueDictionary["SupportID"]) },
                { "userHostAddress", JToken.FromObject(this.UserHostAddress) }
            };
            this.Data.AddPairs(dict);
        }

        public void Dispose()
        {
            if (!_isDisp)
            {
                this.Connection.Close();
                this.Connection.Dispose();
                GC.SuppressFinalize(this.Connection);
                GC.SuppressFinalize(this);
                _isDisp = true;
            }
        }

        #endregion

        #region BACKEND/PRIVATE METHODS


        #endregion
    }
}