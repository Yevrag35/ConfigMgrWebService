using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConfigMgrWebService
{
    public abstract class BaseSms : IDisposable
    {
        #region CONSTANTS
        protected private const string Q_SELECT_ALL = "SELECT * FROM {0}";
        protected private const string CLASS_NAME_FORMAT = "SMS_{0}";
        protected private const string Q_WHERE_EQ = Q_SELECT_ALL + " WHERE {1} = '{2}'";
        protected private const string Q_WHERE_LIKE = Q_SELECT_ALL + " WHERE {1} LIKE '{2}'";
        private const string NAMESPACE = "root\\sms\\site_{0}";

        #endregion

        #region ABSTRACT PROPERTIES
        protected abstract string ClassName { get; }
        protected abstract bool HasLazyProperties { get; }
        protected abstract string[] NeedToInvoke { get; }       // Must be parameterless 'void' methods.
        protected abstract KeyValuePair<bool, string> PKMapping { get; }    // Use when primary key doesn't match the class's property name.
        protected abstract string PrimaryKey { get; }

        #endregion

        #region BASE PROPERTIES
        protected ConnectionManagerBase Connection { get; }

        private bool _isDisp = false;

        #endregion

        #region CONSTRUCTORS
        public BaseSms() { }

        public BaseSms(ConnectionManagerBase wql) =>
            this.Connection = wql;

        #endregion

        #region INSTANCE METHODS
        public string GetClassQuery() => string.Format(Q_SELECT_ALL, this.ClassName);

        public string GetInstanceQuery()
        {
            string pk = this.PrimaryKey;
            if (this.PKMapping.Key)
                pk = this.PKMapping.Value;

            PropertyInfo pi = this.GetType().GetProperty(pk);
            object val = pi.GetValue(this, null);

            string returnString = null;
            if (val != null)
            {
                returnString = string.Format(Q_WHERE_EQ, this.ClassName, this.PrimaryKey, val);
            }
            return returnString;
        }

        #endregion

        #region STATIC METHODS
        public static IEnumerable<BaseSms> GatherInstances<T>(ConnectionManagerBase wql) where T : BaseSms
        {
            var list = new List<BaseSms>();
            using (T newObj = Activator.CreateInstance<T>())
            {
                IResultObject queryResults = wql.QueryProcessor.ExecuteQuery(newObj.GetClassQuery());
                foreach (IResultObject ro in queryResults)
                {
                    T generatedObject = MatchTo<T>(ro);
                    list.Add(generatedObject);
                }
            }
            return list;
        }

        private static T MatchTo<T>(IResultObject resObj) where T : BaseSms
        {
            string[] propList = resObj.PropertyNames;
            Type tType = typeof(T);

            var newObj = (T)Activator.CreateInstance(tType, new object[1] { resObj.ConnectionManager });

            if (newObj.HasLazyProperties)
                resObj.Get();

            IEnumerable<PropertyInfo> propInfo = tType.GetProperties(
                (BindingFlags)52).Where(
                    x => x.CanWrite);

            foreach (PropertyInfo prop in propInfo)
            {
                object wqlVal = TypeDictionary.GetWqlValue(resObj[prop.Name]);
                if (wqlVal != null)
                {
                    if (wqlVal is DateTime dtVal)
                    {
                        prop.SetValue(newObj, dtVal.ToUniversalTime());
                    }
                    else
                    {
                        prop.SetValue(newObj, wqlVal);
                    }
                }
            }
            if (newObj.NeedToInvoke != null && newObj.NeedToInvoke.Length > 0)
            {
                IEnumerable<MethodInfo> mis = tType.GetMethods(
                    BindingFlags.Instance | BindingFlags.NonPublic).Where(
                        x => newObj.NeedToInvoke.Contains(x.Name));

                foreach (MethodInfo mi in mis)
                {
                    mi.Invoke(newObj, null);
                }
            }
            return newObj;
        }

        #endregion

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
    }
}