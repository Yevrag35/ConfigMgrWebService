using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using System;
using System.Configuration;

namespace ConfigMgr.WebApi
{
    public static class Constants
    {
        public const string CM_PACKAGE_QUERY = "SELECT * FROM SMS_Package";
        public const string CM_PACKAGE_QUERY_NAME_FILTER = CM_PACKAGE_QUERY + " WHERE " + NAME + " LIKE '%{0}%'";
        public const string CM_PACKAGE_QUERY_ID_FILTER = CM_PACKAGE_QUERY + " WHERE " + PACKAGE_ID + "='{0}'";
        public const string CM_PACKAGE_DRIVER_QUERY = CM_PACKAGE_QUERY + " WHERE " + NAME + " LIKE 'Drivers - %'";
        public const string CM_PACKAGE_DRIVER_MANF_QUERY = CM_PACKAGE_DRIVER_QUERY + " AND " + MANUFACTURER + "='{0}'";

        public const string TS_METH_GET_SEQUENCE = "GetSequence";
        public const string TS_PACKAGE_PKGID = TS_SMS_TS + "." + PACKAGE_ID + "='{0}'";
        public const string TS_PACKAGE_PKGID_QUERY = "SELECT * FROM " + TS_SMS_TS + "Reference WHERE " + PACKAGE_ID + "='{0}'";
        public const string TS_SMS_TS = "SMS_" + TS_PACKAGE;
        public const string TS_SMS_AOSA = "SMS_" + TS + "_ApplyOperatingSystemAction";
        public const string TS_SMS_GRP = "SMS_" + TS + "_Group";
        public const string TS_SMS_UOSA = "SMS_" + TS + "_UpgradeOperatingSystemAction";
        public const string TS_IMAGE_QUERY = "SELECT * FROM SMS_ImageInformation WHERE " + PACKAGE_ID + "='{0}' AND " + INDEX + "='{1}'";
        public const string TS_IMAGE_PKG_QUERY = "SELECT * FROM SMS_ImagePackage WHERE " + PACKAGE_ID + "='{0}'";
        public const uint OS_IMAGE_TYPE = 257;
        public const uint OS_IMAGE_INSTALL_TYPE = 259;

        public const string ARCHITECTURE = "Architecture";
        public const string CI_ID = "CI_ID";
        public const string CREATE_DATE = "CreationDate";
        public const string DU_CLASS = "__CLASS";
        public const string DESCRIPTION = "Description";
        public const string IMAGE_INDEX = "Image" + INDEX;
        public const string INDEX = "Index";
        public const string INSTALL_ED_INDEX = "InstallEdition" + INDEX;
        public const string LANGUAGE = "Language";
        public const string LAST_REFRESH_TIME = "LastRefreshTime";
        public const string MANUFACTURER = "Manufacturer";
        public const string NAME = "Name";
        public const string OBJECT_ID = "ObjectID";
        public const string OBJECT_TYPE = "ObjectType";
        public const string OS_VERSION = "OS" + VERSION;
        public const string PACKAGE_ID = "PackageID";
        public const string PKG_FLAGS = "PkgFlags";
        public const string PKG_SIZE = "Package" + SIZE;
        public const string PKG_SOURCE_PATH = "PkgSourcePath";
        public const string PRODUCT_TYPE = "ProductType";
        public const string ROLE_NAMES = "RoleNames";
        public const string SIZE = "Size";
        public const string SOURCE_DATE = "SourceDate";
        public const string SOURCE_SITE = "SourceSite";
        public const string SOURCE_VERS = "Source" + VERSION;
        public const string STEPS = "Steps";
        public const string STORED_PKG_VERSION = "StoredPkg" + VERSION;
        public const string TS = "TaskSequence";
        public const string TS_PACKAGE = TS + "Package";
        public const string VERSION = "Version";

        public const string PRIMARY_SITE_SERVER = "PrimarySiteServer";
        public const string SITE_CODE = "SiteCode";
        public const string SMS_ADMIN_QUERY = "SELECT * FROM SMS_Admin WHERE AdminID='{0}'";
        public const string SQL_CONNECTION_STRING = "Server={0};Initial Catalog=CM_{1};Integrated Security=true;Encrypt=true;TrustServerCertificate=yes;";

        public const string RBAC_ADMIN_FUNCTION = @"SELECT dbo.fn_rbac_GetAdminIDsfromUserSIDs('{0}')";
    }

    public static class Methods
    {
        public static WqlConnectionManager NewSMSProvider()
        {
            string serverName = ConfigurationManager.AppSettings[Constants.PRIMARY_SITE_SERVER];

            // Used for Testing
            //string username = ConfigurationManager.AppSettings["UserName"];
            //string password = ConfigurationManager.AppSettings["Password"];

            var nvs = new SmsNamedValuesDictionary();
            var wql = new WqlConnectionManager(nvs);

            //wql.Connect(serverName, username, password);
            wql.Connect(serverName);
            return wql;
        }
    }
}