using ConfigMgr.Enums;
using ConfigMgr.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;

namespace ConfigMgr.WebApi
{
    public class ADHelper : Helper
    {
        private const string ERR_MSG = "An error occured {0}. Error message: {1}";
        private const string ERR_ADD_COMP = "when attempting to add a computer object in Active Directory to a group";
        private string[] CompPropsToLoad => new string[4]
        {
            Constants.DN, Constants.CN, Constants.DNS_NAME, Constants.SAM
        };

        #region METHODS

        #region ADD METHODS
        public bool AddADComputerToGroup(string groupName, string computerName)
        {
            this.OnLogTriggered(LogTriggerAction.Begin, MethodBase.GetCurrentMethod());

            bool returnValue = false;
            string computerDN = this.GetADObject(computerName, ADObjectClass.Computer, ADObjectType.DistinguishedName).Remove(0, 7);
            string groupDN = this.GetADObject(groupName, ADObjectClass.Group, ADObjectType.DistinguishedName);

            if (!string.IsNullOrEmpty(computerDN) && !string.IsNullOrEmpty(groupDN))
            {
                try
                {
                    using (var grpEntry = new DirectoryEntry(groupDN))
                    {
                        grpEntry.Properties["member"].Add(computerDN);
                        grpEntry.CommitChanges();
                        returnValue = true;
                    }
                }
                catch (Exception ex)
                {
                    base.WriteEventLog(string.Format(ERR_MSG, ERR_ADD_COMP, ex.Message), EventLogEntryType.Error);
                }
            }
            this.OnLogTriggered(LogTriggerAction.End, MethodBase.GetCurrentMethod());
            return returnValue;
        }

        #endregion

        #region GET METHODS

        public ADComputer GetADComputer(string name, Domain domain)
        {
            var mb = MethodBase.GetCurrentMethod();
            this.OnLogTriggered(LogTriggerAction.Begin, mb);
            ADComputer adComp = null;
            using (var de = domain.GetDirectoryEntry())
            {
                string sFilter = string.Format("(&(objectClass=computer)((sAMAccountName={0}$)))", name);
                using (var ds = new DirectorySearcher(de, sFilter, CompPropsToLoad, SearchScope.Subtree))
                {
                    try
                    {
                        SearchResult sr = ds.FindOne();
                        using (var objEntry = sr.GetDirectoryEntry())
                        {
                            adComp = new ADComputer(objEntry);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            //Thread.Sleep(3000);
            this.OnLogTriggered(LogTriggerAction.End, mb);
            return adComp;
        }

        public string GetADDefaultNamingContext()
        {
            using (var rootDSE = new DirectoryEntry("LDAP://RootDSE"))
            {
                string dnc = rootDSE.Properties["defaultNamingContext"].Value.ToString();
                return dnc;
            }
        }

        public string GetADObject(string name, ADObjectClass oClass, ADObjectType oType)
        {
            string returnValue = string.Empty;
            SearchResult searchResult = null;

            string defaultNamingContext = GetADDefaultNamingContext();
            string currentDomain = string.Format("LDAP://{0}", defaultNamingContext);

            var domain = new DirectoryEntry(currentDomain);
            var directorySearcher = new DirectorySearcher(domain);
            directorySearcher.PropertiesToLoad.Add("distinguishedName");

            switch (oClass)
            {
                case ADObjectClass.DomainController:
                    directorySearcher.Filter = string.Format("(&(objectClass=computer)((dNSHostName={0})))", name);
                    break;
                case ADObjectClass.Computer:
                    directorySearcher.Filter = string.Format("(&(objectClass=computer)((sAMAccountName={0}$)))", name);
                    break;
                case ADObjectClass.Group:
                    directorySearcher.Filter = string.Format("(&(objectClass=group)((sAMAccountName={0})))", name);
                    break;
                case ADObjectClass.User:
                    directorySearcher.Filter = string.Format("(&(objectClass=user)((sAMAccountName={0})))", name);
                    break;
            }
            //' Invoke directory searcher
            try
            {
                searchResult = directorySearcher.FindOne();
            }
            catch (Exception ex)
            {
                WriteEventLog(string.Format("An error occured when attempting to locate Active Directory object. Error message: {0}", ex.Message), EventLogEntryType.Error);
                return returnValue;
            }

            //' Return selected object type value
            if (searchResult != null)
            {
                DirectoryEntry directoryObject = searchResult.GetDirectoryEntry();

                if (oType.Equals(ADObjectType.ObjectGuid))
                {
                    returnValue = directoryObject.Guid.ToString();
                }

                if (oType.Equals(ADObjectType.DistinguishedName))
                {
                    returnValue = string.Format("LDAP://{0}", directoryObject.Properties["distinguishedName"].Value);
                }
            }

            //' Dispose objects
            directorySearcher.Dispose();
            domain.Dispose();

            return returnValue;
        }

        #endregion

        #region REMOVE METHODS


        #endregion

        #region SET METHODS


        #endregion

        #endregion
    }
}
