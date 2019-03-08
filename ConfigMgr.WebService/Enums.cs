using System;

namespace ConfigMgr.Enums
{
    public enum ADObjectClass
    {
        Computer = 0,
        DomainController = 1,
        Group = 2,
        User = 3
    }
    public enum ADObjectType
    {
        DistinguishedName = 0,
        ObjectGuid = 1
    }

    public enum CMObjectType
    {
        System = 5,
        User = 4
    }

    public enum CMCollectionType
    {
        UserCollection = 1,
        DeviceCollection = 2
    }

    public enum CMResourceProperty
    {
        Name = 0,
        ResourceID = 1,
        SMBIOSGUID = 2,
        CollectionID = 3
    }

    public enum LogTriggerAction
    {
        Begin = 0,
        End = 1
    }
}
