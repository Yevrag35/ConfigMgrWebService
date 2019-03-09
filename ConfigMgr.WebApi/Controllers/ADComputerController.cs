using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ConfigMgr.WebApi.Controllers
{
    [SimpleAuth]
    [WindowsAuth]
    [Authorize(Roles = "Full Administrator")]
    public class ADComputerController : ApiController
    {
        private static ADHelper Helper = new ADHelper();

        [HttpGet]
        [Route("api/ad/computer")]
        public IEnumerable<ADComputer> GetAll()
        {
            Helper.GetADObject()
        }
    }
}
