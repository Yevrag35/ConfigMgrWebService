using ConfigMgr.Enums;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ConfigMgr.WebApi
{
    [SimpleAuth]
    [WindowsAuth]
    [Authorize(Roles = "Full Administrator")]
    public class ADComputerController : ApiController
    {
        [HttpGet]
        [Route("api/ad/computer")]
        public ADComputer GetADComputer([FromUri]string name, [FromUri]string domain = null)
        {
            Methods.WriteApiLog();
            ADComputer adComp = null;
            string userName = HttpContext.Current.User.Identity.Name;
            string hostAddress = HttpContext.Current.Request.UserHostAddress;
            DirectoryContext ctx = string.IsNullOrWhiteSpace(domain)
                ? new DirectoryContext(DirectoryContextType.Domain)
                : new DirectoryContext(DirectoryContextType.Domain, domain);

            using (var dom = Domain.GetDomain(ctx))
            {
                adComp = WebApiConfig.Helper.GetADComputer(name, dom, hostAddress, userName);
            }
            return adComp;
        }
    }
}
