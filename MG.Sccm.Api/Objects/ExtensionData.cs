using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MG.Sccm.Api
{
    public class ExtensionData : Dictionary<string, JToken>
    {
        //private bool Loaded = false;

        public ExtensionData() : base() { }
        public ExtensionData(IDictionary<string, JToken> dict) : base(dict) { }

        public void Add(KeyValuePair<string, JToken> kvp) => base.Add(kvp.Key, kvp.Value);
        public void AddPairs(IDictionary<string, JToken> dict)
        {
            //if (!this.Loaded)
            //{
                foreach (KeyValuePair<string, JToken> kvp in dict)
                {
                    this.Add(kvp);
                }
                //this.Loaded = true;
            //}
        }
    }
}