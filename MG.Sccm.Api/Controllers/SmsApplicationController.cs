using MG.Sccm.Api.Models;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
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
    [Route("api/apps")]
    public class SmsApplicationController : BaseSmsController
    {
        private const string APP_QUERY = "SELECT * FROM SMS_ApplicationLatest";

        [HttpGet]
        public IHttpActionResult GetAllApplications([FromUri]string name = null)
        {
            if (base.TryGetConnection(out SmsConnection connection))
            {
                var list = new List<SmsApplication>();
                string query = APP_QUERY;
                if (!string.IsNullOrEmpty(name))
                    query = string.Format(APP_QUERY + " WHERE LocalizedDisplayName LIKE \"%{0}%\"", name);

                using (IResultObject objCol = connection.Connection.QueryProcessor.ExecuteQuery(query))
                {
                    foreach (IResultObject result in objCol)
                    {
                        using (result)
                        {
                            SmsApplication smsApp = SccmSerialization.DeserializeIResult<SmsApplication>(result);
                            list.Add(smsApp);
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
        [Route("api/apps/{id}")]
        public IHttpActionResult GetApplication([FromUri]int id)
        {
            if (base.TryGetConnection(out SmsConnection connection))
            {
                string query = string.Format(APP_QUERY + " WHERE CI_ID = {0}", id);
                SmsApplication smsApp = null;
                using (IResultObject objCol = connection.Connection.QueryProcessor.ExecuteQuery(query))
                {
                    foreach (IResultObject result in objCol)
                    {
                        using (result)
                        {
                            smsApp = SccmSerialization.DeserializeIResult<SmsApplication>(result);
                            break;
                        }
                    }
                }
                return base.ResponseMessage(base.Request.CreateResponse(HttpStatusCode.OK, smsApp));
            }
            else
            {
                return base.ThrowHeaderError();
            }
        }
    }
}

/**
 *  if (base.TryGetConnection(out SmsConnection connection))
    {
                
    }
    else
    {
        return base.ThrowHeaderError();
    }
 *
**/