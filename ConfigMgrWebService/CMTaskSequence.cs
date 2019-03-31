using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConfigMgrWebService
{
    public class CMTaskSequence : BaseSms
    {
        public string Name { get; set; }
        public string PackageID { get; set; }
        public string AdvertisementId { get; set; }
        public int AdvertFlags { get; set; }
        public string Description { get; set; }
        public string BootImageID { get; set; }

        protected override string ClassName => "SMS_TaskSequencePackage";

        protected override bool HasLazyProperties => false;

        protected override string[] NeedToInvoke => null;

        protected override KeyValuePair<bool, string> PKMapping => new KeyValuePair<bool, string>(false, null);

        protected override string PrimaryKey => "PackageID";
    }
}