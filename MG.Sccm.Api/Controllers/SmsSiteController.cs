using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace MG.Sccm.Api
{
    [Route("api/sites")]
    public class SmsSiteController : BaseSmsController
    {
        [HttpGet]
        [Route("api/sites/current")]
        public IHttpActionResult GetCurrentSite()
        {
            if (base.TryGetConnection(out SmsConnection connection))
            {
                var ires = (IResultObject)connection.Connection.NamedValueDictionary["ConnectedSite"];
                SmsSite site = SccmSerialization.DeserializeIResult<SmsSite>(ires);
                return Ok(site);
            }
            else
            {
                return base.ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Missing \"sccm-session-id\" header."));
            }
        }
    }
}
