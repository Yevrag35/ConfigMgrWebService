using ConfigMgr.Enums;
using ConfigMgr.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Reflection;

namespace ConfigMgr
{
    public class ADHelper : Helper, IADHelper
    {
        #region METHODS

        #region ADD METHODS
        public bool AddADComputerToGroup(string groupName, string computerName)
        {
            this.OnLogTriggered(LogTriggerAction.Begin, MethodBase.GetCurrentMethod());
        }

        #endregion

        #region GET METHODS


        #endregion

        #region REMOVE METHODS


        #endregion

        #region SET METHODS


        #endregion

        #endregion
    }
}
