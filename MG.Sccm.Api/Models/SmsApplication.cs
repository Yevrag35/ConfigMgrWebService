using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace MG.Sccm.Api.Models
{
    public class SmsApplication : BaseWmiObject
    {
        #region FIELDS/CONSTANTS


        #endregion

        #region PROPERTIES
        [SccmInteger]
        public int CI_ID { get; set; }
        [SccmString("CI_UniqueId")]
        public string CI_UniqueId { get; set; }
        [SccmInteger]
        public int CIVersion { get; set; }
        [SccmDateTime("DateCreated")]
        public DateTime Created { get; set; }
        [SccmString]
        public string CreatedBy { get; set; }
        [SccmBool]
        public bool HasContent { get; set; }
        [SccmBool]
        public bool IsDeployed { get; set; }
        [SccmBool]
        public bool IsEnabled { get; set; }
        [SccmDateTime("DateLastModified")]
        public DateTime LastModified { get; set; }
        [SccmString]
        public string LastModifiedBy { get; set; }
        [SccmString]
        public string Manufacturer { get; set; }
        [SccmString]
        public string ModelName { get; set; }
        [SccmString("LocalizedDisplayName")]
        public string Name { get; set; }
        [SccmInteger]
        public int NumberOfDependentDeploymentTypes { get; set; }
        [SccmInteger]
        public int NumberOfDependentTaskSequences { get; set; }
        [SccmInteger]
        public int NumberOfDeployments { get; set; }
        [SccmInteger]
        public int NumberOfDeploymentTypes { get; set; }
        [SccmInteger]
        public int NumberOfDevicesWithApp { get; set; }
        [SccmInteger]
        public int NumberOfUsersWithApp { get; set; }
        [SccmString]
        public string SoftwareVersion { get; set; }

        #endregion

        #region CONSTRUCTORS
        public SmsApplication()
            : base()
        {
        }

        public SmsApplication(ConnectionManagerBase wql)
            : base(wql)
        {
        }

        #endregion

        #region PUBLIC METHODS


        #endregion

        #region BACKEND/PRIVATE METHODS


        #endregion
    }
}