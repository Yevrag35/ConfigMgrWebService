using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConfigMgrWebService
{
    public class CMApplicationDependency : CMApplication
    {
        public int DependentDeploymentType { get; set; }
        public int TargetingDeploymentType { get; set; }
        public DependencyType TypeFlag { get; set; }

        public CMApplicationDependency() { }

        public CMApplicationDependency(IResultObject ires)
        {
            using (ires)
            {
                string appQuery = string.Format("SELECT * FROM SMS_ApplicationLatest WHERE CI_ID={0}", ires["ToApplicationCIID"].IntegerValue);

                using (IResultObject appResults = ires.ConnectionManager.QueryProcessor.ExecuteQuery(appQuery))
                {
                    foreach (IResultObject app in appResults)
                    {
                        using (app)
                        {
                            ApplicationName = app["LocalizedDisplayName"].StringValue;
                            ApplicationDescription = app["LocalizedDescription"].StringValue;
                            ApplicationManufacturer = app["Manufacturer"].StringValue;
                            ApplicationVersion = app["SoftwareVersion"].StringValue;
                            ApplicationCreated = app["DateCreated"].DateTimeValue;
                            ApplicationExecutionContext = app["ExecutionContext"].StringValue;
                            CI_ID = app["CI_ID"].StringValue;
                            NumberOfDependencies = app["NumberOfDependentDTs"].IntegerValue;
                        }
                    }
                }
                DependentDeploymentType = ires["ToDeploymentTypeCIID"].IntegerValue;
                TargetingDeploymentType = ires["FromDeploymentTypeCIID"].IntegerValue;
                TypeFlag = (DependencyType)ires["TypeFlag"].IntegerValue;
            }
        }
        
        public enum DependencyType
        {
            NoAutoInstall = 2,
            AutoInstall = 3
        }
    }
}