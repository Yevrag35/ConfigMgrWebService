using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MG.Sccm.Api
{
    public abstract class BaseWmiObject : IDisposable, IOutputToJson
    {
        #region FIELDS/CONSTANTS
        protected const string Q_SELECT_ALL = "SELECT * FROM {0}";
        protected const string SMS_CLASS = "SMS_{0}";
        internal const string NAMESPACE = "ROOT\\SMS\\site_{0}";

        protected private bool _isDisp = false;

        #endregion

        #region PROPERTIES
        [JsonIgnore]
        protected ConnectionManagerBase Connection { get; }
        [JsonIgnore]
        internal IDictionary<string, JToken> ExtraData { get; set; }

        #endregion

        #region CONSTRUCTORS
        public BaseWmiObject() => this.ExtraData = new Dictionary<string, JToken>();
        public BaseWmiObject(ConnectionManagerBase connection)
            : this() => this.Connection = connection;

        #endregion

        #region PUBLIC METHODS
        public bool ShouldSerializeExtraData() => false;

        /// <summary>
        /// Converts the inheriting class to a JSON-formatted string using programmed serializers.
        /// </summary>
        public virtual string ToJson()
        {
            var converter = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DefaultValueHandling = DefaultValueHandling.Populate,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Error
            };
            converter.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
            return JsonConvert.SerializeObject(this, converter);
        }

        /// <summary>
        /// Converts the inheriting class to a JSON-formatted string using programmed serializers adding in the contents from the specified generic dictionary.
        /// </summary>
        /// <param name="parameters">The dictionary that will have it contents added to resulting JSON string.</param>
        public virtual string ToJson(IDictionary parameters)
        {
            var camel = new CamelCasePropertyNamesContractResolver();
            var cSerialize = new JsonSerializer
            {
                ContractResolver = camel
            };

            var serializer = new JsonSerializerSettings
            {
                ContractResolver = camel,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DefaultValueHandling = DefaultValueHandling.Populate,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Error
            };
            serializer.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));

            var job = JObject.FromObject(this, cSerialize);

            string[] keys = parameters.Keys.Cast<string>().ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                job.Add(key, JToken.FromObject(parameters[key], cSerialize));
            }

            return JsonConvert.SerializeObject(job, serializer);
        }

        #endregion

        #region IDISPOSABLE METHODS
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisp)
                return;

            if (disposing)
                this.Connection.Dispose();

            _isDisp = true;
        }

        #endregion

        #region BACKEND/PRIVATE METHODS


        #endregion
    }
}