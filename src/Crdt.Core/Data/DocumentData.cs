using System.Collections.Generic;

namespace Crdt.Core
{
    public class DocumentData
    {
        public string Id { get; set; }

        public Dictionary<string, DataEntry> Entries {get;set;}
    }
}