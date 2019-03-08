using ConfigMgr.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Net;

namespace ConfigMgr
{
    public interface IADHelper
    {
        #region ADD METHODS
        bool AddADComputerToGroup(string groupName, string computerName);
        bool AddADUserToGroup(string groupName, string userName);

        #endregion

        #region GET METHODS
        string GetADAttributeValue(string distinguishedName, string attributeName);
        IList GetADAttributeValues(string attributeName, string distinguishedName, IList valuesCollection, bool recursive);

        ADComputer GetADComputer(string computerName);
        string GetADComputerDescription(string computerName);

        string GetADDefaultNamingContext();
        ADDomain GetADDomain();
        string GetADDomainName();

        IList<ADGroup> GetADGroupsByUser(string userName);
        bool GetADGroupMemberByComputer(string computerName, string groupName, string domain);
        bool GetADGroupMemberByUser(string userName, string groupName, string domain);

        IList<string> GetADComputerMemberList(DirectoryEntry groupEntry);
        IList<string> GetADGroupMembers(string groupName);
        bool GetADGroupNestedMemberOf(Principal principal, GroupPrincipal group);

        string GetADObject(string name, ADObjectClass oClass, ADObjectType oType);

        IList<ADOrganizationalUnit> GetADOrganizationalUnits(string distinguishedName);

        string GetADSiteNameByIPAddress(string forestName, IPAddress ipAddress);
        IDictionary<string, string> GetADSubnets(string forestName);

        #endregion

        #region REMOVE METHODS
        bool RemoveADComputer(string samAccountName);
        bool RemoveADComputerFromGroup(string groupName, string computerName);

        #endregion

        #region SET METHODS
        bool SetADComputerDescription(string computerName, string description);
        bool SetADComputerManagedBy(string computerName, string userName);
        bool SetADOrganizationalUnitForComputer(string ouLocation, string computerName);

        #endregion
    }
}
