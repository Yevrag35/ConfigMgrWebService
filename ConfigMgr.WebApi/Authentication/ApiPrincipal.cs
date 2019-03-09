using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace ConfigMgr.WebApi
{
    public class ApiPrincipal : IPrincipal
    {
        public IIdentity Identity { get; }
        public IList<string> Roles { get; }
        public bool IsInRole(string role) => 
            Roles.Count > 0 && Roles.Any(x => string.Equals(x, role, StringComparison.InvariantCultureIgnoreCase));

        public ApiPrincipal(IIdentity identity, IEnumerable<string> roles)
        {
            Identity = identity;
            Roles = roles.ToList();
        }
    }
}