using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace ConfigMgr.WebApi
{
    public class ADComputerCollection
    {
        private List<ADComputer> _list;

        public int Count => _list.Count;
        public string RespondingDC { get; set; }

        public IEnumerable<ADComputer> Computers => _list;

        public ADComputerCollection() => _list = new List<ADComputer>();

        public ADComputerCollection(string respDc)
            : this() => this.RespondingDC = respDc;

        internal ADComputerCollection(IEnumerable<ADComputer> comps, string respondingDc)
        {
            _list = new List<ADComputer>(comps);
            this.RespondingDC = respondingDc;
        }

        internal void AddComputer(ADComputer adComp) => _list.Add(adComp);
        private void AddComputer(DirectoryEntry de) =>
            this.AddComputer(new ADComputer(de));

        internal void AddComputers(IEnumerable<ADComputer> comps) => _list.AddRange(comps);

        internal void AddComputers(SearchResultCollection resultCollection)
        {
            for (int i = 0; i < resultCollection.Count; i++)
            {
                SearchResult sr = resultCollection[i];
                using (DirectoryEntry de = sr.GetDirectoryEntry())
                {
                    this.AddComputer(de);
                }
            }
        }

        internal void AddComputers(ADComputerCollection compCol)
        {
            if (string.IsNullOrEmpty(this.RespondingDC))
            {
                this.RespondingDC = compCol.RespondingDC;
            }
            _list.AddRange(compCol._list);
        }
    }
}
