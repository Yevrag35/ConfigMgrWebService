using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConfigMgr
{
    public class CMTaskSequence
    {
        public string PackageName { get; set; }
        public string PackageID { get; set; }
        public string AdvertisementId { get; set; }
        public int AdvertFlags { get; set; }
        public string Description { get; set; }
        public string BootImageID { get; set; }
    }
}