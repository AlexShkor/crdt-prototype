using Crdt.Core.Data;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Crdt.Core
{
    public class DocumentData
    {
        public string Id { get; set; }

        public Dictionary<string, DataEntry> Entries { get; set; }

        public DocumentData()
        {
            Entries = new Dictionary<string, DataEntry>();
        }

        public void ParseEntries(JObject data)
        {
            foreach (var field in data)
            {
                Entries.Add(field.Key, BuildDataEntry(field.Key, field.Value));
            }
        }

        public JObject ToJson()
        {
            var jo = new JObject();
            jo["id"] = Id;
            foreach (var entry in Entries)
            {
                jo[entry.Value.Name] = entry.Value.ToJToken();
            }
            return jo;
        }

        private DataEntry BuildDataEntry(string name, JToken field)
        {
            DataEntry dataEntry;
            var str = field.ToString();
            if (field.Type == JTokenType.Array)
            {
                var arrayEntry = new ArrayDataEntry();
                var values = (JArray)field;
                foreach (var token in values)
                {
                    var data = new DocumentData();
                    data.ParseEntries((JObject)token);
                    arrayEntry.AddItem(data);
                }
                dataEntry = arrayEntry;
            }
            else
            {
                dataEntry = new StringDataEntry() { Name = name, Value = field.Value<string>() };
            }
            dataEntry.Name = name;
            return dataEntry;
        }
    }
}