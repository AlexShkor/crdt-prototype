using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core
{
    public class StringDataEntry : DataEntry
    {
        public override JTokenType Type
        {
            get
            {
                return JTokenType.String;
            }
        }

        public string Value { get; set; }

        public override JToken ToJToken()
        {
            JToken token = Value;
            return token;
        }
    }
}
