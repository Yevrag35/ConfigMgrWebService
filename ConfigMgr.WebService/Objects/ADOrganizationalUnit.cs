using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConfigMgr
{
    public class ADOrganizationalUnit
    {
        public string Name { set; get; }
        public string DistinguishedName { set; get; }
        public bool HasChildren { set; get; }
    }
}