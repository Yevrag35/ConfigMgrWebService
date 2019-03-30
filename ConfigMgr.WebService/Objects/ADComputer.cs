using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace ConfigMgr
{
    public class ADComputer
    {
        public string SamAccountName { get; set; }
        public string Name { get; set; }
        public string DnsHostName { get; set; }
        public string DistinguishedName { get; set; }

        public ADComputer() { }

        public ADComputer(DirectoryEntry dirEnt)
        {
            this.DistinguishedName = dirEnt.Properties[Constants.DN].Value.ToString();
            this.SamAccountName = dirEnt.Properties[Constants.SAM].Value.ToString();
            //this.DnsHostName = dirEnt.Properties[Constants.DNS_NAME].Value.ToString();
            object dnsName = dirEnt.Properties[Constants.DNS_NAME].Value;
            this.DnsHostName = dnsName != null
                ? dnsName.ToString()
                : null;

            this.Name = dirEnt.Properties[Constants.CN].Value.ToString();
        }
    }
}