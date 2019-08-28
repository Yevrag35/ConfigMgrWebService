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
                return base.ThrowHeaderError();
            }
        }

        [HttpGet]
        public IHttpActionResult GetSites([FromUri]string siteCode = null, [FromUri]int? buildNumber = null, [FromUri]int? mode = null,
            [FromUri]string parentSiteCode = null, [FromUri]string siteName = null, [FromUri]int? status = null, [FromUri]int? type = null)
        {
            if (base.TryGetConnection(out SmsConnection connection))
            {
                if (HttpContext.Current.Request.QueryString.Count <= 0)
                {
                    var ex = new ArgumentNullException("At least query parameter is required: 'siteCode', 'buildNumber', 'mode', 'parentSiteCode', 'siteName', 'status', or 'type'.");
                    return base.ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex));
                }

                var queryList = new List<string>(2);
                if (!string.IsNullOrEmpty(siteCode))
                    queryList.Add(string.Format("SiteCode = \"{0}\"", siteCode));

                if (buildNumber.HasValue)
                    queryList.Add(string.Format("BuildNumber = {0}", buildNumber.Value));

                if (mode.HasValue)
                    queryList.Add(string.Format("Mode = {0}", mode.Value));

                if (!string.IsNullOrEmpty(parentSiteCode))
                    queryList.Add(string.Format("ReportingSiteCode = \"{0}\"", parentSiteCode));

                if (!string.IsNullOrEmpty(siteName))
                    queryList.Add(string.Format("SiteName = \"{0}\"", siteName));

                if (status.HasValue)
                    queryList.Add(string.Format("Status = {0}", status.Value));

                if (type.HasValue)
                    queryList.Add(string.Format("Type = {0}", type.Value));

                string oneStr = string.Join(" AND ", queryList);
                string fullQuery = string.Format("SELECT * FROM SMS_Site WHERE {0}", oneStr);

                IResultObject ires = connection.Connection.QueryProcessor.ExecuteQuery(fullQuery);
                var list = new List<SmsSite>();
                foreach (IResultObject site in ires)
                {
                    SmsSite smsSite = SccmSerialization.DeserializeIResult<SmsSite>(site);
                    list.Add(smsSite);
                }
                return base.ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, list));
            }
            else
            {
                return base.ThrowHeaderError();
            }
        }
    }
}
