using ConfigMgr.Enums;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace ConfigMgr.WebApi
{
    public abstract class ADController : EventController
    {
        private const SearchScope SUB = SearchScope.Subtree;
        private const string DC_REGEX = @"^[D|d][C|c]\=(\S{1,})";

        protected private abstract string[] PropsToLoad { get; }

        #region AD METHODS

        protected private DirectoryEntry FindObject(string gcStr, string ldapFilter, string scope)
        {
            DirectoryEntry de = null;
            SearchScope sc = this.ResolveScope(scope);
            using (var gcEnt = new DirectoryEntry(gcStr))
            {
                using (var ds = new DirectorySearcher(gcEnt, ldapFilter, this.PropsToLoad, sc))
                {
                    SearchResult sr = ds.FindOne();
                    if (sr != null)
                        de = sr.GetDirectoryEntry();
                }
            }
            return de;
        }

        protected private SearchResultCollection FindObjects(string gcStr, string ldapFilter, string scope)
        {
            SearchScope sc = this.ResolveScope(scope);
            using (var gcEnt = new DirectoryEntry(gcStr))
            {
                using (var ds = new DirectorySearcher(gcEnt, ldapFilter, this.PropsToLoad, sc))
                {
                    return ds.FindAll();
                }
            }
        }

        public Domain GetDomainFromOUPath(string ouPath)
        {
            string[] dcStrs = ouPath.Split(
                new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries).Where(
                    x => x.StartsWith("DC=", StringComparison.CurrentCultureIgnoreCase)).ToArray();

            string[] cutOut = new string[dcStrs.Length];
            for (int i = 0; i < dcStrs.Length; i++)
            {
                cutOut[i] = Regex.Match(dcStrs[i], DC_REGEX).Groups[1].Value;
            }
            string fullDomain = string.Join(".", cutOut);
            return this.GetDomainFromString(fullDomain);
        }

        protected private Domain GetDomainFromString(string domainName)
        {
            DirectoryContext ctx = string.IsNullOrEmpty(domainName)
                ? new DirectoryContext(DirectoryContextType.Domain)
                : new DirectoryContext(DirectoryContextType.Domain, domainName);

            var retDom = Domain.GetDomain(ctx);
            return retDom;
        }

        protected private string GetRespondingDomainController(Domain domain, string dc)
        {
            string retName = null;
            if (string.IsNullOrEmpty(dc))
            {
                using (DomainController domCon = domain.FindDomainController())
                {
                    retName = domCon.Name;
                }
            }
            else retName = !dc.Contains(domain.Name) ? 
                    string.Format(Constants.FQDN_FORMAT, dc, domain.Name) :
                    dc;

            return retName;
        }

        #endregion

        #region HELPING PRIVATE METHODS
        private SearchScope ResolveScope(string scopeStr)
        {
            SearchScope sc = SUB;
            Type ssType = sc.GetType();
            if (!string.IsNullOrEmpty(scopeStr))
            {
                string[] matches = ssType.GetEnumNames().Where(
                    x => x.Equals(scopeStr, StringComparison.CurrentCultureIgnoreCase)).ToArray();

                if (matches.Length == 1)
                    sc = (SearchScope)Enum.Parse(ssType, matches[0]);
            }
            return sc;
        }

        #endregion
    }
}
