using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core
{
    public class StringDataEntry : DataEntry
    {
        public override string Type
        {
            get
            {
                return "String";
            }
        }

        public string Value { get; set; }
    }
}
