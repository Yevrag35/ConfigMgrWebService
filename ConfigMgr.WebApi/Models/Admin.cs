using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ConfigMgr.WebApi
{
    public class Admin
    {
        private IIdentity _id;

        public string Name { get; }
        public string AuthenticationType { get; }
        public IEnumerable<string> SccmRoles { get; }
        public bool IsWindowsIdentity => _id is WindowsIdentity;

        internal Admin(ApiPrincipal prin)
        {
            _id = prin.Identity;
            Name = _id.Name;
            AuthenticationType = _id.AuthenticationType;
            SccmRoles = prin.Roles;
        }
    }
}