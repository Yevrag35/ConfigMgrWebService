using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.Sccm.Api
{
    public class ConnectionCollection : IList<SmsConnection>, IDictionary
    {
        #region FIELDS/CONSTANTS
        private List<SmsConnection> _list;

        #endregion

        #region INDEXERS
        public SmsConnection this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public SmsConnection this[Guid sessionId] => _list.Find(x => x.SessionId.Equals(sessionId));

        public object this[object key]
        {
            get => _list.Find(x => x.SessionId.Equals(key) || x.UserHostAddress.Equals(key));
            set => throw new NotImplementedException();
        }

        #endregion

        #region PROPERTIES
        public int Count => _list.Count;
        bool IDictionary.IsFixedSize => false;
        public bool IsReadOnly => false;
        bool ICollection.IsSynchronized => false;
        ICollection IDictionary.Keys => _list.Select(x => x.UserHostAddress).ToList();
        ICollection IDictionary.Values => _list.Select(x => x.SessionId).ToList();
        object ICollection.SyncRoot => ((ICollection)_list).SyncRoot;

        #endregion

        #region CONSTRUCTORS
        public ConnectionCollection() => _list = new List<SmsConnection>();
        public ConnectionCollection(int capacity) => _list = new List<SmsConnection>(capacity);
        private ConnectionCollection(IEnumerable<SmsConnection> connections) => _list = new List<SmsConnection>(connections);

        #endregion

        #region PUBLIC METHODS
        public void Add(SmsConnection connection)
        {
            if (!_list.Exists(x => x.UserHostAddress.Equals(connection.UserHostAddress)))
                _list.Add(connection);
        }
        public SmsConnection Add(string userHostAddress, ConnectionManagerBase wql)
        {
            if (!_list.Exists(x => x.UserHostAddress.Equals(userHostAddress)))
            {
                var conn = new SmsConnection(wql)
                {
                    UserHostAddress = userHostAddress
                };
                _list.Add(conn);
                return conn;
            }
            else
            {
                return _list.Find(x => x.UserHostAddress.Equals(userHostAddress));
            }
        }
        void IDictionary.Add(object key, object value)
        {
            throw new NotImplementedException();
            //if (key is KeyValuePair<string, Guid> kvp && value is ConnectionManagerBase wql &&
            //    !_list.Exists(x => x.UserHostAddress.Equals(kvp.Key)))
            //{
            //    _list.Add(new SmsConnection(wql)
            //    {
            //        UserHostAddress = kvp.Key,
            //        SessionId = kvp.Value
            //    });
            //}
            //else if (key is Hashtable ht && value is ConnectionManagerBase wql2 &&
            //    ht.ContainsKey("UserHostAddress") && ht.ContainsKey("SessionId") && !_list.Exists(x => x.UserHostAddress.Equals(ht["UserHostAddress"])))
            //{
            //    _list.Add(new SmsConnection(wql2)
            //    {
            //        UserHostAddress = ht["UserHostAddress"] as string,
            //        SessionId = Guid.Parse(ht["SessionId"] as string)
            //    });
            //}
        }
        public void Clear() => _list.Clear();
        bool IDictionary.Contains(object key) => _list.Exists(x => x.UserHostAddress.Equals(key));
        public bool Contains(SmsConnection item) => _list.Contains(item);
        void ICollection.CopyTo(Array array, int index) => ((ICollection)_list).CopyTo(array, index);
        public void CopyTo(SmsConnection[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
        public bool Exists(Predicate<SmsConnection> match) => _list.Exists(match);
        public SmsConnection Find(Predicate<SmsConnection> match) => _list.Find(match);
        public ConnectionCollection FindAll(Predicate<SmsConnection> match) => new ConnectionCollection(_list.FindAll(match));
        public IEnumerator<SmsConnection> GetEnumerator() => _list.GetEnumerator();
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            var dict = new Dictionary<string, Guid>(_list.Count);
            for (int i = 0; i < _list.Count; i++)
            {
                SmsConnection conn = _list[i];
                dict.Add(conn.UserHostAddress, conn.SessionId);
            }
            return dict.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
        public int IndexOf(SmsConnection item) => _list.IndexOf(item);
        public void Insert(int index, SmsConnection item) => _list.Insert(index, item);
        public bool Remove(SmsConnection item)
        {
            item.Dispose();
            return _list.Remove(item);
        }
        public void RemoveAt(int index)
        {
            SmsConnection conn = _list[index];
            this.Remove(conn);
        }
        public void Remove(object key)
        {
            if (key is Guid sessionId)
            {
                SmsConnection conn = _list.Find(x => x.SessionId.Equals(sessionId));
                this.Remove(conn);
            }
            else if (key is string userHostAddress)
            {
                SmsConnection conn2 = _list.Find(x => x.UserHostAddress.Equals(userHostAddress));
                this.Remove(conn2);
            }
        }

        #endregion

        #region BACKEND/PRIVATE METHODS


        #endregion
    }
}