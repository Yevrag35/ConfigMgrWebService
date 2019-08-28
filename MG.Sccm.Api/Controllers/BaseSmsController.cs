using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using Newtonsoft.Json;
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
    public abstract class BaseSmsController : ApiController
    {
        private const string BAD_HEADER_MSG = "Missing \"sccm-session-id\" header.";

        public bool TryGetConnection(out SmsConnection connection)
        {
            bool result = false;
            connection = null;
            if (this.TryGetSessionIdFromHeaders(out Guid sessionId) && SmsConnectionManager.ConnectedSessions.Exists(x => x.SessionId.Equals(sessionId)))
            {
                connection = SmsConnectionManager.ConnectedSessions[sessionId];
                result = true;
            }
            return result;
        }

        protected IHttpActionResult ThrowHeaderError() => base.ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, BAD_HEADER_MSG));

        private bool TryGetSessionIdFromHeaders(out Guid sessionId)
        {
            bool result = false;
            sessionId = Guid.Empty;
            if (Guid.TryParse(HttpContext.Current.Request.Headers["sccm-session-id"] as string, out Guid sesId))
            {
                sessionId = sesId;
                result = true;
            }
            return result;
        }
    }

    public static class SmsConnectionManager
    {
        internal static ConnectionCollection ConnectedSessions { get; set; }
        [Obsolete]
        internal static ConnectionManagerBase Connection { get; set; }
        //internal static bool IsConnected => Connection != null && Connection.NamedValueDictionary[]
    }
}
