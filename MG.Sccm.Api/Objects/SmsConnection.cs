using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Timers;

namespace MG.Sccm.Api
{
    [Serializable]
    public sealed class SmsConnection : IDisposable
    {
        #region FIELDS/CONSTANTS
        private const int BYTE_BASE = 16;
        private const double TIME_ELASPED = 10800000;   // 3 hours

        private bool _isDisp;
        private Timer _timer;
        private byte[] _secPass;
        private int _origLength;
        private string _user;

        #endregion

        #region PROPERTIES
        [JsonIgnore]
        public ConnectionManagerBase Connection { get; }
        [JsonExtensionData]
        public ExtensionData Data { get; set; }
        [JsonIgnore]
        public bool HasCredentials => !string.IsNullOrEmpty(_user);
        [JsonIgnore]
        public Guid SessionId { get; }
        [JsonIgnore]
        public string UserHostAddress { get; set; }

        #endregion

        #region CONSTRUCTORS
        public SmsConnection()
        {
            _timer = new Timer(TIME_ELASPED);
            _timer.Elapsed += new ElapsedEventHandler(this.OnTimerElapsed);
            this.Data = new ExtensionData();
            this.SessionId = Guid.NewGuid();
        }
        public SmsConnection(ConnectionManagerBase wql)
            : this() => this.Connection = wql;

        #endregion

        #region PUBLIC METHODS
        public bool Connect(string serverName)
        {
            bool result = this.Connection.Connect(serverName);
            if (result)
                _timer.Start();

            return result;
        }
        public bool Connect(string serverName, string userName, string password)
        {
            bool result = this.Connection.Connect(serverName, userName, password);
            if (result)
            {
                _user = userName;
                byte[] realBytes = Encoding.UTF8.GetBytes(password);
                _origLength = realBytes.Length;
                int round = _origLength / BYTE_BASE + 1;

                int newLength = round * BYTE_BASE;
                _secPass = new byte[newLength];
                for (int i = 0; i < _origLength; i++)
                {
                    _secPass[i] = realBytes[i];
                }
                ProtectedMemory.Protect(_secPass, MemoryProtectionScope.SameProcess);

                _timer.Start();
            }
            return result;
        }

        internal NetworkCredential GetConnectingCredential()
        {
            NetworkCredential netCreds = null;
            if (this.HasCredentials)
            {
                byte[] newBytes = new byte[_origLength];
                ProtectedMemory.Unprotect(_secPass, MemoryProtectionScope.SameProcess);
                _secPass.ToList().CopyTo(0, newBytes, 0, _origLength);
                ProtectedMemory.Protect(_secPass, MemoryProtectionScope.SameProcess);
                netCreds = new NetworkCredential(_user, Encoding.UTF8.GetString(newBytes));
            }
            return netCreds;
        }

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
                _timer.Stop();
                _timer.Dispose();
                this.Connection.Close();
                this.Connection.Dispose();
                GC.SuppressFinalize(this.Connection);
                GC.SuppressFinalize(this);
                _isDisp = true;
            }
        }

        #endregion

        #region BACKEND/PRIVATE METHODS


        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => SmsConnectionManager.ConnectedSessions.Remove(this);

        #endregion
    }
}