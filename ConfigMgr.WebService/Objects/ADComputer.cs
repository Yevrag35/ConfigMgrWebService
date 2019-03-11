using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace ConfigMgr
{
    public class ADComputer
    {
        //private static readonly string[] PropsToLoad = new string[4]
        //{
        //    Constants.DN, Constants.NAME, Constants.DNS_NAME, Constants.SAM
        //};

        public string SamAccountName { get; set; }
        public string Name { get; set; }
        public string DnsHostName { get; set; }
        public string DistinguishedName { get; set; }

        public ADComputer() { }

        //private ADComputer(string sam, string cn, string dns, string dn)
        //{
        //    this.SamAccountName = sam;
        //    this.Name = cn;
        //    this.DnsHostName = dns;
        //    this.DistinguishedName = dn;
        //}
        public ADComputer(DirectoryEntry dirEnt)
        {
            this.DistinguishedName = dirEnt.Properties[Constants.DN].Value.ToString();
            this.SamAccountName = dirEnt.Properties[Constants.SAM].Value.ToString();
            this.DnsHostName = dirEnt.Properties[Constants.DNS_NAME].Value.ToString();
            this.Name = dirEnt.Properties[Constants.CN].Value.ToString();
        }

        //public static ADComputer GetADComputer(Domain domain, string computerName)
        //{
        //    SearchResult sr = null;
        //    ADComputer adComp = null;
        //    using (var de = domain.GetDirectoryEntry())
        //    {
        //        string sFilter = string.Format("(&(objectClass=computer)((sAMAccountName={0}$)))", computerName);
        //        using (var ds = new DirectorySearcher(de, sFilter, PropsToLoad, SearchScope.Subtree))
        //        {
        //            sr = ds.FindOne();
        //        }
        //    }
        //    if (sr != null)
        //    {
        //        using (var objEntry = sr.GetDirectoryEntry())
        //        {
        //            adComp = new ADComputer(objEntry);
        //        }
        //    }
        //    return adComp;
        //}
    }
}