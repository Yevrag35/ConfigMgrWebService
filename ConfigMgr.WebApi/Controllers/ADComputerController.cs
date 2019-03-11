using ConfigMgr.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ConfigMgr.WebApi
{
    [SimpleAuth]
    [WindowsAuth]
    [Authorize(Roles = "Full Administrator")]
    public class ADComputerController : ApiController
    {
        private static ADHelper Helper = new ADHelper();

        [HttpGet]
        [Route("api/ad/computer")]
        public string Get()
        {
            return Helper.GetADObject("DGRLAB-SCCM", ADObjectClass.Computer, ADObjectType.DistinguishedName);
        }
    }
}
