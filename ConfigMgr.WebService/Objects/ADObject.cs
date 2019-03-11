using ConfigMgr.Enums;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;

namespace ConfigMgr
{
    public abstract class ADObject
    {
        protected abstract string[] PropsToLoad { get; }
        protected abstract ADObjectClass ObjectClass { get; }

        public string DistinguishedName { get; set; }

        public static DirectoryEntry GetDirectoryEntry<T>(string name)
            where T : ADObject
        {
            return GetDirectoryEntry<T>(name, Domain.GetComputerDomain());
        }

        public static DirectoryEntry GetDirectoryEntry<T>(string name, Domain domain)
            where T : ADObject
        {
            DirectoryEntry dirEnt = null;
            using (var de = domain.GetDirectoryEntry())
            {
                T o = Activator.CreateInstance<T>();
                string sFilter = string.Format("(&(objectClass={0})((sAMAccountName={1}$)))", o.ObjectClass.ToString(), name);
                using (var ds = new DirectorySearcher(de, sFilter, o.PropsToLoad, SearchScope.Subtree))
                {
                    SearchResult sr = ds.FindOne();
                    if (sr != null)
                        dirEnt = sr.GetDirectoryEntry();
                }
            }
            return dirEnt;
        }
    }
}
