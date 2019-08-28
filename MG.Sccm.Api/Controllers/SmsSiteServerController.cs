using MG.Sccm.Api.Models;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace MG.Sccm.Api
{
    [Route("api/siteServers")]
    public class SmsSiteServerController : BaseSmsController
    {
        [HttpGet]
        public IHttpActionResult GetAllServers([FromUri]string siteCode = null)
        {
            if (base.TryGetConnection(out SmsConnection connection))
            {
                var list = new List<SmsSiteServer>();

                string query = "SELECT * FROM SMS_SystemResourceList";
                if (!string.IsNullOrEmpty(siteCode))
                {
                    query = query + " WHERE SiteCode = \"" + siteCode + "\"";
                }

                using (IResultObject objCol = connection.Connection.QueryProcessor.ExecuteQuery(query))
                {
                    foreach (IResultObject result in objCol)
                    {
                        using (result)
                        {
                            SmsSiteServer sss = SccmSerialization.DeserializeIResult<SmsSiteServer>(result);
                            list.Add(sss);
                        }
                    }
                }
                return base.ResponseMessage(base.Request.CreateResponse(HttpStatusCode.OK, list));
            }
            else
            {
                return base.ThrowHeaderError();
            }
        }

        [HttpGet]
        public IHttpActionResult GetByRole([FromUri]string role)
        {
            if (base.TryGetConnection(out SmsConnection connection))
            {
                var list = new List<SmsSiteServer>();

                string query = string.Format("SELECT * FROM SMS_SystemResourceList WHERE RoleName = \"SMS {0}\"", role);
                using (IResultObject objCol = connection.Connection.QueryProcessor.ExecuteQuery(query))
                {
                    foreach (IResultObject result in objCol)
                    {
                        using (result)
                        {
                            SmsSiteServer sss = SccmSerialization.DeserializeIResult<SmsSiteServer>(result);
                            list.Add(sss);
                        }
                    }
                }
                return base.ResponseMessage(base.Request.CreateResponse(HttpStatusCode.OK, list));
            }
            else
            {
                return base.ThrowHeaderError();
            }
        }

        [HttpGet]
        public IHttpActionResult GetByName([FromUri]string name)
        {
            if (base.TryGetConnection(out SmsConnection connection))
            {
                var list = new List<SmsSiteServer>();

                string query = string.Format("SELECT * FROM SMS_SystemResourceList WHERE ServerName LIKE \"{0}%\"", name);
                using (IResultObject objCol = connection.Connection.QueryProcessor.ExecuteQuery(query))
                {
                    foreach (IResultObject result in objCol)
                    {
                        using (result)
                        {
                            SmsSiteServer sss = SccmSerialization.DeserializeIResult<SmsSiteServer>(result);
                            list.Add(sss);
                        }
                    }
                }
                JContainer jcon = this.CombineRoles(list);
                return base.ResponseMessage(base.Request.CreateResponse(HttpStatusCode.OK, jcon));
            }
            else
            {
                return base.ThrowHeaderError();
            }
        }

        private JContainer CombineRoles(List<SmsSiteServer> servers)
        {
            var serverNames = servers.Select(x => x.Name).Distinct().ToList();

            if (serverNames.Count > 1)
            {
                var jar = new JArray();
                for (int i = 0; i < serverNames.Count; i++)
                {
                    string name = serverNames[i];
                    JObject job = this.OneServerObject(name, servers.Where(x => x.Name.Equals(name)));
                    jar.Add(job);
                }
                return jar;
            }
            else
            {
                return this.OneServerObject(serverNames[0], servers);
            }
        }

        private JObject OneServerObject(string name, IEnumerable<SmsSiteServer> servers)
        {
            var job = new JObject
            {
                { "serverName", JToken.FromObject(name) }
            };
            var innerJar = new JArray();
            foreach (SmsSiteServer sss in servers)
            {
                innerJar.Add(JToken.FromObject(sss));
            }
            job.Add("components", innerJar);
            return job;
        }
    }
}
