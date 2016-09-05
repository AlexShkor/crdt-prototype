using Newtonsoft.Json.Linq;

namespace Crdt.Core
{
    public abstract class DataEntry
    {
        public string Name { get; set; }

        public abstract JTokenType Type { get; }

        public abstract JToken ToJToken();
    }
}