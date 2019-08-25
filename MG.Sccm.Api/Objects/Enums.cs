using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MG.Sccm.Api
{
    public enum AADAgentType : int
    {
        User = 1,
        Device = 2,
        Group = 3
    }

    public enum AccountTypes : int
    {
        User = 0,
        Group = 1,
        Machine = 2,
        UnverifiedUser = 128,
        UnverifiedGroup = 129,
        UnverifiedMachine = 130
    }

    public enum AlertFeatureGroup : int
    {
        Administration = 1,
        Resources = 2,
        Deployment = 3,
        Monitoring = 4,
        Reporting = 5
    }

    public enum AlertRuleState : int
    {
        Unknown = -1,
        Bad = 0,
        Good = 1
    }

    public enum AlertSeverity : int
    {
        Error = 1,
        Warning = 2,
        Informational = 3
    }

    public enum AlertStatus : int
    {
        Active = 0,
        Postponed = 1,
        Canceled = 2,
        Unknown = 3,
        Disabled = 4,
        NeverTriggered = 5
    }

    public enum AppExecutionContext : int
    {
        System = 0,
        User = 1
    }

    public enum AppLogonRequirement : int
    {
        Others = 0,
        LogonRequired = 1
    }

    public enum AppReqCurrentState : int
    {
        Requested = 1,
        Canceled = 2,
        Denied = 3,
        Approved = 4
    }

    public enum Architecture : int
    {
        x86 = 0,
        x64 = 1,
        ia64 = 2,
    }

    public enum BaselineClientStatus : int
    {
        Compliant = 1,
        InProgress = 2,
        FalsetCompliant = 3,
        CriticalError = 4
    }

    public enum BaselineItemType : int
    {
        PatchOrCU = 1,
        LanguagePack = 2
    }

    public enum BaselinePlatform : int
    {
        x86 = 1,
        x64 = 2
    }

    public enum BaselineType : int
    {
        Production = 1,
        Staging = 2
    }

    public enum BoundaryFlag : int
    {
        Fast = 0,
        Slow = 1
    }

    public enum BoundaryType : int
    {
        IPSubnet = 0,
        ADSite = 1,
        IPv6Prefix = 2,
        IPRange = 3
    }

    public enum CertType : int
    {
        BootMedia = 1,
        DistributionPoint = 2,
        ISVProxy = 3,
        SMSIssuing = 4,
        SccmGeneratedSiteSystem = 5,
        WsusSigning = 6
    }

    public enum ClientOperationState : int
    {
        Unknown = 0,
        FalsetApplicable = 1,
        Failed = 2,
        Succeeded = 3
    }

    public enum ClientSettingType : int
    {
        Device = 1,
        User = 2
    }

    public enum CollectionType : int
    {
        Other = 0,
        User = 1,
        Device = 2
    }

    public enum DesiredConfigType : int
    {
        Install = 1,
        Uninstall = 2,
        Monitor = 3,
        Enforce = 4
    }

    public enum FeatureType : int
    {
        Application = 1,
        Program = 2,
        MobileProgram = 3,
        Script = 4,
        SoftwareUpdate = 5,
        Baseline = 6,
        TaskSequence = 7,
        ContentDistribution = 8,
        DistributionPointGroup = 9,
        DistributionPointHealth = 10,
        ConfigurationPolicy = 11,
        AbstractConfigurationItem = 28
    }

    public enum ForestDiscoveryStatus : int
    {
        Succeeded = 0,
        Completed = 1,
        AccessDenied = 2,
        Failed = 3,
        Stopped = 4
    }
    public enum ForestPublishingStatus : int
    {
        Unknown = 0,
        Succeeded = 1,
        Failed = 2
    }

    public enum HealthStatus : int
    {
        Green,
        Yellow,
        Red
    }

    public enum InboxHealthStatus : int
    {
        Normal,
        Warning,
        Critical
    }

    public enum KeyType : int
    {
        SelfSigned = 1,
        Issued = 2
    }

    public enum ObjectType : int
    {
        PkgTypeRegular = 0,
        PkgTypeDriver = 3,
        PkgTypeTaskSequence = 4,
        PkgTypeSoftwareUpdates = 5,
        PkgTypeDeviceSettings = 6,
        PkgContentPackage = 8,
        PkgTypeImage = 257,
        PkgTypeBootImage = 258,
        PkgTypeOSInstallImage = 259,
        Application = 512
    }

    public enum ObjectTypeId : int
    {
        CIAssignment = 200,
        Advertisement = 201
    }

    public enum OptionalComponents : int
    {
        DismCmdletsx86 = 1,
        Dot3Svcx86 = 2,
        EnhancedStoragex86 = 3,
        FMAPIx86 = 4,
        FontSupportJAJPx86 = 5,
        FontSupportKOKRx86 = 6,
        FontSupportZHCNx86 = 7,
        FontSupportZHHKx86 = 8,
        FontSupportZHTWx86 = 9,
        HTAx86 = 10,
        StorageWMIx86 = 11,
        LegacySetupx86 = 12,
        MDACx86 = 13,
        NetFxx86 = 14,
        PowerShellx86 = 15,
        PPPoEx86 = 16,
        RNDISx86 = 17,
        Scriptingx86 = 18,
        SecureStartupx86 = 19,
        Setupx86 = 20,
        SetupClientx86 = 21,
        SetupServerx86 = 22,
        WDSToolsx86 = 24,
        WinReCfgx86 = 25,
        WMIx86 = 26,
        DismCmdletsx64 = 27,
        Dot3Svcx64 = 28,
        EnhancedStoragex64 = 29,
        FMAPIx64 = 30,
        FontSupportJAJPx64 = 31,
        FontSupportKOKRx64 = 32,
        FontSupportZHCNx64 = 33,
        FontSupportZHHKx64 = 34,
        FontSupportZHTWx64 = 35,
        HTAx64 = 36,
        StorageWMIx64 = 37,
        LegacySetupx64 = 38,
        MDACx64 = 39,
        NetFxx64 = 40,
        PowerShellx64 = 41,
        PPPoEx64 = 42,
        RNDISx64 = 43,
        Scriptingx64 = 44,
        SecureStartupx64 = 45,
        Setupx64 = 46,
        SetupClientx64 = 47,
        SetupServerx64 = 48,
        WDSToolsx64 = 50,
        WinReCfgx64 = 51,
        WMIx64 = 52,
        NetFx4x86 = 53,
        PowerShell3x86 = 54,
        NetFx4x64 = 55,
        PowerShell3x64 = 56,
        SecureBootCmdletsx86 = 57,
        SecureBootCmdletsx64 = 58
    }

    //public enum PackageType : int
    //{
    //    Regular = 0,
    //    Driver = 3,
    //    TaskSequence = 4,
    //    SoftwareUpdate = 5,
    //    DeviceSetting =  6,
    //    VirtualApp = 7,
    //    Application = 8,
    //    Image = 257,
    //    BootImage = 258,
    //    OSInstall = 259,
    //    VHD = 260
    //}

    public enum PrimaryActionTargetObjectType : int
    {
        Threat = 1,
        RequestPolicyFalsew = 8
    }

    public enum PrimaryActionType : int
    {
        FullScan = 1,
        QuickScan = 2,
        DownloadDefinition = 3,
        EvaluateSoftwareUpdate = 4,
        ExcludeScanPath = 5,
        OverrideDefaultAction = 6,
        RestoreQuarantineItems = 7,
        DownloadComputerPolicy = 8,
        DownloadUserPolicy = 9,
        CollectDiscoveryData = 10,
        CollectSoftwareInventory = 11,
        CollectHardwareInventory = 12,
        EvaluateAppDeployments = 13,
        EvaluateSoftwareUpdateDeployments = 14,
        SwitchToNextSUP = 15,
        EvaluateHealthAttestation = 16
    }

    public enum RefreshType : int
    {
        Manual = 1,
        Perodic = 2,
        ConstantUpdate = 4,
        Both = 6
    }

    public enum RoleFileType : int
    {
        Empty = 0,
        Actual = 1,
        Proposed = 2,
        Transactions = 4,
        LocalTransactions = 6
    }

    [Flags]
    public enum RoleName : int
    {
        SiteServer = 0,
        ComponentServer = 1,
        StateMigrationPoint = 2,
        DistributionPoint = 4,
        ManagementPoint = 8,
        SQLServer = 16,
        SystemHealthValidator = 32,
        DeviceManagementPoint = 64,
        ServerLocationPoint = 4096,
    }

    public enum SettingType : int
    {
        Antimalware = 0,
        ClientSettings = 1
    }

    public enum SiteMode : int
    {
        Normal = 0,
        InMaintenance = 1,
        Recovering = 2,
        Upgrading = 3,
        EvaluationTimedOut = 4,
        SiteExpansion = 5,
        InteropNonUpgradedPrimarySites = 6,
        InteropNonUpgradedSecondarySites = 7
    }

    public enum SSLState : int
    {
        HTTP = 0,
        HTTPS = 1,
        NotApplicable = 2,
        AlwaysHTTPS = 3,
        AlwaysHTTP = 4
    }

    public enum SiteStatus : int
    {
        Active = 1,
        Pending = 2,
        Failed = 3,
        Deleted = 4,
        Upgrade = 5,
        FailedToDeleteOrDeinstallSecondarySite = 6,
        FailedToUpgradeSecondarySite = 7,
        SecondarySiteRecovering = 8,
        SecondarySiteRecoveryFailure = 9
    }

    public enum SiteType : int
    {
        Secondary = 1,
        Primary = 2,
        CAS = 4
    }

    [Flags]
    public enum UpdateImpact : byte
    {
        SiteServer = 0x1,
        Console = 0x2,
        Client = 0x4,
        NewFeatures = 0x8,
        BugFixes = 0x10
    }

    public enum UpdateState : int
    {
        Downloaded = 131074,
        Installed = 196612,
        ReadyToInstall = 262146,
        Other = 327682
    }

    public enum UpdateType : int
    {
        Regular,
        Weave,
        QFE
    }
}