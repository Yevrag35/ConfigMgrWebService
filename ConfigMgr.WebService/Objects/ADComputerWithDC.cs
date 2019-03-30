using System;
using System.DirectoryServices;

namespace ConfigMgr
{
    public class ADComputerWithDC : ADComputer
    {
        public string RespondingDC { get; set; }

        public ADComputerWithDC() { }

        public ADComputerWithDC(DirectoryEntry de, string respDC)
            : base(de) => this.RespondingDC = respDC;
    }
}
