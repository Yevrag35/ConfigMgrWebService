using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace MG.Sccm.Api
{
    [Route("api/connection")]
    public class ConnectionController : ApiController
    {
        [HttpPost]
        public IHttpActionResult NewConnection([FromUri]string serverName)
        {
            var wql = new WqlConnectionManager(new SmsNamedValuesDictionary());
            string hostAddress = HttpContext.Current.Request.UserHostAddress;
            bool exists = SmsConnectionManager.ConnectedSessions.Exists(x => x.UserHostAddress.Equals(hostAddress));
            if (exists)
            {
                ConnectionCollection col = SmsConnectionManager.ConnectedSessions.FindAll(x => x.UserHostAddress.Equals(hostAddress));
                for (int i = 0; i < col.Count; i++)
                {
                    SmsConnection conn = col[i];
                    if (conn.Connection.NamedValueDictionary["ProviderLocation"] is ManagementObject provLoc &&
                        !provLoc.Path.Server.Equals(serverName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SmsConnectionManager.ConnectedSessions.Remove(conn);
                    }
                }
            }

            SmsConnection connection = SmsConnectionManager.ConnectedSessions.Add(hostAddress, wql);
            try
            {
                string pass = File.ReadAllText("C:\\Admin\\Pass.txt");

                if (connection.Connection.Connect(serverName, "YEVRAG35\\Mike", pass))
                {
                    HttpStatusCode code = !exists
                        ? HttpStatusCode.Created
                        : HttpStatusCode.Accepted;

                    return ResponseMessage(Request.CreateResponse(code, connection));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return e.InnerException.GetType().Equals(typeof(ManagementException))
                    ? ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, serverName + " is not the name of a valid SCCM server."))
                    : ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteConnections()
        {
            bool removedAny = false;
            for (int i = SmsConnectionManager.ConnectedSessions.Count - 1; i >= 0; i--)
            {
                SmsConnection conn = SmsConnectionManager.ConnectedSessions[i];
                if (conn.UserHostAddress.Equals(HttpContext.Current.Request.UserHostAddress))
                {
                    SmsConnectionManager.ConnectedSessions.Remove(conn);
                    removedAny = true;
                }
            }

            return removedAny
                ? StatusCode(HttpStatusCode.NoContent)
                : StatusCode(HttpStatusCode.NotFound);
        }

        [HttpDelete]
        public IHttpActionResult DeleteConnection([FromUri]Guid id)
        {
            if (SmsConnectionManager.ConnectedSessions.Exists(x => x.SessionId.Equals(id)))
            {
                SmsConnectionManager.ConnectedSessions.Remove(id);
                return base.StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return base.NotFound();
            }
        }
    }
}
