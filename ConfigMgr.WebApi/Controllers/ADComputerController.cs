using ConfigMgr.Enums;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace ConfigMgr.WebApi
{
    [SimpleAuth]
    [WindowsAuth]
    [Authorize(Roles = "Full Administrator")]
    public class ADComputerController : ADController
    {
        protected private override string[] PropsToLoad => new string[4]
        {
            Constants.DN, Constants.CN, Constants.DNS_NAME, Constants.SAM
        };

        [HttpGet]
        [Route("api/ad/computer")]
        public ADComputerWithDC GetADComputer([FromUri]string name, [FromUri]string domain = null, [FromUri]string dc = null)
        {
            var thisMeth = MethodBase.GetCurrentMethod();
            base.MethodBegin(thisMeth);

            ADComputerWithDC adComp = null;

            using (Domain dom = base.GetDomainFromString(domain))
            {
                string respDc = base.GetRespondingDomainController(dom, dc);

                using (DirectoryEntry de = dom.GetDirectoryEntry())
                {
                    string dn = de.Properties[Constants.DN].Value as string;
                    string gcForm = string.Format(Constants.GC_WITH_DC, respDc, dn);

                    string sFilter = string.Format(Constants.LDAP_FILTER_CLS_SAM, Constants.COMP, name);

                    DirectoryEntry resDe = base.FindObject(gcForm, sFilter, null);
                    if (resDe != null)
                    {
                        using (resDe)
                        {
                            adComp = new ADComputerWithDC(resDe, respDc);
                        }
                    }
                }
            }
            base.MethodEnd(thisMeth);
            return adComp;
        }

        [HttpGet]
        [Route("api/ad/computers")]
        public ADComputerCollection GetADComputers([FromUri]string ouPath = null, [FromUri]string dc = null, [FromUri]string searchScope = null)
        {
            var thisMeth = MethodBase.GetCurrentMethod();
            base.MethodBegin(thisMeth);

            Domain dom = !string.IsNullOrEmpty(ouPath) 
                ? base.GetDomainFromOUPath(ouPath) 
                : base.GetDomainFromString(null);

            using (dom)
            {
                if (string.IsNullOrEmpty(ouPath))
                {
                    using (DirectoryEntry de = dom.GetDirectoryEntry())
                    {
                        ouPath = de.Properties[Constants.DN].Value as string;
                    }
                }

                string respDc = base.GetRespondingDomainController(dom, dc);
                var compCol = new ADComputerCollection(respDc);

                string gcForm = string.Format(Constants.GC_WITH_DC, respDc, ouPath);
                string sFilter = string.Format(Constants.LDAP_FILTER_CLS, Constants.COMP);

                SearchResultCollection srCol = base.FindObjects(gcForm, sFilter, searchScope);

                compCol.AddComputers(srCol);
                base.MethodEnd(thisMeth);
                return compCol;
            }
        }
    }
}
